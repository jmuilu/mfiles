using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using MFilesAPI;

namespace BBMRIData
{
    // <summary>
    //  Wrapper for MFiles properties
    // </summary>

    public class ExtPropertyDef
    {
        public PropertyDef PropertyDef { get; set;}
        public int LookupID { get; set ; }
        public int LookupTypeID { get; set; }

        public ExtPropertyDef()
        {
            LookupID = -1; 
        }
    }

    public class DataLoader
    {
        public char cDelimiter { get; set; }
        private MFilesAPI.Vault oVault;

        public DataLoader(MFilesAPI.Vault vault)
        {
            this.oVault = vault;
            cDelimiter = '\t';
        }

        public void load(int iWorkFow, int iState, string szFilePath, int iObjType, int iObjClass, 
            int[] iPropDefIDs, 
            string[] szSelectedColumns, int iTiteProperty, string[] szTitleName )
        {
            int DBUG = 2;

            if (iPropDefIDs.Length != szSelectedColumns.Length)
            {
                throw new Exception("PropertyDefs and selected columns do not mactch");
            }

            MFilesAPI.ObjectClass oClass = oVault.ClassOperations.GetObjectClass(iObjClass);
            //oClass.NamePropertyDef

            //lookup PropertyDefs so we have type info later
            ExtPropertyDef[] oPropDefs = new ExtPropertyDef[iPropDefIDs.Length];
            for (int i = 0; i < iPropDefIDs.Length; i++)
            {
                oPropDefs[i] = new ExtPropertyDef();
                oPropDefs[i].PropertyDef = oVault.PropertyDefOperations.GetPropertyDef(iPropDefIDs[i]);
                if (szSelectedColumns[i].Contains('@'))
                {   // TODO
                    if (oPropDefs[i].PropertyDef.DataType != MFDataType.MFDatatypeLookup && oPropDefs[i].PropertyDef.DataType != MFDataType.MFDatatypeMultiSelectLookup)
                    {
                        throw new Exception("Lookup defined for non lookup data type. Column=" + szSelectedColumns[i]);
                    }
                    string[] v = szSelectedColumns[i].Split('@');
                    if (v.Length != 3) throw new Exception("lookup column must have three components separated by @");
                    szSelectedColumns[i] = v[0];
                    oPropDefs[i].LookupID = Int32.Parse(v[1]);
                    oPropDefs[i].LookupTypeID = Int32.Parse(v[2]);
                }
                else
                {
                    if (oPropDefs[i].PropertyDef.DataType == MFDataType.MFDatatypeLookup || oPropDefs[i].PropertyDef.DataType == MFDataType.MFDatatypeMultiSelectLookup)
                    {
                        throw new Exception("Lookup must be defind for lookup data type. Column=" + szSelectedColumns[i]);
                    }

                }
            }


            // Read the file line by line.
            using (System.IO.StreamReader file = new System.IO.StreamReader(szFilePath))
            {
                string line;
                line = file.ReadLine();
                string[] headers = line.Split(cDelimiter);
                int[] iSelectedColumns = new int[ szSelectedColumns.Length ];
                int[] iTitleColumns = new int[szTitleName.Length];

                int j = 0;
                int h = 0;
                for (int i = 0; i < headers.Length; i++)
                {
                    foreach (string v in szSelectedColumns)
                    {
                        if (String.Equals(v, headers[i] ))
                        {
                            iSelectedColumns[ j] = i; //save position of selected column in flat file
                            j++;
                            break;
                        }
                    }
                    foreach (string v in szTitleName)
                    {
                        if (String.Equals(v, headers[i]))
                        {
                            iTitleColumns[h] = i; //save position of selected column in flat file
                            h++;
                            break;
                        }
                    }
                }
                if (j != szSelectedColumns.Length)
                {
                    throw new Exception("Selected columns were not correct... check column headers");
                }
                if (h != szTitleName.Length)
                {
                    throw new Exception("Selected columns were not correct... check column headers for title");
                }
                while ((line = file.ReadLine()) != null)
                {
                    string[] values = line.Split(cDelimiter);
                    string[] selectedValues = new string[iSelectedColumns.Length];
                    for( int i = 0; i < selectedValues.Length ; i++ ) 
                    {
                        selectedValues[i] = values[iSelectedColumns[i] ]; //take values from selected columns
                        
                    }
                    string lookupTitle = createLookupTitle(selectedValues, iTitleColumns);
                    ObjectVersion find = MFilesUtil.MF_GetTheLatestVisibleObjectVersionByTitle(oVault, iObjClass, lookupTitle);
                    if (find == null)
                    {
                        this.createObject(iWorkFow,iState, iObjType, iObjClass, oPropDefs, selectedValues, iTiteProperty,lookupTitle);
                    }
                    else
                    {
                        //Console.Out.WriteLine(lookupTitle + " already loaded ");
                        oVault.ObjectOperations.RemoveObject(find.ObjVer.ObjID);
                        this.createObject(iWorkFow, iState, iObjType, iObjClass, oPropDefs, selectedValues, iTiteProperty, lookupTitle);
                    }
                    if (DBUG-- == 0)
                    {
                        break; //FOR DEBUGGING - remove this
                    }
                }
                file.Close();
            }

        }

