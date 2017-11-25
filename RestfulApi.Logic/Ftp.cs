using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections;

namespace FileTransfer
{
    /// <summary>  
    /// Supports client-side FTP. Most common
    /// FTP operations are present in this class.
    /// </summary>
    /// <author>       
    /// Bruce Blackshaw
    /// </author>
    /// <version>      
    /// $Revision: 1.1 $
    /// </version>
    public class Ftp
    {
        private const int Chunksize = 4096;

        /// <summary>   
        /// Set the TCP timeout on the underlying socket.
        /// 
        /// If a timeout is set, then any operation which
        /// takes longer than the timeout value will be
        /// killed with a java.io.InterruptedException. We
        /// set both the control and data connections
        /// 
        /// </summary>
        public int Timeout
        {
            set
            {
                timeout = value;
                control.Timeout = value;
            }
        }

        public event UpdateEventHandler BinaryUpdate;
        protected virtual void OnBinaryUpdate(UpdateEventArgs e)
        {
            BinaryUpdate?.Invoke(this, e);
        }

        /// <summary>  
        /// Set the connect mode to ACTIVE or PASV
        /// </summary>
        public ConnectMode ConnectMode
        {
            set
            {
                connectMode = value;
            }
        }

        /// <summary>  
        /// Gets the latest valid reply from the server
        /// </summary>
        /// <returns>  
        /// reply object encapsulating last valid server response
        /// </returns>
        public Reply LastValidReply
        {
            get
            {
                return lastValidReply;
            }
        }

        /// <summary>  
        /// Set the logging stream, replacing stdout
        /// </summary>
        public StreamWriter LogStream
        {
            set
            {
                control.LogStream = value;
            }
        }

        /// <summary>  
        /// Get the current transfer type
        /// </summary>
        /// <returns>  
        /// the current type of the transfer, i.e. BINARY or ASCII
        /// </returns>
        /// <summary>  
        /// Set the transfer type
        /// </summary>
        public TransferType TransferType
        {
            get
            {
                return transferType;
            }

            set
            {
                // determine the character to send
                string typeStr = ASCII_CHAR;
                if (value.Equals(TransferType.Binary))
                {
                    typeStr = BINARY_CHAR;
                }

                // send the command
                var reply = control.SendCommand("TYPE " + typeStr);
                lastValidReply = control.ValidateReply(reply, "200");

                // record the type
                transferType = value;
            }
        }

        /// <summary>  
        /// Revision control id
        /// </summary>
        //static string cvsId = "@(#)$Id: csFtp.cs,v 1.1 2003/05/17 12:33:13 bruceb Exp $";

        /// <summary>  
        /// Format to interpret MTDM timestamp
        /// </summary>
        static string dtFormat = "yyyyMMddHHmmss";

        /// <summary>  
        /// The char sent to the server to set BINARY
        /// </summary>
        static string BINARY_CHAR = "I";

        /// <summary>  
        /// The char sent to the server to set ASCII
        /// </summary>
        static string ASCII_CHAR = "A";

        /// <summary>  
        /// Socket responsible for controlling the connection
        /// </summary>
        ControlSocket control;

        /// <summary>  
        /// Socket responsible for transferring the data
        /// </summary>
        Socket data;

        /// <summary>  
        /// Socket timeout for both data and control. In milliseconds
        /// </summary>
        int timeout;

        /// <summary>  
        /// Record of the transfer type - make the default ASCII
        /// </summary>
        TransferType transferType = TransferType.Ascii;

        /// <summary>  Record of the connect mode - make the default PASV ( as this was
        /// the original mode supported )
        /// </summary>
        ConnectMode connectMode = ConnectMode.Passive;

        /// <summary>  
        /// Holds the last valid reply from the server on the control socket
        /// </summary>
        Reply lastValidReply;

        /// <summary>  
        /// Constructor. Creates the control socket
        /// </summary>
        /// <param name="remoteHost"> 
        /// the remote hostname 
        /// </param>
        public Ftp(string remoteHost)
        {
            control = new ControlSocket(remoteHost, ControlSocket.ControlPort, null, 0);
        }

        /// <summary>  
        /// Constructor. Creates the control socket
        /// </summary>
        /// <param name="remoteAddr"> the address of the
        /// remote host
        /// 
        /// </param>
        public Ftp(IPAddress remoteAddress)
        {
            control = new ControlSocket(remoteAddress, ControlSocket.ControlPort, null, 0);
        }

