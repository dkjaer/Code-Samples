namespace FileTransfer
{
    public class UpdateEventArgs
    {
        public long Current { get; set; }

        public long Total { get; set; }

        public UpdateEventArgs()
        {
        }

        public UpdateEventArgs(long current, long total)
        {
            Current = current;
            Total = total;
        }
    }
}