        private string createLookupTitle(string[] values, int[] iTitleColumns)
        {
            string key = values[0];
            for ( int i = 1; i < iTitleColumns.Length ; i++) 
            {
                key = key+"-"+values[i] ;
            }
            return key;
        }

        protected void createObject( int iWorkFlow, int iState, int iObjType, int iObjClass, ExtPropertyDef[] oPropDefs, string[] values, int iTitleProperty, string szTitle)
        {
            PropertyValues props = new PropertyValues();

            //Create Class Property Deliberatley // ask: to link ObjectType to its class ?
            PropertyValue classProp = new PropertyValue();
            classProp.PropertyDef = (int)MFBuiltInPropertyDef.MFBuiltInPropertyDefClass;
            classProp.Value.SetValue(MFDataType.MFDatatypeLookup, iObjClass);
            props.Add(-1, classProp);


            //Create other Properties based on defs/line values
            for (int i = 0; i < oPropDefs.Length; i++)
            {
                PropertyValue propVal = new PropertyValue();
                propVal.PropertyDef = oPropDefs[i].PropertyDef.ID;
                //prop 
                if (oPropDefs[i].PropertyDef.DataType == MFDataType.MFDatatypeLookup)
                {
                    ObjectVersion find = null;
                    if (oPropDefs[i].LookupTypeID == MF_TYPE.VALUE_LIST)
                    {
                        ValueListItem item = MFilesUtil.MF_GetTheValueListItemByName(oVault, MFilesAPI.MFConditionType.MFConditionTypeEqual, oPropDefs[i].LookupID, values[i]);
                        propVal.Value.SetValue(oPropDefs[i].PropertyDef.DataType, item.ID);
                    }
                    else
                    {
                        find = MFilesUtil.MF_GetTheLatestVisibleObjectVersionByTitle(oVault, oPropDefs[i].LookupID, values[i]);
                        if (find == null)
                        {
                            throw new Exception("Cannot create object. Lookup value is missing. ID=" + values[i] + " ObjectClass ID=" + oPropDefs[i].LookupID);
                        }
                        Lookup lookup = new MFilesAPI.Lookup();
                        lookup.Item = find.ObjVer.ID;
                        propVal.Value.SetValueToLookup(lookup);
                        //propVal.Value.SetValue(oPropDefs[i].PropertyDef.DataType, find.ObjVer.ID);
                    }
                }
                else
                {
                    if (oPropDefs[i].PropertyDef.DataType == MFDataType.MFDatatypeMultiSelectLookup)
                    {
                        ObjectVersion find = MFilesUtil.MF_GetTheLatestVisibleObjectVersionByTitle(oVault, oPropDefs[i].LookupID, values[i]);
                        if (find == null)
                        {
                            throw new Exception("Cannot create object. Lookup value is missing. ID=" + values[i] + " ObjectClass ID=" + oPropDefs[i].LookupID);
                        }
                        Lookups lookups = new MFilesAPI.Lookups();
                        Lookup lookup = new MFilesAPI.Lookup();
                        lookup.Item = find.ObjVer.ID;
                        lookups.Add(0,lookup);
                        propVal.Value.SetValueToMultiSelectLookup(lookups);
                    }
                    else
                    {
                        propVal.Value.SetValue(oPropDefs[i].PropertyDef.DataType, values[i]);
                    }
                }
                //propVal.Value(
                props.Add(-1, propVal);
            }

            if (iState > 0)
            {
                PropertyValue pv = new MFilesAPI.PropertyValue() ;
                pv.PropertyDef = 39 ;  //39 iWorkFlow;
                pv.TypedValue.SetValue(MFDataType.MFDatatypeLookup, iState);
                props.Add(-1, pv);
                pv = new MFilesAPI.PropertyValue();
                pv.PropertyDef = 38;
                pv.TypedValue.SetValue(MFDataType.MFDatatypeLookup, iWorkFlow);
                props.Add(-1, pv);
            }

            PropertyValue titlePv = new MFilesAPI.PropertyValue();
            titlePv.PropertyDef = iTitleProperty;  //iWorkFlow;
            titlePv.TypedValue.SetValue(MFDataType.MFDatatypeText, szTitle);
            props.Add(-1, titlePv);

            //Create Object
            //todo: check in manually after import (m_vault.ObjectOperations.CheckIn(oObjectVersionAndProperties.ObjVer)..
            ObjectVersionAndProperties ovap = oVault.ObjectOperations.CreateNewObjectEx(
                iObjType,
                props,
                new SourceObjectFiles(),
                false,
                true,
                null
            );
            Console.WriteLine("Created Object {0}-{1}", iObjType, ovap.ObjVer.ID);

        }



    }
    class ObjectProperty
    {
        private MFilesAPI.PropertyValue oPropertyValue;
        private MFilesAPI.PropertyDef oPropertyDef;
        public ObjectProperty(MFilesAPI.PropertyValue oPropertyValue, MFilesAPI.PropertyDef oPropertyDef)
        {
            this.oPropertyDef = oPropertyDef;
            this.oPropertyValue = oPropertyValue;
        }