        /// <summary>  
        /// Constructor. Creates the control
        /// socket. Allows setting of control port ( normally
        /// set by default to 21 ).
        /// </summary>
        /// <param name="remoteAddr"> the address of the
        /// remote host
        /// </param>
        /// <param name="controlPort"> port for control stream
        /// 
        /// </param>
        public Ftp(IPAddress remoteAddress, int controlPort)
        {
            control = new ControlSocket(remoteAddress, controlPort, null, 0);
        }

        /// <summary>  
        /// Constructor. Creates the control socket
        /// </summary>
        /// <param name="remoteHost"> 
        /// the remote hostname
        /// </param>
        /// <param name="log">      
        /// log stream for logging to
        /// </param>		
        /// <param name="timeout">      
        /// the length of the timeout, in milliseconds
        /// </param>
        public Ftp(string remoteHost, StreamWriter log, int timeout)
        {
            control = new ControlSocket(remoteHost, ControlSocket.ControlPort, log, timeout);
        }

        /// <summary>  
        /// Constructor. Creates the control socket
        /// </summary>
        /// <param name="remoteHost"> 
        /// the remote hostname
        /// </param>
        /// <param name="controlPort"> 
        /// port for control stream
        /// </param>
        /// <param name="log">      
        /// log stream for logging to
        /// </param>		
        /// <param name="timeout">      
        /// the length of the timeout, in milliseconds
        /// </param>
        public Ftp(string remoteHost, int controlPort, StreamWriter log, int timeout)
        {
            control = new ControlSocket(remoteHost, controlPort, log, timeout);
        }

        /// <summary>  
        /// Constructor. Creates the control socket
        /// </summary>
        /// <param name="remoteAddr"> 
        /// the address of the remote host
        /// </param>
        /// <param name="log">      
        /// log stream for logging to
        /// </param>				
        /// <param name="timeout">      
        /// the length of the timeout, in seconds
        /// </param>
        public Ftp(IPAddress remoteAddress, StreamWriter log, int timeout)
        {
            control = new ControlSocket(
                remoteAddress,
                ControlSocket.ControlPort,
                log, 
                timeout);
        }

        /// <summary>  
        /// Constructor. Creates the control
        /// socket. Allows setting of control port ( normally
        /// set by default to 21 ).
        /// </summary>
        /// <param name="remoteAddr"> 
        /// the address of the remote host
        /// </param>
        /// <param name="controlPort">
        /// port for control stream
        /// </param>
        /// <param name="log">      
        /// log stream for logging to
        /// </param>		
        /// <param name="timeout">      
        /// the length of the timeout, in seconds		
        /// </param>
        public Ftp(IPAddress remoteAddress, int controlPort, StreamWriter log, int timeout)
        {
            control = new ControlSocket(remoteAddress, controlPort, log, timeout);
        }

        /// <summary>  
        /// Login into an account on the FTP server. This
        /// call completes the entire login process
        /// </summary>
        /// <param name="user">      
        /// user name
        /// </param>
        /// <param name="password">  
        /// user's password
        /// </param>
        public void Login(string user, string password)
        {
            var response = control.SendCommand("USER " + user);
            lastValidReply = control.ValidateReply(response, "331");
            response = control.SendCommand("PASS " + password);
            lastValidReply = control.ValidateReply(response, "230");
        }

        /// <summary>  
        /// Supply the user name to log into an account
        /// on the FTP server. Must be followed by the
        /// password() method - but we allow for
        /// </summary>
        /// <param name="user">      
        /// user name
        /// </param>
        public void User(string user)
        {
            var reply = control.SendCommand("USER " + user);

            // we allow for a site with no password - 230 response
            var validCodes = new string[] { "230", "331" };
            lastValidReply = control.ValidateReply(reply, validCodes);
        }

        /// <summary>  
        /// Supplies the password for a previously supplied
        /// username to log into the FTP server. Must be
        /// preceeded by the user() method
        /// </summary>
        /// <param name="password">  
        /// user's password
        /// </param>
        public void Password(string password)
        {
            var reply = control.SendCommand("PASS " + password);

            // we allow for a site with no passwords ( 202 )
            var validCodes = new string[] { "230", "202" };
            lastValidReply = control.ValidateReply(reply, validCodes);
        }

