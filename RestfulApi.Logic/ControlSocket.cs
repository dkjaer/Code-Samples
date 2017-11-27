using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FileTransfer
{
    public class ControlSocket
    {
        /// <summary>   
        /// Set the TCP timeout on the underlying control socket.
        /// </summary>
        /// <param>
        /// The length of the timeout, in milliseconds
        /// </param>
        internal int Timeout
        {
            set
            {
                timeout = value;
                if (controlSock == null)
                {
                    throw new SystemException("Failed to set timeout - no control socket");
                }

                SetSocketTimeout(controlSock, timeout);
            }
        }

        /// <summary>  
        /// Set the logging stream, replacing stdout		
        /// </summary>
        internal StreamWriter LogStream
        {
            set
            {
                if (value != null)
                {
                    log = value;
                }
            }
        }

        /// <summary>  
        /// Revision control id
        /// </summary>
        //private static string cvsId = "@( # )$Id: FTPControlSocket.cs,v 1.1 2003/05/17 12:33:13 bruceb Exp $";

        /// <summary>   
        /// Standard FTP end of line sequence
        /// </summary>
        internal const string EndOfLine = "\r\n";

        /// <summary>   
        /// The control port number for FTP
        /// </summary>
        internal const int ControlPort = 21;

        /// <summary>   Controls if responses sent back by the
        /// server are sent to assigned output stream
        /// </summary>
        bool debugResponses;

        /// <summary>  
        /// Output stream debug is written to, stdout by default
        /// </summary>
        TextWriter log = Console.Out;

        /// <summary>  
        /// Timeout value
        /// </summary>
        int timeout = -1;

        /// <summary>  
        /// The underlying socket.
        /// </summary>
        Socket controlSock = null;

        /// <summary>  
        /// The control socket's output stream
        /// </summary>
        StreamWriter writer = null;

        /// <summary>  
        /// The control socket's input stream
        /// </summary>
        StreamReader reader = null;

        /// <summary>   
        /// Constructor. Performs TCP connection and
        /// sets up reader/writer. Allows different control
        /// port to be used
        /// </summary>
        /// <param name="remoteHost">  Remote hostname
        /// </param>
        /// <param name="controlPort"> port for control stream
        /// </param>
        /// <param name="timeout">      the length of the timeout, in seconds
        /// </param>
        /// <param name="log">         the new logging stream
        /// 
        /// </param>
        public ControlSocket(
            string remoteHost,
            int controlPort,
            StreamWriter log,
            int timeout)
        {
            // resolve remote host & take first entry
            var remoteHostEntry = System.Net.Dns.GetHostEntry(remoteHost);
            var ipAddresses = remoteHostEntry.AddressList;
            Initialize(ipAddresses[0], controlPort, log, timeout);
        }

        /// <summary>   
        /// Constructor. Performs TCP connection and
        /// sets up reader/writer. Allows different control
        /// port to be used
        /// </summary>
        /// <param name="remoteAddr">  Remote inet address
        /// </param>
        /// <param name="controlPort"> port for control stream
        /// </param>
        /// <param name="log">         the new logging stream
        /// </param>
        /// <param name="timeout">      the length of the timeout, in milliseconds
        /// </param>
        public ControlSocket(
            IPAddress remoteAddr,
            int controlPort,
            StreamWriter log,
            int timeout)
        {
            Initialize(remoteAddr, controlPort, log, timeout);
        }

        /// <summary>   
        /// Common constructor code. Performs TCP connection and
        /// sets up reader/writer. Allows different control
        /// port to be used
        /// </summary>
        /// <param name="remoteAddr">  Remote inet address
        /// </param>
        /// <param name="controlPort"> port for control stream
        /// </param>
        /// <param name="log">         the new logging stream
        /// </param>
        /// <param name="timeout">      the length of the timeout, in milliseconds
        /// </param>
        internal void Initialize(
            IPAddress remoteAddr,
            int controlPort,
            StreamWriter log,
            int timeout)
        {
            LogStream = log;

            // ensure we get debug from initial connection sequence
            DebugResponses(true);

            // establish socket connection & set timeouts
            var ipe = new IPEndPoint(remoteAddr, controlPort);
            controlSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Timeout = timeout;
            controlSock.Connect(ipe);

            InitStreams();
            ValidateConnection();

            // switch off debug - user can switch on from this point
            DebugResponses(false);
        }

        /// <summary>   
        /// Checks that the standard 220 reply is returned
        /// following the initiated connection
        /// </summary>
        void ValidateConnection()
        {
            var reply = ReadReply();
            ValidateReply(reply, "220");
        }

        /// <summary>  
        /// Obtain the reader/writer stream for this connection
        /// </summary>
        void InitStreams()
        {
            var stream = new NetworkStream(controlSock, true);
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
        }

        /// <summary>  
        /// Quit this FTP session and clean up.
        /// </summary>
        public void Logout()
        {
            log.Flush();
            log = null;

            // owns socket so will close it
            writer.Close();
            reader.Close();
        }

        /// <summary>  
        /// Request a data socket be created on the
        /// server, connect to it and return our
        /// connected socket.
        /// </summary>
        /// <param name="connectMode">  
        /// connection mode to connect with, either active or passive
        /// </param>
        /// <returns>  
        /// connected data socket
        /// </returns>
        internal Socket CreateDataSocket(ConnectMode connectMode)
        {
            // active mode ( PORT )
            if (connectMode == ConnectMode.Active)
            {
                return CreateDataSocketActive();
            }
            else // PASV
            {
                return CreateDataSocketPASV();
            }
        }

        /// <summary>  
        /// Create a listening socket which waits for a connection
        /// </summary>
        /// <returns>  
        /// not connected data socket		
        /// </returns>
        Socket CreateDataSocketActive()
        {
            // create listening socket at a system allocated port
            //Socket sock = new Socket( AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // choose any port

            // 127.0.0.1 converted to a long integer is 2130706433
            // I'm just using the localhost address for a default.
            var IP4 = new IPAddress(2130706433);

            // Find the local InterNetwork V4 address
            var IPs = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            foreach (IPAddress ip in IPs)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    IP4 = ip;
                    break;
                }
            }

            var localHostEntry = Dns.GetHostEntry(IP4);

            // Find the remote InterNetwork V4 address
            foreach (var ip in localHostEntry.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    IP4 = ip;
                    break;
                }
            }

            var localEndPoint = new IPEndPoint(IP4, 0);
            sock.Bind(localEndPoint);

            // queue up to 5 connections
            sock.Listen(5);

            // get the listen port
            var port = ((IPEndPoint)sock.LocalEndPoint).Port;
            IPAddress addr = ((IPEndPoint)sock.LocalEndPoint).Address;

            // find out ip & port we are listening on			
            SetDataPort((IPEndPoint)sock.LocalEndPoint);
            return sock;
        }

        /// <summary>  
        /// Sets the data port on the server, i.e. sends a PORT
        /// command		
        /// </summary>
        /// <param name="ep">local endpoint
        /// </param>
        void SetDataPort(IPEndPoint ep)
        {
            //byte[] hostBytes = BitConverter.GetBytes( ep.Address.Address );

            // This is a .NET 1.1 API
            var hostBytes = ep.Address.GetAddressBytes();
            var portBytes = ToByteArray((ushort)ep.Port);

            // assemble the PORT command
            string cmd = new StringBuilder("PORT ").
                Append((short)hostBytes[0]).Append(",").
                Append((short)hostBytes[1]).Append(",").
                Append((short)hostBytes[2]).Append(",").
                Append((short)hostBytes[3]).Append(",").
                Append((short)portBytes[0]).Append(",").
                Append((short)portBytes[1]).ToString();

            // send command and check reply
            var reply = SendCommand(cmd);
            ValidateReply(reply, "200");
        }

        /// <summary>  
        /// Convert a short into a byte array
        /// </summary>
        /// <param name="val">  value to convert
        /// </param>
        /// <returns>  a byte array
        /// 
        /// </returns>
        protected internal byte[] ToByteArray(ushort val)
        {
            var bytes = new byte[2];
            bytes[0] = (byte)(val >> 8); // bits 1- 8
            bytes[1] = (byte)(val & 0x00FF); // bits 9-16
            return bytes;
        }

        /// <summary>  
        /// Request a data socket be created on the
        /// server, connect to it and return our
        /// connected socket.
        /// </summary>
        /// <returns>  connected data socket
        /// 
        /// </returns>
        Socket CreateDataSocketPASV()
        {
            // PASSIVE command - tells the server to listen for
            // a connection attempt rather than initiating it
            var reply = SendCommand("PASV");
            ValidateReply(reply, "227");

            // The reply to PASV is in the form:
            // 227 Entering Passive Mode ( h1,h2,h3,h4,p1,p2 ).
            // where h1..h4 are the IP address to connect and
            // p1,p2 the port number
            // Example:
            // 227 Entering Passive Mode ( 128,3,122,1,15,87 ).
            // NOTE: PASV command in IBM/Mainframe returns the string
            // 227 Entering Passive Mode 128,3,122,1,15,87	( missing 
            // brackets )

            // extract the IP data string from between the brackets
            var startIP = reply.IndexOf('(');
            var endIP = reply.IndexOf(')');

            // allow for IBM missing brackets around IP address
            if (startIP < 0 && endIP < 0)
            {
                startIP = reply.ToUpper().LastIndexOf("MODE", StringComparison.Ordinal) + 4;
                endIP = reply.Length;
            }

            var ipData = reply.Substring(startIP + 1, (endIP) - (startIP + 1));
            var parts = new int[6];
            var len = ipData.Length;
            var partCount = 0;
            var buf = new StringBuilder();

            // loop thru and examine each char
            for (int i = 0; i < len && partCount <= 6; i++)
            {
                var ch = ipData[i];
                if (char.IsDigit(ch))
                {
                    buf.Append(ch);
                }
                else if (ch != ',')
                {
                    throw new FtpException("Malformed PASV reply: " + reply);
                }

                // get the part
                if (ch == ',' || i + 1 == len)
                {
                    // at end or at separator
                    try
                    {
                        parts[partCount++] = int.Parse(buf.ToString());
                        buf.Length = 0;
                    }
                    catch
                    {
                        throw new FtpException("Malformed PASV reply: " + reply);
                    }
                }
            }

            // assemble the IP address
            // we try connecting, so we don't bother checking digits etc
            var ipAddress = string.Format("{0}.{1}.{2}.{3}", parts[0], parts[1], parts[2], parts[3]);

            // assemble the port number
            var port = (parts[4] << 8) + parts[5];            
            var addr = Dns.GetHostAddresses(ipAddress)[0];
            var ipe = new IPEndPoint(addr, port);
            var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SetSocketTimeout(sock, timeout);
            sock.Connect(ipe);
            return sock;
        }

        /// <summary>  
        /// Send a command to the FTP server and
        /// return the server's reply
        /// </summary>
        /// <returns>  reply to the supplied command
        /// 
        /// </returns>
        internal string SendCommand(string command)
        {
            if (debugResponses)
            {
                log.WriteLine("---> " + command);
            }

            // send it
            writer.Write(command + EndOfLine);
            writer.Flush();

            // and read the result
            return ReadReply();
        }

        /// <summary>  
        /// Read the FTP server's reply to a previously
        /// issued command. RFC 959 states that a reply
        /// consists of the 3 digit code followed by text.
        /// The 3 digit code is followed by a hyphen if it
        /// is a muliline response, and the last line starts
        /// with the same 3 digit code.		
        /// </summary>
        /// <returns>  
        /// reply string
        /// </returns>
        internal string ReadReply()
        {
            var firstLine = reader.ReadLine();
            if (firstLine == null || firstLine.Length == 0)
            { 
                throw new IOException("Unexpected null reply received");
            }

            var reply = new StringBuilder(firstLine);

            if (debugResponses)
            {
                log.WriteLine(reply.ToString());
            }

            var replyCode = reply.ToString().Substring(0, 3);

            // Check for multiline response and build up the reply.
            if (reply[3] == '-')
            {
                var complete = false;
                while (!complete)
                {
                    var line = reader.ReadLine();
                    if (line == null)
                    {
                        throw new IOException("Unexpected null reply received");
                    }

                    if (debugResponses)
                    { 
                        log.WriteLine(line);
                    }

                    if (line.Length > 3 && line.Substring(0, 3).Equals(replyCode) && line[3] == ' ')
                    {
                        reply.Append(line.Substring(3));
                        complete = true;
                    }
                    else
                    {
                        reply.Append(" ");
                        reply.Append(line);
                    }
                } 
            }

            return reply.ToString();
        }

        /// <summary>  Validate the response the host has supplied against the
        /// expected reply. If we get an unexpected reply we throw an
        /// exception, setting the message to that returned by the
        /// FTP server
        /// 
        /// </summary>
        /// <param name="reply">             the entire reply string we received
        /// </param>
        /// <param name="expectedReplyCode"> the reply we expected to receive
        /// 
        /// 
        /// </param>
        internal Reply ValidateReply(string reply, string expectedReplyCode)
        {
            // all reply codes are 3 chars long
            var replyCode = reply.Substring(0, 3);
            var replyText = reply.Substring(4);
            var replyObj = new Reply(replyCode, replyText);

            if (replyCode.Equals(expectedReplyCode))
            {
                return replyObj;
            }

            // if unexpected reply, throw an exception
            throw new FtpException(replyText, replyCode);
        }

        /// <summary>  
        /// Validate the response the host has supplied against the
        /// expected reply. If we get an unexpected reply we throw an
        /// exception, setting the message to that returned by the
        /// FTP server
        /// </summary>
        /// <param name="reply">              
        /// the entire reply string we received
        /// </param>
        /// <param name="expectedReplyCodes"> 
        /// array of expected replies
        /// </param>
        /// <returns>  
        /// an object encapsulating the server's reply
        /// </returns>
        internal Reply ValidateReply(string reply, string[] expectedReplyCodes)
        {
            // all reply codes are 3 chars long
            var replyCode = reply.Substring(0, 3);
            var replyText = reply.Substring(4);
            var replyObj = new Reply(replyCode, replyText);

            for (int i = 0; i < expectedReplyCodes.Length; i++)
            {
                if (replyCode.Equals(expectedReplyCodes[i]))
                {
                    return replyObj;
                }
            }

            // got this far, not recognised
            throw new FtpException(replyText, replyCode);
        }

        /// <summary>  
        /// Switch debug of responses on or off
        /// </summary>
        /// <param name="on"> true if you wish to have responses to
        /// stdout, false otherwise
        /// 
        /// </param>
        internal void DebugResponses(bool on)
        {
            debugResponses = on;
        }

        /// <summary>  
        /// Helper method to set a socket's timeout value
        /// </summary>
        /// <param name="sock">socket to set timeout for
        /// </param>
        /// <param name="timeout">timeout value to set
        /// </param>
        void SetSocketTimeout(Socket sock, int timeout)
        {
            if (timeout > 0)
            {
                sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, timeout);
                sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, timeout);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //// free managed resources
            }

            //// free native resources if there are any.
        }
    }
}