        public MFilesAPI.PropertyValue PropertyValue
        {
            get { return oPropertyValue; }
        }

        public MFilesAPI.TypedValue TypedValue
        {
            get { return oPropertyValue.TypedValue; }
        }

        public MFilesAPI.PropertyDef PropertyDef
        {
            get { return oPropertyDef; }
        }

        public string Name
        {
            get { return oPropertyDef.Name; }
        }

        public string DisplayValue
        {
            get { return oPropertyValue.TypedValue.DisplayValue; }
        }


    }

    // <summary>
    //  Wrapper class for MFiles objects
    // </summary>
    class MFilesObject
    {
        private MFilesAPI.Vault oVault;
        private MFilesAPI.ObjectVersion oObjectVersion;
        private List<ObjectProperty> lObjecProperties = new List<ObjectProperty>();
        private Hashtable hObjectProperties = new Hashtable();
        private Hashtable hObjectPropertiesID = new Hashtable();

        public MFilesObject(MFilesAPI.Vault oVault, MFilesAPI.ObjectVersion oObjectVersion)
        {
            this.oVault = oVault;
            //oVault.ObjectPropertyOperations(
            this.oObjectVersion = oObjectVersion;
            MFilesAPI.PropertyValues oPropVals = oVault.ObjectPropertyOperations.GetProperties(oObjectVersion.ObjVer);
            foreach (MFilesAPI.PropertyValue oPV in oPropVals)
            {
                PropertyDef prop = oVault.PropertyDefOperations.GetPropertyDef(oPV.PropertyDef);
                ObjectProperty op = new ObjectProperty(oPV, prop);
                lObjecProperties.Add(op);
                if ( hObjectProperties[prop.Name] != null ) 
                {
                    throw new Exception("BBMRIData.MFilesObject: Property name ("+prop.Name+ ") is not unique in Object (" + Title+")");
                }
                hObjectProperties[prop.Name] = op;
                hObjectPropertiesID[prop.ID] = op;
            }
        }

        public ObjectProperty GetObjectProperty(string Name)
        {
            ObjectProperty op = (ObjectProperty) hObjectProperties[Name];
            if (op == null)
            {
                throw new Exception("BBMRIData.MFilesObject: Property name (" + Name + ") does not exist in Object (" + Title + ")");
            }
            return op;
        }

        public ObjectProperty GetObjectProperty(int  id)
        {
            ObjectProperty op = (ObjectProperty)hObjectPropertiesID[id];
            if (op == null)
            {
                throw new Exception("BBMRIData.MFilesObject: Property name (" + id + ") does not exist in Object (" + Title + ")");
            }
            return op;
        }

        public List<ObjectProperty> ObjectProperties
        {
            get { return lObjecProperties; }
        }

        public MFilesAPI.ObjType ObjType 
        {
            get {
                MFilesAPI.ObjType oObjType = default(MFilesAPI.ObjType);
                oObjType = oVault.ObjectTypeOperations.GetObjectType(oObjectVersion.ObjVer.Type);
                return oObjType;
            }
        }
        public MFilesAPI.ObjectVersionWorkflowState WorkflowState
        {
            get {
                MFilesAPI.ObjectVersionWorkflowState oState = oVault.ObjectPropertyOperations.GetWorkflowState(oObjectVersion.ObjVer);
                return oState;
            }
        }

        public MFilesAPI.ObjectFiles ObjectFiles
        {
            get
            {
                MFilesAPI.ObjectFiles oFiles = oVault.ObjectFileOperations.GetFiles(oObjectVersion.ObjVer);
                return oFiles;
            }
        }