        /// <summary>  
        /// Issue arbitrary ftp commands to the FTP server.
        /// </summary>
        /// <param name="command">    
        /// ftp command to be sent to server
        /// </param>
        /// <param name="validCodes"> 
        /// valid return codes for this command		
        /// </param>
        public void Quote(string command, string[] validCodes)
        {
            var reply = control.SendCommand(command);

            // allow for no validation to be supplied
            if (validCodes != null && validCodes.Length > 0)
            {
                lastValidReply = control.ValidateReply(reply, validCodes);
            }
        }

        /// <summary>  
        /// Put a local file onto the FTP server. It
        /// is placed in the current directory.
        /// </summary>
        /// <param name="localPath">  
        /// path of the local file
        /// </param>
        /// <param name="remoteFile"> 
        /// name of remote file in current directory		
        /// </param>
        public void Put(string localPath, string remoteFile)
        {
            Put(localPath, remoteFile, false);
        }

        /// <summary>  
        /// Put a stream of data onto the FTP server. It
        /// is placed in the current directory.
        /// </summary>
        /// <param name="sourceStream">  
        /// input stream of data to put
        /// </param>
        /// <param name="remoteFile"> 
        /// name of remote file in current directory		
        /// </param>
        public void Put(Stream sourceStream, string remoteFile)
        {
            Put(sourceStream, remoteFile, false);
        }

        /// <summary>  
        /// Put a local file onto the FTP server. It
        /// is placed in the current directory. Allows appending
        /// if current file exists
        /// </summary>
        /// <param name="localPath">  
        /// path of the local file
        /// </param>
        /// <param name="remoteFile"> 
        /// name of remote file in current directory
        /// </param>
        /// <param name="append">     
        /// true if appending, false otherwise		
        /// </param>
        public void Put(string localPath, string remoteFile, bool append)
        {
            // get according to set type
            if (TransferType == TransferType.Ascii)
            {
                PutASCII(localPath, remoteFile, append);
            }
            else
            {
                PutBinary(localPath, remoteFile, append);
            }

            ValidateTransfer();
        }

        /// <summary>  
        /// Put a stream of data onto the FTP server. It
        /// is placed in the current directory. Allows appending
        /// if current file exists
        /// </summary>
        /// <param name="sourceStream">  
        /// input stream of data to put
        /// </param>
        /// <param name="remoteFile"> 
        /// name of remote file in current directory
        /// </param>
        /// <param name="append">     
        /// true if appending, false otherwise 
        /// </param>
        public void Put(Stream sourceStream, string remoteFile, bool append)
        {
            // get according to set type
            if (TransferType == TransferType.Ascii)
            {
                PutAscii(sourceStream, remoteFile, append);
            }
            else
            {
                PutBinary(sourceStream, remoteFile, append);
            }

            ValidateTransfer();
        }

        /// <summary>  
        /// Validate that the put() or get() was successful
        /// </summary>
        void ValidateTransfer()
        {
            // check the control response
            var validCodes = new string[] { "226", "250" };
            var reply = control.ReadReply();
            lastValidReply = control.ValidateReply(reply, validCodes);
        }

        /// <summary>  
        /// Get the network stream associated with the data socket
        /// </summary>
        /// <returns>  
        /// Network stream ready for reading/writing
        /// </returns>
        NetworkStream GetDataStream()
        {
            var sock = data;

            // in active mode, we must accept the FTP server's connection
            if (connectMode == ConnectMode.Active)
            {
                sock = data.Accept();
            }

            // ensure network stream owns the socket
            return new NetworkStream(sock, true);
        }

        /// <summary>  
        /// Request the server to set up the put
        /// </summary>
        /// <param name="remoteFile"> name of remote file in
        /// current directory
        /// </param>
        /// <param name="append">     true if appending, false otherwise
        /// 
        /// </param>
        void InitPut(string remoteFile, bool append)
        {
            // set up data channel
            data = control.CreateDataSocket(connectMode);

            // send the command to store
            var cmd = append ? "APPE " : "STOR ";
            var reply = control.SendCommand(cmd + remoteFile);

            // Can get a 125 or a 150
            var validCodes = new string[] { "125", "150" };
            lastValidReply = control.ValidateReply(reply, validCodes);
        }

