namespace FileTransfer
{
    public class Reply
    {
        /// <summary>  Getter for reply code
        ///  
        /// </summary>
        /// <returns> server's reply code
        /// 
        /// </returns>
        public string Code { get; }

        /// <summary>  Getter for reply text
        /// 
        /// </summary>
        /// <returns> server's reply text
        /// 
        /// </returns>
        public string Text { get; }

        /// <summary>  Revision control id
        /// </summary>
        //private static string cvsId = "@( # )$Id: FTPReply.cs,v 1.1 2003/05/17 12:33:13 bruceb Exp $";

        /// <summary>  
        /// Constructor. Only to be constructed
        /// by this package, hence internal access
        /// </summary>
        /// <param name="code"> the server's reply code
        /// </param>
        /// <param name="text"> the server's reply text
        /// 
        /// </param>
        internal Reply(string code, string text)
        {
            Code = code;
            Text = text;
        }
    }
}