        public string Title
        {
            get { return oObjectVersion.Title; }
        }

        public string CreatedString
        {
            get {  return oObjectVersion.LastModifiedUtc.ToLongTimeString() ;}
        }

        public string TypeNameSingular
        {
            get { return ObjType.NameSingular; }
        }

        public string TypeNamePlural
        {
            get { return ObjType.NamePlural; }
        }

        public MFilesAPI.ObjectVersion ObjectVersion
        {
            get { return oObjectVersion; }
        }


    }

    // <summary>
    //  Utility methods
    // </summary>
    class MFilesUtil
    {



        static public int MF_GetTheClassIDByName(MFilesAPI.Vault oVault, MFilesAPI.MFConditionType eCondition, string szClassName)
        {

            ValueListItem res = MF_GetTheClassByName(oVault, eCondition, szClassName);
            return res.ID;
        }

        static public ValueListItem MF_GetTheClassByName(MFilesAPI.Vault oVault, MFilesAPI.MFConditionType eCondition, string szClassName)
        {
            int iValueList = (int)MFBuiltInValueList. MFBuiltInValueListClasses; // search from iternal value list!!
            return MF_GetTheValueListItemByName(oVault, eCondition, iValueList, szClassName);
        }

        /*
         * Get value list item from the value list by unique name, including internal lists like classes, class groups
         * 
         * oVault     - Vault
         * eCondition - Type of value list. Internal lists are defined in MFBuiltInValueList
         * szName     - Name
         * 
         * Exception:
         * - If name is notfound or name is not unique (i.e. more than one values are returned)
         */
        static public ValueListItem MF_GetTheValueListItemByName(MFilesAPI.Vault oVault, MFilesAPI.MFConditionType eCondition, int iValueList, string szName)
        {

            // Set the search conditions for the value list item.
            MFilesAPI.SearchCondition oScValueListItem = new MFilesAPI.SearchCondition();

            MFValueListItemPropertyDef eProp = MFilesAPI.MFValueListItemPropertyDef.MFValueListItemPropertyDefName;
            oScValueListItem.Expression.SetValueListItemExpression(eProp, MFilesAPI.MFParentChildBehavior.MFParentChildBehaviorNone);
            oScValueListItem.ConditionType = eCondition;

            oScValueListItem.TypedValue.SetValue(MFilesAPI.MFDataType.MFDatatypeText, szName);
            MFilesAPI.SearchConditions arrSearchConditions = new MFilesAPI.SearchConditions();
            arrSearchConditions.Add(-1, oScValueListItem);

            // Search for the value list item.
            MFilesAPI.ValueListItemSearchResults results = oVault.ValueListItemOperations.SearchForValueListItemsEx(iValueList, arrSearchConditions);
            if (results.Count > 0)
            {
                // Found.
                if (results.Count > 1)
                {
                    throw new Exception("BBMRIData.MFilesUtil: Entity name is not unique. Name = " + szName);
                }
            }
            else
            {
                // Not found.
                throw new Exception("BBMRIData.MFilesUtil: Entity not found. Name = " + szName);
            }
            return results[1];
        }

        static public ValueListItemSearchResults MF_GetAllValueListItems(MFilesAPI.Vault oVault, int iValueList, MFilesAPI.SearchConditions arrSearchConditions)
        {
            MFilesAPI.ValueListItemSearchResults results = oVault.ValueListItemOperations.SearchForValueListItemsEx(iValueList, arrSearchConditions);
            return results;
        }

        /*
         * Get all objects which are instances of given class
         */ 
        static public MFilesAPI.ObjectSearchResults MF_GetObjectsByClassId(MFilesAPI.Vault oVault, int iClass)
        {

            // Create a search condition for the object class.
            MFilesAPI.SearchCondition oSearchCondition = new MFilesAPI.SearchCondition();
            oSearchCondition.ConditionType = MFilesAPI.MFConditionType.MFConditionTypeEqual;
            oSearchCondition.Expression.DataPropertyValuePropertyDef = (int)MFilesAPI.MFBuiltInPropertyDef.MFBuiltInPropertyDefClass;

            oSearchCondition.TypedValue.SetValue(MFilesAPI.MFDataType.MFDatatypeLookup, iClass);
            // Invoke the search operation.
            MFilesAPI.ObjectSearchResults oObjectVersions = oVault.ObjectSearchOperations.SearchForObjectsByCondition(oSearchCondition, false);
            return oObjectVersions;

        }

