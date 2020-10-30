namespace AcousticConnections.Enums
{
    public enum ApiCommand
    {
        Login = 1,
        Logout = 2,
        CreateContactList = 3,
        ImportTable = 4,
        ImportList = 5,
        CreateTable = 6,
        GetJobStatus = 7,
        ExportList = 8,
        GetSendMailingsForOrg = 9,
        GetAggregateTrackingForMailing = 10,
        RemoveRecipient = 11,
        DeleteRelationalTableData = 12,
        GetLists = 13,
        AddRecipient = 14,
        OptOutRecipient = 15,
        SelectRecipientData = 16,
        UpdateRecipient = 17,
        GdprErasure = 18
    }
}