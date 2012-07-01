Imports Microsoft.VisualBasic


Public Class Utils
    Function MF_FindClassID( _
            ByRef oVault As MFilesAPI.Vault, _
            ByVal szClassName As String) As Integer

        ' Set the search conditions for the value list item.
        Dim oScValueListItem As New MFilesAPI.SearchCondition
        oScValueListItem.Expression.SetValueListItemExpression( _
            MFilesAPI.MFValueListItemPropertyDef.MFValueListItemPropertyDefName, _
            MFilesAPI.MFParentChildBehavior.MFParentChildBehaviorNone)
        oScValueListItem.ConditionType = MFilesAPI.MFConditionType.MFConditionTypeEqual
        oScValueListItem.TypedValue.SetValue(MFilesAPI.MFDataType.MFDatatypeText, szClassName)
        Dim arrSearchConditions As New MFilesAPI.SearchConditions
        arrSearchConditions.Add(-1, oScValueListItem)

        ' Search for the value list item.
        Dim results As MFilesAPI.ValueListItemSearchResults
        results = oVault.ValueListItemOperations.SearchForValueListItemsEx(MFilesAPI.MFBuiltInValueList.MFBuiltInValueListClasses, arrSearchConditions)
        If results.Count > 0 Then
            ' Found.
            MF_FindClassID = results(1).ID
        Else
            ' Not found.
            MF_FindClassID = -1
        End If
    End Function
End Class
