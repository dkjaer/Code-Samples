using System;

namespace FileTransfer
{
    public class FtpException : Exception
    {
        /// <summary>   
        /// Get the reply code if it exists
        /// </summary>
        /// <returns>  
        /// reply if it exists, -1 otherwise
        /// </returns>
        public int Code { get; }

        /// <summary>  
        /// Revision control id
        /// </summary>
        //private static string cvsId = "@( # )$Id: FTPException.cs,v 1.1 2003/05/17 12:33:13 bruceb Exp $";

        /// <summary>   
        /// Constructor. Delegates to super.
        /// </summary>
        /// <param name="message">  Message that the user will be
        /// able to retrieve
        /// 
        /// </param>
        public FtpException(string message)
            : base(message)
        {
            Code = -1;
        }

        /// <summary>  
        /// Constructor. Permits setting of reply code
        /// </summary>
        /// <param name="message">       
        /// message that the user will be able to retrieve
        /// </param>
        /// <param name="code"> string form of reply code
        /// 
        /// </param>
        public FtpException(string message, string code)
            : base(message)
        {
            // extract reply code if possible
            try
            {
                Code = int.Parse(code);
            }
            catch
            {
                Code = -1;
            }
        }
    }
}
