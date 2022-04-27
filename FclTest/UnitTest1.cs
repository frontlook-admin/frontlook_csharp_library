using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace FclTest
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			//var F = "   &".FL_SetNewCode();
			try
			{
				var f = new FL_FTP();
				f.AbsolutePath = "C:\\Users\\deban\\Downloads\\20190401_20200331_AceReporterSync";
				f.Action = FTP_Action.Upload;
				f.Host = "103.87.174.250";
				f.UserName = "administrator";
				f.Password = "Deb@301191";
				f.Run();

				f = new FL_FTP();
				f.AbsolutePath = "C:\\Users\\deban\\Downloads\\20190401_20200331_AceReporterSync";
				f.Action = FTP_Action.Download;
				f.Host = "103.87.174.250";
				f.UserName = "administrator";
				f.Password = "Deb@301191";
				f.Run();
			}
			catch (Exception ex)
			{
				throw ex;
			}


		}
	}

	public enum FTP_Action
	{
		Upload, Download
	}
	public class FL_FTP
	{
		public FL_FTP()
		{

		}
		public string UserName { get; set; }
		public string Password { get; set; }
		public string FolderPath => Path.GetDirectoryName(FilePath);
		public string AbsolutePath { get; set; }
		public string FilePath;
		public string Host { get; set; }
		public FTP_Action Action { get; set; }
		public string RemoteFolderPath { get; set; }
		public string FileName => Path.GetFileName(FilePath);

		private string ZippedFilePath()
		{
			var zipFile = Path.Combine(Path.GetDirectoryName(AbsolutePath), Path.GetFileNameWithoutExtension(AbsolutePath) + ".zip");
			if (File.Exists(zipFile)) File.Delete(zipFile);

			//var x = Path.Combine(dir, Path.GetFileNameWithoutExtension(filePath) + "1.zip");
			//var y = Path.Combine(dir, Path.GetFileNameWithoutExtension(filePath) + "2.zip");
			ZipArchive zipArchive = ZipFile.Open(zipFile, ZipArchiveMode.Create);
			using (var zip = zipArchive)
			{

				zip.CreateEntryFromFile(AbsolutePath, Path.GetFileName(AbsolutePath), CompressionLevel.Optimal);
			}
			/*using var zip1 = ZipFile.Open(x, ZipArchiveMode.Create);
            zip1.CreateEntryFromFile(filePath, Path.GetFileName(filePath), CompressionLevel.Optimal);
            using var zip2 = ZipFile.Open(y, ZipArchiveMode.Create);
            zip2.CreateEntryFromFile(filePath, Path.GetFileName(filePath), CompressionLevel.NoCompression);*/
			FilePath = zipFile;
			return zipFile;
		}

		public void Run()
		{
			if (Action == FTP_Action.Upload)
			{
				Upload();
			}
			else
			{
				Download();
			}
		}

		private void Upload()
		{
			//ZippedFilePath();
			using (var client = new WebClient())
			{
				client.Credentials = new NetworkCredential(UserName, Password);
				client.UploadFile($"ftp://{Host}/{RemoteFolderPath}/{Path.GetFileName(AbsolutePath)}", WebRequestMethods.Ftp.UploadFile, AbsolutePath);
			}
			//File.Delete(FilePath);
		}

		private void Download()
		{
			using (var client = new WebClient())
			{
				client.Credentials = new NetworkCredential(UserName, Password);
				client.DownloadFile($"ftp://{Host}/{RemoteFolderPath}/{Path.GetFileName(AbsolutePath)}", AbsolutePath);
			}

		}

		public void UploadFtpFile()
		{
			FtpWebRequest request;
			//ZippedFilePath();
			request = WebRequest.Create(new Uri(string.Format(@"ftp://{0}/{1}/{2}", Host, RemoteFolderPath, Path.GetFileName(AbsolutePath)))) as FtpWebRequest;
			request.Method = WebRequestMethods.Ftp.UploadFile;
			request.UseBinary = true;
			request.UsePassive = true;
			request.KeepAlive = true;
			request.Credentials = new NetworkCredential(UserName, Password);
			request.ConnectionGroupName = "group";

			using (FileStream fs = File.OpenRead(AbsolutePath))
			{
				byte[] buffer = new byte[fs.Length];
				fs.Read(buffer, 0, buffer.Length);
				fs.Close();
				Stream requestStream = request.GetRequestStream();
				requestStream.Write(buffer, 0, buffer.Length);
				requestStream.Flush();
				requestStream.Close();
			}

		}
	}
}