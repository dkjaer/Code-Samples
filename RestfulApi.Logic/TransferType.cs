using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransfer
{
    /// <summary>  
    /// Enumerates the transfer types possible. We support only the two common types, 
    /// ASCII and Image ( often called binary ).
    /// </summary>
    public enum TransferType
    {
        /// <member>   
        /// Represents ASCII transfer type
        /// </member>
        ASCII = 1,

        /// <member>   
        /// Represents Image ( or binary ) transfer type
        /// </member>
        BINARY = 2
    }
}