using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MFilesAPI;

namespace BBMRIData
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// http://www.developerfusion.com/tools/convert/vb-to-csharp/

    public partial class MainWindow : Window
    {

        MFilesAPI.MFilesServerApplication oServerApp;
        MFilesAPI.VaultsOnServer oVaults;
        MFilesAPI.Vault oSelectedVault;

        const string Root = @"C:\Temp\";

        void OnSelect(object sender, RoutedEventArgs e)
        {
            //get valut from user selection and login as default user
            ListBoxItem item = e.Source as ListBoxItem;
            String g = item.Tag as String;
            oSelectedVault = oServerApp.LogInToVault(g);

            // get all data files (should get only those which are needed... todo: improve the query)
            int iClass = MF_CLASS.PARTICIPANT_DATA_FILE_MULTI_PARTICIPANT;
            MFilesAPI.ObjectSearchResults oObjectVersions = MFilesUtil.MF_GetObjectsByClassId(oSelectedVault, iClass);

            // Process all data file objects.
            foreach (MFilesAPI.ObjectVersion oObjectVersionTmp in oObjectVersions)
            {

                MFilesAPI.ObjectVersion oObjectVersion = oSelectedVault.ObjectOperations.GetObjectInfo(oObjectVersionTmp.ObjVer, true); //ask: True = ?                
                MFilesObject obj = new MFilesObject(oSelectedVault, oObjectVersion);
                ObjectProperty oFormat = obj.GetObjectProperty(MF_PTYPE.FORMAT);


                //Take only documents which are awaiting processing 
                if (oFormat != null &&
                    oFormat.DisplayValue.Equals(MF_DATA_FORMAT.QPATI_TEST_EXPORT) &&
                    obj.WorkflowState.State.TypedValue.GetLookupID() == MF_WFLOW_STATE.STORED_AWAITS_PROCESSING)
                {

                    string basicData = null;
                    string diagnosisData = null;

                    console.AppendText(">>>ID" + obj.GetObjectProperty(MF_PTYPE.FORMAT).Name + "<< " + obj.GetObjectProperty(MF_PTYPE.FORMAT).DisplayValue);
                    listBox1.Items.Add(obj.TypeNameSingular + ": " + " ID=" + obj.ObjType.ID + obj.Title + ". State: " + obj.WorkflowState.State.Value.DisplayValue +
                        " Val=" + obj.WorkflowState.State.TypedValue.GetLookupID());

                    console.AppendText(obj.Title + Environment.NewLine);
                    foreach (ObjectProperty oP in obj.ObjectProperties)
                    {
                        console.AppendText("  PROP: " + oP.Name + ":" + " PROP ID: " + oP.PropertyDef.ID + " " + oP.DisplayValue + Environment.NewLine);

                    }
                    foreach (ObjectFile oF in obj.ObjectFiles)
                    {
                        string newFileName = Guid.NewGuid().ToString() + "_" + oF.Title + "." + oF.Extension;
                        newFileName = System.IO.Path.Combine(Root, newFileName);
                        oSelectedVault.ObjectFileOperations.DownloadFile(oF.ID, oF.Version, newFileName);
                        console.AppendText("  FILE: " + oF.Title + " " + newFileName + " " + Environment.NewLine);

                        if (oF.Title.Equals("Uusi Basic_Data"))
                        {
                            basicData = newFileName;
                        }
                        if (oF.Title.Equals("Uusi Diagnosis"))
                        {
                            diagnosisData = newFileName;
                        }

                    }
                    //basic data:
                    //sample	bbmri_participant_id	oberon	date	age	gender	subjectid_tmp	life_status	dod	age_of_death	biobank
                    //B2-93	64573		19930104	22464,04264	M	585524	DEAD	20000205	25061	VSSHP

                    //diagnosis
                    //ID	bbmri_sample_id	sample	bbmri_participant	organ_snomed	organ_pre	organ_text	organ_info	diag_snomed	diag_text	diag_info	biobnk
                    //1	B2-93-T63600	B2-93	64573	T63600		PYLORIC ANTRUM		M43000	CHRONIC GASTRITIS		VSSHP



                    DataLoader ldr = new DataLoader(oSelectedVault);

                    if (false && basicData != null)
                    {
                        ldr.load(MF_WORKFLOWS.PATIENT_STATE,MF_STATES.CONSENTED, basicData, MF_OTYPE.PARTICIPANT, MF_CLASS.PARTICIPANT,
                            new int[] { MF_PTYPE.LOCAL_PARTICIPANT_ID, MF_PTYPE.GENDER, MF_PTYPE.BIOBANK },
                            new string[] { "bbmri_participant_id", "gender@" + MF_VLIST.GENDERS + "@" + MF_TYPE.VALUE_LIST, "biobank@" + MF_CLASS.BIOBANK + "@" + MF_TYPE.OBJECT }, // biobank is lookup column
                            MF_PTYPE.TITLE_PARTICIPANT,
                            new string[] { "bbmri_participant_id" });

                    }
                    else
                    {
                        //throw new Exception("Basic file not found");

                    }
                    if (diagnosisData != null)
                    {

                        ldr.load(-1,-1,diagnosisData, MF_OTYPE.SAMPLE, MF_CLASS.SAMPLE,
                            new int[] { MF_PTYPE.LOCAL_SAMPLE_ID, MF_PTYPE.ORGAN_SNOMED, MF_PTYPE.ORGAN_TEXT, MF_PTYPE.PARTICIPANT },
                            new string[] { "sample", "organ_snomed", "organ_text", "bbmri_participant@" + MF_CLASS.PARTICIPANT +"@"+ MF_TYPE.OBJECT }, // participant is lookup column
                            MF_PTYPE.TITLE_SAMPLE,
                            new string[] { "bbmri_sample_id" }); // TITLE_SAMPLE

                        //ldr.load(diagnosisData, 155, 86, new int[] { 1297, 1294,1293}, new string[] { "sample@85", "diag_text", "diag_snomed" });
                        //ObjectVersion find = MFilesUtil.MF_GetTheObjectVersionByTitle(oSelectedVault, 85, "B2-93");

                        //MFilesObject objtmp = new MFilesObject(oSelectedVault, find);
                        //console.AppendText("Diagnis data loaded.. FOUND: " + objtmp.Title+" "+objtmp.CreatedString);
                    }
                    else
                    {
                        //throw new Exception("Diagnosis file not found");
                    }
                }

            }


        }

        public MainWindow()
        {
            InitializeComponent();

            oServerApp = new MFilesAPI.MFilesServerApplication();
            //oVaultConnection = new MFilesAPI.MFServerConnection();
            oServerApp.Connect(MFAuthType.MFAuthTypeLoggedOnWindowsUser);
            oVaults = oServerApp.GetVaults();

            for (int i = 1; i <= oVaults.Count; i++)
            {
                ListBoxItem item = new ListBoxItem();

                Console.Out.Write(oVaults[i].Name);
                item.ToolTip = "Connect to " + oVaults[i].Name;
                item.Name = "connectItem" + i;
                item.Visibility = Visibility.Visible;
                item.Content = oVaults[i].Name;
                item.Tag = oVaults[i].GUID;
                //item.AddHandler(
                item.Selected += new RoutedEventHandler(OnSelect);
                listBox2.Items.Add(item);

            }

        }
    }
}
