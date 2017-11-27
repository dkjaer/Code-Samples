namespace FileTransfer
{
    public class FileTransferEventArgs
    {
        public string FilePath { get; set; }

        public string OriginalFileName { get; set; }

        public string NewFileName { get; set; }

        public FtpSettings FtpSettings { get; set; }

        public string Destination { get; set; }

        public string Status { get; set; }

        public long CurrentProgress { get; set; }

        public long TotalProgress { get; set; }

        public FileTransferEventArgs()
        {

        }

        public FileTransferEventArgs(string status)
        {
            Status = status;
        }

        public FileTransferEventArgs(long currentProgress, long totalProgress)
        {
            CurrentProgress = currentProgress;
            TotalProgress = totalProgress;
        }

        public FileTransferEventArgs(string filePath, FtpSettings ftpSettings)
        {
            FilePath = filePath;
            FtpSettings = ftpSettings;
        }

        public FileTransferEventArgs(
            string filePath,
            string originalFileName,
            string newFileName,
            FtpSettings ftpSettings)
        {
            FilePath = filePath;
            OriginalFileName = originalFileName;
            NewFileName = newFileName;
            FtpSettings = ftpSettings;
        }

        public FileTransferEventArgs(
            string filePath,
            string originalFileName,
            string newFileName,
            FtpSettings ftpSettings,
            string destination)
        {
            FilePath = filePath;
            OriginalFileName = originalFileName;
            NewFileName = newFileName;
            FtpSettings = ftpSettings;
            Destination = destination;
        }
    }
}
