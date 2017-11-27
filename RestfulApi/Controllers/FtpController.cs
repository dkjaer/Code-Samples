using System.Web;
using System.Web.Http;
using FileTransfer;

namespace RestfulApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class FtpController : ApiController
    {
        void Client_ProgressChanged(object sender, FileTransferEventArgs e)
        {

        }

        void Client_StatusChanged(object sender, FileTransferEventArgs e)
        {

        }
        
        /// <summary>
        /// Upload a document to an FTP server.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="useSsh"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="subFolder"></param>
        /// <param name="checkForFile"></param>
        /// <param name="renameAfterUpload"></param>
        /// <param name="connectMode"></param>
        /// <param name="transferType"></param>
        /// <param name="localFilePath"></param>
        /// <returns></returns>
        [Route("Upload")]
        public IHttpActionResult PostFile(
            string url,
            bool useSsh,
            string username,
            string password,
            string subFolder,
            bool checkForFile,
            bool renameAfterUpload,
            ConnectMode connectMode,
            TransferType transferType,
            string localFilePath)
        {
            var settings = new FtpSettings();
            settings.Url = url;
            settings.UseSsh = useSsh;
            settings.Username = username;
            settings.Password = password;
            settings.SubFolder = subFolder;
            settings.CheckForFile = checkForFile;
            settings.RenameAfterUpload = renameAfterUpload;
            settings.ConnectMode = connectMode;
            settings.TransferType = transferType;

            var client = new Client();
            client.StatusChanged += Client_StatusChanged;
            client.ProgressChanged += Client_ProgressChanged;
            
            var result = client.Upload(settings, localFilePath);
            if (client.ErrorMessage != null && client.ErrorMessage != string.Empty)
            {
                return InternalServerError(new System.Exception(client.ErrorMessage));
            }

            return Ok(result);
        }

        /// <summary>
        /// Return a list of files in an FTP directory.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="useSsh"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="connectMode"></param>
        /// <param name="subFolder"></param>
        /// <returns></returns>
        [Route("Directory")]
        public IHttpActionResult GetFileList(
            string url,
            bool useSsh,
            string username,
            string password,
            ConnectMode connectMode,
            string subFolder = "")
        {
            var settings = new FtpSettings();
            settings.Url = url;
            settings.UseSsh = useSsh;
            settings.Username = username;
            settings.Password = password;
            settings.SubFolder = subFolder;
            settings.ConnectMode = connectMode;

            var client = new Client();
            var result = client.FolderList(settings);
            if (client.ErrorMessage != string.Empty)
            {
                return  InternalServerError(new System.Exception(client.ErrorMessage));
            }

            return Ok(result);
        }
    }
}