        /*
         * Get the latest object by unique title or name 
         * Exception:
         * - If object is not found or it is deleted or object is not unique
         */
        static public MFilesAPI.ObjectVersion MF_GetTheLatestVisibleObjectVersionByTitle(MFilesAPI.Vault oVault, int iID, string szTitle)
        {

            MFilesAPI.ObjectSearchResults oObjectVersions = MF_GetLatestVisibleObjectsByTitle(oVault, iID, (int)MFilesAPI.MFBuiltInPropertyDef.MFBuiltInPropertyDefNameOrTitle, szTitle);
            ObjectVersion oObjVersion = null; 
            foreach (ObjectVersion oV in oObjectVersions)
            {
                if (oV.LatestCheckedInVersionOrCheckedOutVersion && ! oV.Deleted)
                {
                    if (oObjVersion != null)
                    {
                        throw new Exception("MFilesUtil: Object title " + szTitle + " not unique. Created "+oV.LastModifiedUtc.ToLongTimeString()+" "+oObjVersion.LastModifiedUtc.ToLongTimeString());
                    }
                    oObjVersion = oV;
                }
            }
            if (oObjVersion == null)
            {
                //not tested
                throw new Exception("Object "+szTitle+" not found. ID="+iID);
            }
            return oObjVersion;
        }

        static public ValueListItem MF_GetTheValueListItemByDisplayID(MFilesAPI.Vault oVault, int iValueListObject, string value) 
        {
            ValueListItem items= oVault.ValueListItemOperations.GetValueListItemByDisplayID(iValueListObject, value);
            if (items == null)
            {
                throw new Exception("Value list item " + value + " not found in value list. Value list ID=" + iValueListObject);
            }
            return items; 
        }

        static public MFilesAPI.ValueListItemSearchResults GetValueListItems(MFilesAPI.Vault oVault, int iValueListObject, int iValue)
        {
            MFilesAPI.SearchCondition oScValueListItem = new SearchCondition();
            oScValueListItem.Expression.SetValueListItemExpression(MFilesAPI.MFValueListItemPropertyDef.MFValueListItemPropertyDefID, MFilesAPI.MFParentChildBehavior.MFParentChildBehaviorNone);
            oScValueListItem.ConditionType = MFilesAPI.MFConditionType.MFConditionTypeEqual;
            oScValueListItem.TypedValue.SetValue(MFilesAPI.MFDataType.MFDatatypeLookup, iValueListObject);
            MFilesAPI.SearchConditions arrSearchConditions = new MFilesAPI.SearchConditions();
            arrSearchConditions.Add(-1, oScValueListItem);
   
            // Search for the value list item.
             MFilesAPI.ValueListItemSearchResults oResults = oVault.ValueListItemOperations.SearchForValueListItems(iValue,arrSearchConditions);
             return oResults;
        }

 
        //new
        static public MFilesAPI.ObjectSearchResults MF_GetLatestVisibleObjectsByTitle(MFilesAPI.Vault oVault, int iClass, int iPropertyType, string szTitle)
        {

            // Create a search condition for the object class.

            MFilesAPI.SearchConditions oSearchConditions = new SearchConditions();

            MFilesAPI.SearchCondition oSearchByClass = new MFilesAPI.SearchCondition();
            oSearchByClass.ConditionType = MFilesAPI.MFConditionType.MFConditionTypeEqual;
            oSearchByClass.Expression.DataPropertyValuePropertyDef = (int)MFilesAPI.MFBuiltInPropertyDef.MFBuiltInPropertyDefClass;
            oSearchByClass.TypedValue.SetValue(MFilesAPI.MFDataType.MFDatatypeLookup, iClass);
            oSearchConditions.Add(-1, oSearchByClass);

            MFilesAPI.SearchCondition oSearchByTitle = new MFilesAPI.SearchCondition();
            oSearchByTitle.ConditionType = MFilesAPI.MFConditionType.MFConditionTypeEqual;
            oSearchByTitle.Expression.DataPropertyValuePropertyDef = iPropertyType ; //(int)MFilesAPI.MFBuiltInPropertyDef.MFBuiltInPropertyDefNameOrTitle;
            oSearchByTitle.TypedValue.SetValue(MFilesAPI.MFDataType.MFDatatypeText, szTitle);

            oSearchConditions.Add(-1, oSearchByTitle);


            // Invoke the search operation.
            MFilesAPI.ObjectSearchResults oObjectVersions = oVault.ObjectSearchOperations.SearchForObjectsByConditions(oSearchConditions, MFSearchFlags.MFSearchFlagLookInAllVersions, false);
            return oObjectVersions;

        }

    }

}
