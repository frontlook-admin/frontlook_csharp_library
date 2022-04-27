using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;

namespace frontlook_csharp_library.FL_General
{
	public static class FL_PS
	{
		/*public static void Execute()
        {
            RunspaceConfiguration runspaceConfiguration = RunspaceConfiguration.Create();

            Runspace runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration);
            runspace.Open();

            Pipeline pipeline = runspace.CreatePipeline();

            //Here's how you add a new script with arguments
            Command myCommand = new Command(scriptfile);
            CommandParameter testParam = new CommandParameter("key", "value");
            myCommand.Parameters.Add(testParam);

            pipeline.Commands.Add(myCommand);

            // Execute PowerShell script
            pipeline.Invoke();
        }*/

		/*public void Execute(string command, string[] parameters)
        {
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                // use "AddScript" to add the contents of a script file to the end of the execution pipeline.
                // use "AddCommand" to add individual commands/cmdlets to the end of the execution pipeline.
                PowerShellInstance.AddScript(command);
                //PowerShellInstance.AddScript("Test-Dtc -LocalComputerName param($param1) -RemoteComputerName param($param2) -Verbose");

                // use "AddParameter" to add a single parameter to the last command/script on the pipeline.
                var localComputer = Environment.GetEnvironmentVariable("COMPUTERNAME");
                PowerShellInstance.AddParameter("param1", localComputer);

                // use "AddParameter" to add a single parameter to the last command/script on the pipeline.
                var remoteComputer = "localhost";
                PowerShellInstance.AddParameter("param2", remoteComputer);

                // invoke execution on the pipeline (collecting output)
                Collection<PSObject> PSOutput = PowerShellInstance.Invoke();

                // loop through each output object item
                foreach (PSObject outputItem in PSOutput)
                {
                    // if null object was dumped to the pipeline during the script then a null
                    // object may be present here. check for null to prevent potential NRE.
                    if (outputItem != null)
                    {
                        //TODO: do something with the output item 
                        // outputItem.BaseOBject
                    }
                }
            }
        }*/

		public static void Execute(string command, string[] parameters)
		{
			using (PowerShell PowerShellInstance = PowerShell.Create())
			{
				// use "AddScript" to add the contents of a script file to the end of the execution pipeline.
				// use "AddCommand" to add individual commands/cmdlets to the end of the execution pipeline.
				PowerShellInstance.AddScript(command);
				//PowerShellInstance.AddScript("Test-Dtc -LocalComputerName param($param1) -RemoteComputerName param($param2) -Verbose");

				// use "AddParameter" to add a single parameter to the last command/script on the pipeline.

				PowerShellInstance.AddParameters(parameters);
				// invoke execution on the pipeline (collecting output)
				Collection<PSObject> PSOutput = PowerShellInstance.Invoke();

				// loop through each output object item
				foreach (PSObject outputItem in PSOutput)
				{
					// if null object was dumped to the pipeline during the script then a null
					// object may be present here. check for null to prevent potential NRE.
					if (outputItem != null)
					{
						//TODO: do something with the output item 
						// outputItem.BaseOBject
					}
				}
			}
		}
		public static void Execute(string command)
		{
			using (PowerShell PowerShellInstance = PowerShell.Create())
			{
				// use "AddScript" to add the contents of a script file to the end of the execution pipeline.
				// use "AddCommand" to add individual commands/cmdlets to the end of the execution pipeline.
				PowerShellInstance.AddScript(command);
				//PowerShellInstance.AddScript("Test-Dtc -LocalComputerName param($param1) -RemoteComputerName param($param2) -Verbose");

				// use "AddParameter" to add a single parameter to the last command/script on the pipeline.

				//PowerShellInstance.AddParameters(parameters);
				// invoke execution on the pipeline (collecting output)
				Collection<PSObject> PSOutput = PowerShellInstance.Invoke();

				// loop through each output object item
				foreach (PSObject outputItem in PSOutput)
				{
					// if null object was dumped to the pipeline during the script then a null
					// object may be present here. check for null to prevent potential NRE.
					if (outputItem != null)
					{
						//TODO: do something with the output item 
						// outputItem.BaseOBject
					}
				}
			}
		}


