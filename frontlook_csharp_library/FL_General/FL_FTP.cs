using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;

namespace frontlook_csharp_library.FL_General
{

	public enum FTP_Action
	{
		Upload, Download, Delete, GetList
	}
	public enum FTP_SearchMode
	{
		Exact, Like, EndsWith, StartsWith
	}
	public class FL_FTP
	{
		public FL_FTP()
		{

		}
		public FL_FTP(FL_FTP f)
		{
			Host = f.Host;
			UserName = f.UserName;
			Password = f.Password;
			AbsolutePath = f.AbsolutePath;
			Extension = f.Extension;
			SearchMode = f.SearchMode;
			SearchString = f.SearchString;
			FilePath = f.FilePath;
			Action = f.Action;
			RemoteFolderPath = f.RemoteFolderPath;
			EnableDefaultZip = f.EnableDefaultZip;

		}

		public string UserName { get; set; }
		public string Password { get; set; }
		public string FolderPath => Path.GetDirectoryName(FilePath);
		public string AbsolutePath { get; set; }
		public string Extension { get; set; }

		public string BasePath { get => $"ftp://{Host}"; set => _ = $"ftp://{Host}"; }
		//public string RemotePath { get => string.IsNullOrEmpty(RemoteFolderPath) ? "" : $"/{RemoteFolderPath}"; set => _ = string.IsNullOrEmpty(RemoteFolderPath) ? "" : $"/{RemoteFolderPath}"; }
		//public string RemotePathFolder { get => string.IsNullOrEmpty(RemoteFolderPath) ? "" : $"{RemoteFolderPath}"; set => _ = string.IsNullOrEmpty(RemoteFolderPath) ? "" : $"{RemoteFolderPath}"; }
		//public string Dir { get => $"ftp://{Host}{RemotePath}"; set => _ = $"ftp://{Host}{RemotePath}"; }

		public string SearchString { get; set; }
		public FTP_SearchMode SearchMode { get; set; }
		public string FilePath;
		public string Host { get; set; }
		public FTP_Action Action { get; set; }
		public string RemoteFolderPath { get; set; }
		public bool EnableDefaultZip { get; set; }
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
			if (Action == FTP_Action.Upload)
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
			$"ftp://{Host}{_rPath}/{Path.GetFileName(AbsolutePath)}".FL_ConsoleWriteDebug();
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
		public void CreateDirectory(string ftpAddress)
		{
			FtpWebRequest reqFTP = null;
			Stream ftpStream = null;

			string[] subDirs = ftpAddress.Split('/');
			//string[] subDirs = ftpAddress.Split('\\');

			string currentDir = ftpAddress;

			foreach (string subDir in subDirs)
			{
				try
				{
					currentDir = currentDir + "/" + subDir;
					reqFTP = (FtpWebRequest)FtpWebRequest.Create(currentDir);
					reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
					reqFTP.UseBinary = true;
					reqFTP.Credentials = new NetworkCredential(UserName, Password);
					FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
					ftpStream = response.GetResponseStream();
					ftpStream.Close();
					response.Close();
				}
				catch (Exception ex)
				{
					throw;
					//directory already exist I know that is weak but there is no way to check if a folder exist on ftp...
				}
			}
		}
		/*public void CreateDirectory(string fileName)
		{
            WebRequest request = WebRequest.Create(fileName);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            request.Credentials = new NetworkCredential(UserName, Password);
            using (var resp = (FtpWebResponse)request.GetResponse())
            {
                Console.WriteLine(resp.StatusCode);
            }

        }*/

		private bool CheckIfFileExistsOnServer(string fileName)
		{
			var b = false;
			var request = (FtpWebRequest)WebRequest.Create(fileName);
			request.Credentials = new NetworkCredential(UserName, Password);
			request.Method = WebRequestMethods.Ftp.GetFileSize;

			try
			{
				FtpWebResponse response = (FtpWebResponse)request.GetResponse();
				b = true;

				response.Close();
			}
			catch (WebException ex)
			{
				FtpWebResponse response = (FtpWebResponse)ex.Response;
				if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
					b = false;

				response.Close();
			}

			return b;
		}
		public List<string> GetFileList()
		{
			try
			{
				var _rPath = string.IsNullOrEmpty(RemoteFolderPath) ? "" : $"/{RemoteFolderPath}";
				FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"ftp://{Host}{_rPath}");

				request.Credentials = new NetworkCredential(UserName, Password);
				request.Method = WebRequestMethods.Ftp.ListDirectory;
				FtpWebResponse response = (FtpWebResponse)request.GetResponse();
				Stream responseStream = response.GetResponseStream();
				StreamReader reader = new StreamReader(responseStream);

				string names = reader.ReadToEnd();

				reader.Close();
				response.Close();
				if (!string.IsNullOrEmpty(names))
				{
					var files = names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
					if (!string.IsNullOrEmpty(SearchString))
					{

						switch (SearchMode)
						{
							case FTP_SearchMode.Like:
								{
									files = files.Where(e => Path.GetFileNameWithoutExtension(e).ToLower().Contains(SearchString.ToLower())).ToList();
									break;
								}

							case FTP_SearchMode.StartsWith:
								{
									files = files.Where(e => Path.GetFileNameWithoutExtension(e).ToLower().StartsWith(SearchString.ToLower())).ToList();
									break;
								}

							case FTP_SearchMode.EndsWith:
								{
									files = files.Where(e => Path.GetFileNameWithoutExtension(e).ToLower().EndsWith(SearchString.ToLower())).ToList();
									break;
								}

							case FTP_SearchMode.Exact:
								{
									files = files.Where(e => Path.GetFileNameWithoutExtension(e).ToLower().Equals(SearchString.ToLower())).ToList();
									break;
								}
							default:
								{
									files = files.Where(e => Path.GetFileNameWithoutExtension(e).ToLower().Contains(SearchString.ToLower())).ToList();
									break;
								}
						}
					}
					if (files.Any())
					{

						if (string.IsNullOrEmpty(Extension))
						{
							return files.OrderByDescending(e => e).ToList();
						}
						else
						{
							if (files.Any(e => Path.GetExtension(e).ToUpper() == Extension.ToUpper()))
							{
								return files.Where(e => Path.GetExtension(e).ToUpper() == Extension.ToUpper()).OrderByDescending(e => e).ToList();
							}
							else
							{

								throw new Exception($"No file available with extension {Extension}..!!");
							}
						}
					}
					else
					{

						throw new Exception($"No file available with Search String: {SearchString}..!!");
					}
				}
				else
				{
					throw new Exception("No file available..!!");
				}

			}
			catch (Exception)
			{
				throw;
			}
		}

		private string Upload()
		{
			/*if(File.Exists(AbsolutePath + ".zip"))
			{
				File.Delete(AbsolutePath + ".zip");
			}*/

			FilePath = FL_PS.Zipper(AbsolutePath, EnableDefaultZip);
			//FL_PS.UnZipper(AbsolutePath);
			using (var client = new WebClient())
			{
				client.Credentials = new NetworkCredential(UserName, Password);
				var _rPath = string.IsNullOrEmpty(RemoteFolderPath) ? "" : $"/{RemoteFolderPath}";
				var dir = $"ftp://{Host}{_rPath}";
				/*if (CheckIfFileExistsOnServer(dir))
				{
                    CreateDirectory(dir);
				}*/
				client.UploadFile($"{dir}/{FileName}", WebRequestMethods.Ftp.UploadFile, FilePath);
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
