using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace frontlook_csharp_library.FL_General
{

    public enum FTP_Action
    {
        Upload,Download,Delete
    }
    public class FL_FTP
    {
        public FL_FTP()
        {

        }
        public string UserName { get;set; }
        public string Password { get;set; }
        public string FolderPath => Path.GetDirectoryName(FilePath);
        public string AbsolutePath { get;set; }
        public string FilePath;
        public string Host { get;set; }
        public FTP_Action Action { get;set; }
        public string RemoteFolderPath { get;set; }
        public bool EnableDefaultZip { get;set; }
        public string FileName => Path.GetFileName(FilePath);

        private string ZippedFilePath()
        {
            var zipFile = Path.Combine(Path.GetDirectoryName(AbsolutePath), Path.GetFileNameWithoutExtension(AbsolutePath) + ".zip");
            if (File.Exists(zipFile)) File.Delete(zipFile);
            
            //var x = Path.Combine(dir, Path.GetFileNameWithoutExtension(filePath) + "1.zip");
            //var y = Path.Combine(dir, Path.GetFileNameWithoutExtension(filePath) + "2.zip");
            using var zip = ZipFile.Open(zipFile, ZipArchiveMode.Create);
            zip.CreateEntryFromFile(AbsolutePath, Path.GetFileName(AbsolutePath), CompressionLevel.Optimal);
            /*using var zip1 = ZipFile.Open(x, ZipArchiveMode.Create);
            zip1.CreateEntryFromFile(filePath, Path.GetFileName(filePath), CompressionLevel.Optimal);
            using var zip2 = ZipFile.Open(y, ZipArchiveMode.Create);
            zip2.CreateEntryFromFile(filePath, Path.GetFileName(filePath), CompressionLevel.NoCompression);*/
            FilePath = zipFile;
            return zipFile;
        }

        /*public void ZippedFilePath1()
        {
            var strings = new[] { AbsolutePath, AbsolutePath + ".zip" };
            FL_PS.Execute("Compress-Archive", strings);
        }*/
        /*public void ZippedFilePath(string AbsolutePath)
        {
            var strings = new[] { AbsolutePath, AbsolutePath + ".zip" };
            FL_PS.Execute($"Compress-Archive {AbsolutePath} {AbsolutePath + ".zip"}");
        }*/

        public string Run()
        {
            if(Action == FTP_Action.Upload)
            {
                return Upload();
            }
            else if (Action == FTP_Action.Download)
            {
                return Download();
            }
            else if (Action == FTP_Action.Delete)
            {
                return Delete();
            }
            else
            {
                return "";//throw new Exception("FTP Action: Unknown parameters");
            }
        }

        private string Delete()
        {

            var _rPath = string.IsNullOrEmpty(RemoteFolderPath) ? "" : $"/{RemoteFolderPath}";
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"ftp://{Host}{_rPath}/{Path.GetFileName(AbsolutePath)}");

            //If you need to use network credentials
            request.Credentials = new NetworkCredential(UserName, Password);
            //additionally, if you want to use the current user's network credentials, just use:
            //System.Net.CredentialCache.DefaultNetworkCredentials

            request.Method = WebRequestMethods.Ftp.DeleteFile;
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            var c = $"Delete status: {response.StatusDescription}";
            Console.WriteLine(c);
            response.Close();
            return c;
        }

        private string Upload()
        {
            FilePath = FL_PS.Zipper(AbsolutePath, EnableDefaultZip);
            //FL_PS.UnZipper(AbsolutePath);
            using (var client = new WebClient())
            {
                client.Credentials = new NetworkCredential(UserName, Password);
                var _rPath = string.IsNullOrEmpty(RemoteFolderPath) ? "" : $"/{RemoteFolderPath}";
                client.UploadFile($"ftp://{Host}{_rPath}/{FileName}", WebRequestMethods.Ftp.UploadFile, FilePath);
            }
            return FilePath;
            //File.Delete(FilePath);
        }

        private string Download()
        {
            if (File.Exists(AbsolutePath))
            {
                File.Delete(AbsolutePath);
            }
            if (Directory.Exists(Path.Combine(Path.GetDirectoryName(AbsolutePath), Path.GetFileNameWithoutExtension(AbsolutePath))))
            {
                Directory.Delete(Path.Combine(Path.GetDirectoryName(AbsolutePath), Path.GetFileNameWithoutExtension(AbsolutePath)), true);
            }
            using var client = new WebClient();
            client.Credentials = new NetworkCredential(UserName, Password);
            var _rPath = string.IsNullOrEmpty(RemoteFolderPath) ? "" : $"/{RemoteFolderPath}";
            
            client.DownloadFile($"ftp://{Host}{_rPath}/{Path.GetFileName(AbsolutePath)}", AbsolutePath);
            return FL_PS.UnZipper(AbsolutePath, EnableDefaultZip);
        }

        /*public void UploadFtpFile()
        {
            FtpWebRequest request;
            ZippedFilePath();
            request = WebRequest.Create(new Uri(string.Format(@"ftp://{0}/{1}/{2}", Host, RemoteFolderPath, FileName))) as FtpWebRequest;
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.UseBinary = true;
            request.UsePassive = true;
            request.KeepAlive = true;
            request.Credentials = new NetworkCredential(UserName, Password);
            request.ConnectionGroupName = "group";

            using FileStream fs = File.OpenRead(FilePath);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(buffer, 0, buffer.Length);
            requestStream.Flush();
            requestStream.Close();
        }*/
    }
}