        /// <summary>  
        /// Put as ASCII, i.e. read a line at a time and write
        /// inserting the correct FTP separator
        /// </summary>
        /// <param name="localPath">  
        /// full path of local file to read from
        /// </param>
        /// <param name="remoteFile"> 
        /// name of remote file we are writing to
        /// </param>
        /// <param name="append">     
        /// true if appending, false otherwise
        /// </param>
        void PutASCII(string localPath, string remoteFile, bool append)
        {
            // create an inputstream & pass to common method
            var sourceStream = new FileStream(localPath, FileMode.Open, FileAccess.Read);
            PutAscii(sourceStream, remoteFile, append);
        }

        /// <summary>  
        /// Put as ASCII, i.e. read a line at a time and write
        /// inserting the correct FTP separator
        /// </summary>
        /// <param name="sourceStream">  
        /// input stream of data to put
        /// </param>
        /// <param name="remoteFile"> 
        /// name of remote file we are writing to
        /// </param>
        /// <param name="append">     
        /// true if appending, false otherwise
        /// </param>
        void PutAscii(Stream sourceStream, string remoteFile, bool append)
        {
            // need to read line by line ...
            var reader = new StreamReader(sourceStream);
            InitPut(remoteFile, append);

            // get an character output stream to write to ... AFTER we
            // have the ok to go ahead AND AFTER we've successfully opened a
            // stream for the local file
            var writer = new StreamWriter(GetDataStream());

            // write line by line, writing \r\n as required by RFC959 after
            // each line
            string line = null;
            while ((line = reader.ReadLine()) != null)
            {
                writer.Write(line, 0, line.Length);
                writer.Write(ControlSocket.EndOfLine, 0, ControlSocket.EndOfLine.Length);
            }

            reader.Close();

            // closing the writer will close the data socket
            writer.Flush();
            writer.Close();
        }

        /// <summary>  
        /// Put as binary, i.e. read and write raw bytes
        /// </summary>
        /// <param name="localPath">  
        /// full path of local file to read from
        /// </param>
        /// <param name="remoteFile"> 
        /// name of remote file we are writing to
        /// </param>
        /// <param name="append">     
        /// true if appending, false otherwise
        /// </param>
        void PutBinary(string localPath, string remoteFile, bool append)
        {
            // open input stream to read source file ... do this
            // BEFORE opening output stream to server, so if file not
            // found, an exception is thrown
            var sourceStream = new FileStream(localPath, FileMode.Open, FileAccess.Read);
            PutBinary(sourceStream, remoteFile, append);
        }