		private static string ZipMover(string AbsolutePath)
		{
			try
			{

				var From = Path.Combine(AbsolutePath, Path.GetFileName(AbsolutePath));
				var To = Path.GetDirectoryName(AbsolutePath);
				FL_PS.Execute($"Move-Item -Path '{From}' -Destination '{To}'");

				return AbsolutePath;
			}
			catch
			{
				throw;
			}

		}

		public static string UnZipper(string AbsolutePath, bool EnableDefault)
		{
			return UnZipper(AbsolutePath, AbsolutePath.Replace(".zip", ""), EnableDefault);
		}

		public static string UnZipper(string AbsoluteZipPath, string AbsolutePath, bool EnableDefault)
		{
			if (!EnableDefault)
			{
				try
				{
					var strings = new[] { AbsoluteZipPath, AbsolutePath };
					FL_PS.Execute($"Expand-Archive '{AbsoluteZipPath}' '{AbsolutePath}'");

					return ZipMover(AbsolutePath);
				}
				catch
				{
					throw;
				}
			}
			else
			{
				try
				{

					System.IO.Compression.ZipFile.ExtractToDirectory(AbsoluteZipPath, AbsolutePath);

					//return ZipMover(AbsolutePath);
					return Path.GetDirectoryName(AbsolutePath);
				}
				catch
				{
					throw;
				}
			}
		}

		public static string Zipper(string AbsolutePath, bool EnableDefault)
		{
			if (File.Exists(AbsolutePath + ".zip"))
			{
				File.Delete(AbsolutePath + ".zip");
			}
			return Zipper(AbsolutePath, AbsolutePath + ".zip", EnableDefault);
		}

		public static string Zipper(string AbsolutePath, string AbsoluteZipPath, bool EnableDefault)
		{
			if (!EnableDefault)
			{
				try
				{
					var strings = new[] { AbsolutePath, AbsoluteZipPath };
					FL_PS.Execute($"Compress-Archive '{AbsolutePath}' '{AbsoluteZipPath}'");
					//File.Delete(AbsolutePath);
					return AbsoluteZipPath;
				}
				catch
				{
					throw;
				}
			}
			else
			{
				try
				{
					System.IO.Compression.ZipFile.CreateFromDirectory(AbsolutePath, AbsoluteZipPath);
					return AbsoluteZipPath;
				}
				catch
				{
					throw;
				}
			}

		}


		public static void FL_ZipFile(this List<string> filesToZip, string path, int compression = 9)
		{

			if (compression < 0 || compression > 9)
				throw new ArgumentException("Invalid compression rate.");

			if (!Directory.Exists(new FileInfo(path).Directory.ToString()))
				throw new ArgumentException("The Path does not exist.");

			foreach (string c in filesToZip)
				if (!File.Exists(c))
					throw new ArgumentException(string.Format("The File {0} does not exist!", c));


			Crc32 crc32 = new();
			ZipOutputStream stream = new(File.Create(path));
			stream.SetLevel(compression);

			for (int i = 0; i < filesToZip.Count; i++)
			{
				ZipEntry entry = new(Path.GetFileName(filesToZip[i]));
				entry.DateTime = DateTime.Now;

				using FileStream fs = File.OpenRead(filesToZip[i]);
				byte[] buffer = new byte[fs.Length];
				fs.Read(buffer, 0, buffer.Length);
				entry.Size = fs.Length;
				fs.Close();
				crc32.Reset();
				crc32.Update(buffer);
				entry.Crc = crc32.Value;
				stream.PutNextEntry(entry);
				stream.Write(buffer, 0, buffer.Length);
			}
			stream.Finish();
			stream.Close();
		}
	}
}
