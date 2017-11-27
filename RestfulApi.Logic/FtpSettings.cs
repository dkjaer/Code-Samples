namespace FileTransfer
{
    public class FtpSettings
    {
        public string Url { get; set; }

        public bool UseSsh { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string SubFolder { get; set; }

        public bool CheckForFile { get; set; }

        public bool RenameAfterUpload { get; set; }

        public ConnectMode ConnectMode { get; set; }

        public TransferType TransferType { get; set; }

        public FtpSettings()
        {
            // Assign default settings.
            SubFolder = string.Empty;
            CheckForFile = true;
            RenameAfterUpload = true;
            ConnectMode = ConnectMode.Passive;
            TransferType = TransferType.Binary;
        }
    }
}