        /// <summary>  
        /// Put as binary, i.e. read and write raw bytes
        /// </summary>
        /// <param name="sourceStream">  
        /// input stream of data to put
        /// </param>
        /// <param name="remoteFile"> 
        /// name of remote file we are writing to
        /// </param>
        /// <param name="append">     
        /// true if appending, false otherwise
        /// </param>
        void PutBinary(Stream sourceStream, string remoteFile, bool append)
        {
            var reader = new BufferedStream(sourceStream);
            InitPut(remoteFile, append);

            // get an output stream
            var writer = new BinaryWriter(GetDataStream());
            var buf = new byte[512];

            // read a chunk at a time and write to the data socket
            var count = 0;
            while ((count = reader.Read(buf, 0, buf.Length)) > 0)
            {
                writer.Write(buf, 0, count);
                try
                {
                    OnBinaryUpdate(new UpdateEventArgs(reader.Position, reader.Length));
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }

            reader.Close();
            writer.Flush();
            writer.Close();
        }

        /// <summary>  
        /// Put data onto the FTP server. It
        /// is placed in the current directory.
        /// </summary>
        /// <param name="bytes">       
        /// array of bytes
        /// </param>
        /// <param name="remoteFile"> 
        /// name of remote file in current directory
        /// </param>
        public void Put(byte[] bytes, string remoteFile)
        {
            Put(bytes, remoteFile, false);
        }

        /// <summary>  
        /// Put data onto the FTP server. It
        /// is placed in the current directory. Allows
        /// appending if current file exists
        /// </summary>
        /// <param name="bytes">       
        /// array of bytes
        /// </param>
        /// <param name="remoteFile"> name of remote file in
        /// current directory
        /// </param>
        /// <param name="append">     true if appending, false otherwise
        /// 
        /// </param>
        public void Put(byte[] bytes, string remoteFile, bool append)
        {
            InitPut(remoteFile, append);
            var writer = new BinaryWriter(GetDataStream());
            writer.Write(bytes, 0, bytes.Length);
            writer.Flush();
            writer.Close();
            ValidateTransfer();
        }

        /// <summary>  
        /// Get data from the FTP server. Uses the currently
        /// set transfer mode.
        /// </summary>
        /// <param name="localPath">  
        /// local file to put data in
        /// </param>
        /// <param name="remoteFile"> 
        /// name of remote file in current directory		
        /// </param>
        public void Get(string localPath, string remoteFile)
        {
            // get according to set type
            if (TransferType == TransferType.Ascii)
            {
                GetAscii(localPath, remoteFile);
            }
            else
            {
                GetBinary(localPath, remoteFile);
            }

            ValidateTransfer();
        }

        /// <summary>  
        /// Get data from the FTP server. Uses the currently
        /// set transfer mode.
        /// </summary>
        /// <param name="destinationStream"> 
        /// data stream to write data to
        /// </param>
        /// <param name="remoteFile"> 
        /// name of remote file in current directory
        /// </param>
        public void Get(Stream destinationStream, string remoteFile)
        {
            // get according to set type
            if (TransferType == TransferType.Ascii)
            {
                GetAscii(destinationStream, remoteFile);
            }
            else
            {
                GetBinary(destinationStream, remoteFile);
            }

            ValidateTransfer();
        }

        /// <summary>  
        /// Request to the server that the get is set up
        /// </summary>
        /// <param name="remoteFile"> 
        /// name of remote file
        /// </param>
        void InitGet(string remoteFile)
        {
            // set up data channel
            data = control.CreateDataSocket(connectMode);

            // send the retrieve command
            var reply = control.SendCommand("RETR " + remoteFile);

            // Can get a 125 or a 150
            var validCodes1 = new string[] { "125", "150" };
            lastValidReply = control.ValidateReply(reply, validCodes1);
        }

        /// <summary>  
        /// Get as ASCII, i.e. read a line at a time and write
        /// using the correct newline separator for the OS
        /// </summary>
        /// <param name="localPath">  
        /// full path of local file to write to
        /// </param>
        /// <param name="remoteFile"> 
        /// name of remote file		
        /// </param>
        void GetAscii(string localPath, string remoteFile)
        {
            // B.McKeown:
            // Call initGet() before creating the FileOutputStream.
            // This will prevent being left with an empty file if a FTPException
            // is thrown by initGet().
            InitGet(remoteFile);

            // B. McKeown: Need to store the local file name so the file can be
            // deleted if necessary.
            var localFile = new FileInfo(localPath);

            // create the buffered stream for writing
            //UPGRADE_ISSUE: Constructor 'java.io.BufferedWriter.BufferedWriter' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaioBufferedWriterBufferedWriter_javaioWriter"'
            var writer = new StreamWriter(localPath);

            // get an character input stream to read data from ... AFTER we
            // have the ok to go ahead AND AFTER we've successfully opened a
            // stream for the local file
            var reader = new StreamReader(GetDataStream());

            // read/write a line at a time
            IOException storedException = null;
            string line = null;
            try
            {
                while ((line = reader.ReadLine()) != null)
                {
                    writer.Write(line, 0, line.Length);
                    writer.WriteLine();
                }
            }
            catch (IOException ex)
            {
                storedException = ex;
            }
            finally
            {
                writer.Close();

                // if an error occurred, deleted the local file
                if (storedException != null && File.Exists(localFile.FullName))
                {
                    File.Delete(localFile.FullName);
                }
            }

            try
            {
                reader.Close();
            }
            catch (IOException exception)
            {
                Console.WriteLine(exception.Message);
            }

            // if we failed to write the file, rethrow the exception
            if (storedException != null)
            {
                throw storedException;
            }
        }

        /// <summary>  
        /// Get as ASCII, i.e. read a line at a time and write
        /// using the correct newline separator for the OS
        /// </summary>
        /// <param name="destinationStream"> 
        /// data stream to write data to
        /// </param>
        /// <param name="remoteFile"> 
        /// name of remote file
        /// </param>
        void GetAscii(Stream destinationStream, string remoteFile)
        {
            InitGet(remoteFile);

            // create the buffered stream for writing
            var writer = new StreamWriter(destinationStream);

            // get an character input stream to read data from ... AFTER we
            // have the ok to go ahead
            var reader = new StreamReader(GetDataStream());

            // read/write a line at a time
            IOException storedException = null;
            string line = null;
            try
            {
                while ((line = reader.ReadLine()) != null)
                {
                    writer.Write(line, 0, line.Length);
                    writer.WriteLine();
                }
            }
            catch (IOException ex)
            {
                storedException = ex;
            }
            finally
            {
                writer.Close();
            }

            try
            {
                reader.Close();
            }
            catch (IOException exception)
            {
                Console.WriteLine(exception.Message);
            }

            // if we failed to write the file, rethrow the exception
            if (storedException != null)
            {
                throw storedException;
            }
        }

        /// <summary>  
        /// Get as binary file, i.e. straight transfer of data
        /// </summary>
        /// <param name="localPath">  
        /// full path of local file to write to
        /// </param>
        /// <param name="remoteFile"> 
        /// name of remote file
        /// </param>
        void GetBinary(string localPath, string remoteFile)
        {
            // B.McKeown:
            // Call initGet() before creating the FileOutputStream.
            // This will prevent being left with an empty file if a FTPException
            // is thrown by initGet().
            InitGet(remoteFile);

            // B. McKeown: Need to store the local file name so the file can be
            // deleted if necessary.
            var localFile = new FileInfo(localPath);

            // create the buffered output stream for writing the file
            var writer = new BinaryWriter(new FileStream(localPath, FileMode.OpenOrCreate));

            // get an input stream to read data from ... AFTER we have
            // the ok to go ahead AND AFTER we've successfully opened a
            // stream for the local file
            var reader = new BinaryReader(GetDataStream());

            // do the retrieving
            var chunk = new byte[Chunksize];
            int count;
            IOException storedException = null;

            // read from socket & write to file in chunks
            try
            {
                while ((count = reader.Read(chunk, 0, chunk.Length)) > 0)
                {
                    writer.Write(chunk, 0, count);
                }
            }
            catch (IOException ex)
            {
                storedException = ex;
            }
            finally
            {
                writer.Close();

                // if an error occurred, deleted the local file
                if (storedException != null && File.Exists(localFile.FullName))
                    File.Delete(localFile.FullName);
            }

            // close streams
            try
            {
                reader.Close();
            }
            catch (IOException exception)
            {
                Console.WriteLine(exception.Message);
            }

            // if we failed to write the file, rethrow the exception
            if (storedException != null)
            {
                throw storedException;
            }
        }

        /// <summary>  
        /// Get as binary file, i.e. straight transfer of data
        /// </summary>
        /// <param name="destinationStream"> 
        /// stream to write to
        /// </param>
        /// <param name="remoteFile"> 
        /// name of remote file
        /// </param>
        void GetBinary(Stream destinationStream, string remoteFile)
        {
            InitGet(remoteFile);

            // create the buffered output stream for writing the file
            var writer = new BinaryWriter(destinationStream);

            // get an input stream to read data from ... AFTER we have
            // the ok to go ahead AND AFTER we've successfully opened a
            // stream for the local file
            var reader = new BinaryReader(GetDataStream());

            // do the retrieving
            var chunk = new byte[Chunksize];
            int count;
            IOException storedException = null;

            // read from socket & write to file in chunks
            try
            {
                while ((count = reader.Read(chunk, 0, chunk.Length)) > 0)
                {
                    writer.Write(chunk, 0, count);
                }
            }
            catch (IOException exception)
            {
                storedException = exception;
            }
            finally
            {
                writer.Close();
            }

            // close streams
            try
            {
                reader.Close();
            }
            catch (IOException exception)
            {
                Console.WriteLine(exception.Message);
            }

            // if we failed to write to the stream, rethrow the exception
            if (storedException != null)
            {
                throw storedException;
            }
        }

        /// <summary>  
        /// Get data from the FTP server. Transfers in
        /// whatever mode we are in. Retrieve as a byte array. Note
        /// that we may experience memory limitations as the
        /// entire file must be held in memory at one time.
        /// </summary>
        /// <param name="remoteFile"> 
        /// name of remote file in current directory
        /// </param>
        public byte[] Get(string remoteFile)
        {
            InitGet(remoteFile);

            // get an input stream to read data from
            var reader = new BinaryReader(GetDataStream());

            // do the retrieving
            var chunk = new byte[Chunksize]; // read chunks into
            var temp = new MemoryStream(Chunksize); // temp swap buffer
            int count; // size of chunk read

            // read from socket & write to file
            while ((count = reader.Read(chunk, 0, chunk.Length)) > 0)
            {
                temp.Write(chunk, 0, count);
            }

            temp.Close();

            // close streams
            try
            {
                reader.Close();
            }
            catch (IOException exception)
            {
                Console.WriteLine(exception.Message);
            }

            ValidateTransfer();

            return temp.ToArray();
        }

        /// <summary>  
        /// Run a site-specific command on the
        /// server. Support for commands is dependent
        /// on the server
        /// </summary>
        /// <param name="command">  
        /// the site command to run
        /// </param>
        /// <returns> true if command ok, false if
        /// command not implemented
        /// </returns>
        public bool Site(string command)
        {
            // send the retrieve command
            var reply = control.SendCommand("SITE " + command);

            // Can get a 200 ( ok ) or 202 ( not impl ). Some
            // FTP servers return 502 ( not impl )
            var validCodes = new string[] { "200", "202", "502" };
            lastValidReply = control.ValidateReply(reply, validCodes);

            // return true or false? 200 is ok, 202/502 not
            // implemented
            return reply.Substring(0, (3) - (0)).Equals("200");
        }

        /// <summary>  
        /// List current directory's contents as an array of strings of
        /// filenames.
        /// </summary>
        /// <returns>  
        /// an array of current directory listing strings
        /// </returns>
        public string[] Dir()
        {
            return Dir(null, false);
        }

        /// <summary>  
        /// List a directory's contents as an array of strings of filenames.
        /// </summary>
        /// <param name="dirname"> name of directory( <b>not</b> a file mask )
        /// </param>
        /// <returns>  an array of directory listing strings
        /// 
        /// </returns>
        public string[] Dir(string directoryName)
        {
            return Dir(directoryName, false);
        }

        /// <summary>  
        /// List a directory's contents as an array of strings. A detailed
        /// listing is available, otherwise just filenames are provided.
        /// The detailed listing varies in details depending on OS and
        /// FTP server. Note that a full listing can be used on a file
        /// name to obtain information about a file
        /// </summary>
        /// <param name="dirname"> 
        /// name of directory ( <b>not</b> a file mask )
        /// </param>
        /// <param name="full">    
        /// true if detailed listing required false otherwise
        /// </param>
        /// <returns>  
        /// an array of directory listing strings
        /// </returns>
        public string[] Dir(string directoryName, bool full)
        {
            // set up data channel
            data = control.CreateDataSocket(connectMode);

            // send the retrieve command
            var command = full ? "LIST " : "NLST ";
            if (directoryName != null)
            {
                command += directoryName;
            }

            // some FTP servers bomb out if NLST has whitespace appended
            command = command.Trim();
            var reply = control.SendCommand(command);

            // check the control response. wu-ftp returns 550 if the
            // directory is empty, so we handle 550 appropriately 
            var validCodes1 = new string[] { "125", "150", "550" };
            lastValidReply = control.ValidateReply(reply, validCodes1);

            // an empty array of files for 550
            var result = new string[0];

            // a normal reply ... extract the file list
            if (!lastValidReply.Code.Equals("550"))
            {
                // get an character input stream to read data from .
                var reader = new StreamReader(GetDataStream());

                // read a line at a time
                var lines = new ArrayList();
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
                try
                {
                    reader.Close();
                }
                catch (IOException exception)
                {
                    Console.WriteLine(exception.Message);
                }

                // check the control response
                var validCodes2 = new string[] { "226", "250" };
                reply = control.ReadReply();
                lastValidReply = control.ValidateReply(reply, validCodes2);

                // empty array is default
                if (lines.Count > 0)
                {
                    result = (string[])lines.ToArray(typeof(string));
                }
            }

            return result;
        }

        /// <summary>  
        /// Switch debug of responses on or off
        /// </summary>
        /// <param name="on"> true if you wish to have responses to
        /// the log stream, false otherwise
        /// 
        /// </param>
        public void DebugResponses(bool on)
        {
            control.DebugResponses(on);
        }

        /// <summary>  
        /// Delete the specified remote file
        /// </summary>
        /// <param name="remoteFile"> name of remote file to
        /// delete
        /// 
        /// </param>
        public void Delete(string remoteFile)
        {
            var reply = control.SendCommand("DELE " + remoteFile);
            lastValidReply = control.ValidateReply(reply, "250");
        }

        /// <summary>  
        /// Rename a file or directory
        /// </summary>
        /// <param name="from"> name of file or directory to rename
        /// </param>
        /// <param name="to">   intended name
        /// 
        /// </param>
        public void Rename(string from, string to)
        {
            var reply = control.SendCommand("RNFR " + from);
            lastValidReply = control.ValidateReply(reply, "350");

            reply = control.SendCommand("RNTO " + to);
            lastValidReply = control.ValidateReply(reply, "250");
        }

        /// <summary>  
        /// Delete the specified remote working directory
        /// </summary>
        /// <param name="dir"> 
        /// name of remote directory to delete
        /// </param>
        public void Rmdir(string directory)
        {
            var reply = control.SendCommand("RMD " + directory);

            // some servers return 257, technically incorrect but
            // we cater for it ...
            var validCodes = new string[] { "250", "257" };
            lastValidReply = control.ValidateReply(reply, validCodes);
        }

        /// <summary>  
        /// Create the specified remote working directory
        /// </summary>
        /// <param name="dir"> 
        /// name of remote directory to create		
        /// </param>
        public void Mkdir(string directory)
        {
            var reply = control.SendCommand("MKD " + directory);
            lastValidReply = control.ValidateReply(reply, "257");
        }

        /// <summary>  
        /// Change the remote working directory to
        /// that supplied
        /// </summary>
        /// <param name="dir"> name of remote directory to
        /// change to
        /// 
        /// </param>
        public void Chdir(string directory)
        {
            var reply = control.SendCommand("CWD " + directory);
            lastValidReply = control.ValidateReply(reply, "250");
        }

        /// <summary>  
        /// Get modification time for a remote file
        /// </summary>
        /// <param name="remoteFile">  
        /// name of remote file
        /// </param>
        /// <returns>   
        /// modification time of file as a date
        /// </returns>
        public DateTime ModTime(string remoteFile)
        {
            var reply = control.SendCommand("MDTM " + remoteFile);
            lastValidReply = control.ValidateReply(reply, "213");

            // parse the reply string ...
            return DateTime.ParseExact(lastValidReply.Text, dtFormat, null);
        }

        /// <summary>  
        /// Get the current remote working directory
        /// </summary>
        /// <returns>   
        /// the current working directory
        /// </returns>
        public string Pwd()
        {
            var result = string.Empty;
            var reply = control.SendCommand("PWD");
            lastValidReply = control.ValidateReply(reply, "257");

            // get the reply text and extract the dir
            // listed in quotes, if we can find it. Otherwise
            // just return the whole reply string
            var text = lastValidReply.Text;
            var start = text.IndexOf('"');
            var end = text.LastIndexOf('"');
            if (start >= 0 && end > start)
            {
                result = text.Substring(start + 1, (end) - (start + 1));
            }
            else
            {
                result = text;
            }

            return result;
        }

        /// <summary>  
        /// Get the type of the OS at the server
        /// </summary>
        /// <returns>   
        /// the type of server OS		
        /// </returns>
        public string System()
        {
            var reply = control.SendCommand("SYST");
            lastValidReply = control.ValidateReply(reply, "215");
            return lastValidReply.Text;
        }

        /// <summary>  
        /// Get the help text for the specified command
        /// </summary>
        /// <param name="command"> name of the command to get help on
        /// </param>
        /// <returns> help text from the server for the supplied command
        /// 
        /// </returns>
        public string Help(string command)
        {
            var reply = control.SendCommand("HELP " + command);
            var validCodes = new string[] { "211", "214" };
            lastValidReply = control.ValidateReply(reply, validCodes);
            return lastValidReply.Text;
        }

        /// <summary>  
        /// Quit the FTP session
        /// </summary>
        public void Quit()
        {
            try
            {
                var reply = control.SendCommand("QUIT");
                var validCodes = new string[] { "221", "226" };
                lastValidReply = control.ValidateReply(reply, validCodes);
            }
            finally
            {
                // ensure we clean up the connection
                control.Logout();
                control = null;
            }
        }
    }
}
