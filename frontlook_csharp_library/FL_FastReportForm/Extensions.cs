using FastReport;
using frontlook_csharp_library.FL_General;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace frontlook_csharp_library.FL_FastReportForm
{
	public static class Extensions
	{
		#region Fields

		private const float scaleFactor = 300 / 96f;

		#endregion Fields

		#region Methods

		public static void PrintWithDialog(this Report report)
		{
			using (var dlg = new PrintDialog())
			{
				dlg.AllowSomePages = true;
				dlg.AllowSelection = true;
				dlg.UseEXDialog = true;

				if (dlg.ShowDialog() != DialogResult.OK) return;

				report.Print(dlg.PrinterSettings);
			}
		}

		public static void PrintWithDialogMod(this Report report)
		{
			using (var dlg = new PrintDialog())
			{
				dlg.AllowSomePages = true;
				dlg.AllowSelection = true;
				dlg.UseEXDialog = true;

				if (dlg.ShowDialog() != DialogResult.OK) return;

				report.PrintMod(dlg.PrinterSettings);
			}
		}

		public static void Print(this Report report, PrinterSettings settings = null)
		{
			var doc = report.PrepareDoc(settings);
			if (doc == null) return;

			doc.Print();
			doc.Dispose();
		}

		public static void PrintMod(this Report report, PrinterSettings settings = null)
		{
			var doc = report.PrepareDocMod(settings);
			if (doc == null) return;

			doc.Print();
			doc.Dispose();
		}

		public static void Preview(this Report report, FRPrintPreviewControl preview, PrinterSettings settings = null)
		{
			var doc = report.PrepareDoc(settings);
			if (doc == null) return;

			preview.Document = doc;
		}

		public static void Preview(this Report report, FRPrintPreviewControlAlt preview, PrinterSettings settings = null)
		{
			var doc = report.PrepareDoc(settings);
			if (doc == null) return;

			preview.Document = doc;
		}

		public static void PreviewMod(this Report report, FRPrintPreviewControl preview, PrinterSettings settings = null)
		{
			var doc = report.PrepareDocMod(settings);
			if (doc == null) return;

			preview.Document = doc;
		}

		public static void PreviewMod(this Report report, FRPrintPreviewControlAlt preview, PrinterSettings settings = null)
		{
			var doc = report.PrepareDocMod(settings);
			if (doc == null) return;

			preview.Document = doc;
		}

		public static void Preview(this Report report, PrinterSettings settings = null)
		{
			var doc = report.PrepareDoc(settings);
			if (doc == null) return;

			using (var preview = new FRPrintPreviewDialog() { Document = doc })
				preview.ShowDialog();
			doc.Dispose();
		}

		public static void PreviewMod(this Report report, PrinterSettings settings = null)
		{
			var doc = report.PrepareDocMod(settings);
			if (doc == null) return;

			using (var preview = new FRPrintPreviewDialog() { Document = doc })
				preview.ShowDialog();
			doc.Dispose();
		}

		public static PrinterSettings? GetSetPrinterSettings(PrintType printType, string PrintSettingDirPath, string PrintSettingFilePath, Report Rep)
		{
			if (printType == PrintType.Default)
			{
				using var pd = new PrintDialog();
				//pd.PrinterSettings.
				var pdSettingList = new List<string>();
				if (File.Exists(PrintSettingFilePath))
				{
					var pf = File.ReadAllLines(PrintSettingFilePath);
					if (pf.Length == 5)
					{
						pd.AllowSomePages = true;
						pd.AllowSelection = true;
						pd.AllowPrintToFile = true;
						pd.UseEXDialog = true;
						pd.ShowNetwork = true;
						pd.PrinterSettings.PrintRange = PrintRange.AllPages;
						pd.PrinterSettings.PrinterName = pf[0];
						pd.PrinterSettings.Copies = (short)int.Parse(pf[1]);
						pd.PrinterSettings.PrintToFile = bool.Parse(pf[2]);
						pd.PrinterSettings.Collate = bool.Parse(pf[3]);
						var px = Rep.FL_GetPageSettings();
						px.Margins = new Margins(0, 0, 0, 0);
						pd.PrinterSettings.DefaultPageSettings.PaperSize = px.PaperSize;
						pd.PrinterSettings.DefaultPageSettings.Landscape = bool.Parse(pf[4]);
						return pd.PrinterSettings;
					}

					pd.AllowSomePages = true;
					pd.AllowSelection = true;
					pd.AllowPrintToFile = true;
					pd.UseEXDialog = true;
					pd.PrinterSettings.PrintRange = PrintRange.AllPages;
					if (pd.ShowDialog() != DialogResult.OK) return null;
					pdSettingList.Add(pd.PrinterSettings.PrinterName);
					pdSettingList.Add(pd.PrinterSettings.Copies.ToString());
					pdSettingList.Add(pd.PrinterSettings.PrintToFile.ToString());
					pdSettingList.Add(pd.PrinterSettings.Collate.ToString());
					pdSettingList.Add(pd.PrinterSettings.FL_GetPaperSize().ToString());
					pdSettingList.Add(pd.PrinterSettings.DefaultPageSettings.Landscape.ToString());

					setUpPrinter(pdSettingList, PrintSettingDirPath, PrintSettingFilePath);
					return pd.PrinterSettings;
				}

				pd.AllowSomePages = true;
				pd.AllowSelection = true;
				pd.AllowPrintToFile = true;
				pd.ShowNetwork = true;
				pd.UseEXDialog = true;
				pd.PrinterSettings.PrintRange = PrintRange.AllPages;
				if (pd.ShowDialog() != DialogResult.OK) return null;
				pdSettingList.Add(pd.PrinterSettings.PrinterName);
				pdSettingList.Add(pd.PrinterSettings.Copies.ToString());
				pdSettingList.Add(pd.PrinterSettings.PrintToFile.ToString());
				pdSettingList.Add(pd.PrinterSettings.Collate.ToString());

				pdSettingList.Add(pd.PrinterSettings.DefaultPageSettings.Landscape.ToString());
				setUpPrinter(pdSettingList, PrintSettingDirPath, PrintSettingFilePath);
				return pd.PrinterSettings;
			}
			else
			{
				using var pd = new PrintDialog();
				var pdSettingList = new List<string>();
				if (File.Exists(PrintSettingFilePath))
				{
					var pf = File.ReadAllLines(PrintSettingFilePath);
					if (pf.Length == 6)
					{
						pd.AllowSomePages = true;
						pd.AllowSelection = true;
						pd.AllowPrintToFile = true;
						pd.UseEXDialog = true;
						pd.ShowNetwork = true;
						pd.PrinterSettings.PrintRange = PrintRange.AllPages;
						pd.PrinterSettings.PrinterName = pf[0];
						pd.PrinterSettings.Copies = (short)int.Parse(pf[1]);
						pd.PrinterSettings.PrintToFile = bool.Parse(pf[2]);
						pd.PrinterSettings.Collate = bool.Parse(pf[3]);
						var pz = pd.PrinterSettings.FL_SetPaperSize(int.Parse(pf[4]));
						pd.PrinterSettings.DefaultPageSettings.PaperSize = pz;
						pd.PrinterSettings.DefaultPageSettings.Landscape = bool.Parse(pf[5]);
						return pd.PrinterSettings;
					}

					pd.AllowSomePages = true;
					pd.AllowSelection = true;
					pd.AllowPrintToFile = true;
					pd.UseEXDialog = true;
					pd.PrinterSettings.PrintRange = PrintRange.AllPages;
					if (pd.ShowDialog() != DialogResult.OK) return null;
					pdSettingList.Add(pd.PrinterSettings.PrinterName);
					pdSettingList.Add(pd.PrinterSettings.Copies.ToString());
					pdSettingList.Add(pd.PrinterSettings.PrintToFile.ToString());
					pdSettingList.Add(pd.PrinterSettings.Collate.ToString());
					pdSettingList.Add(pd.PrinterSettings.FL_GetPaperSize().ToString());
					pdSettingList.Add(pd.PrinterSettings.DefaultPageSettings.Landscape.ToString());

					setUpPrinter(pdSettingList, PrintSettingDirPath, PrintSettingFilePath);
					return pd.PrinterSettings;
				}

				pd.AllowSomePages = true;
				pd.AllowSelection = true;
				pd.AllowPrintToFile = true;
				pd.ShowNetwork = true;
				pd.UseEXDialog = true;
				pd.PrinterSettings.PrintRange = PrintRange.AllPages;
				if (pd.ShowDialog() != DialogResult.OK) return null;
				pdSettingList.Add(pd.PrinterSettings.PrinterName);
				pdSettingList.Add(pd.PrinterSettings.Copies.ToString());
				pdSettingList.Add(pd.PrinterSettings.PrintToFile.ToString());
				pdSettingList.Add(pd.PrinterSettings.Collate.ToString());
				pdSettingList.Add(pd.PrinterSettings.FL_GetPaperSize().ToString());
				pdSettingList.Add(pd.PrinterSettings.DefaultPageSettings.Landscape.ToString());
				setUpPrinter(pdSettingList, PrintSettingDirPath, PrintSettingFilePath);
				return pd.PrinterSettings;
			}
		}



		public static void SetPrinterSettings(PrintType printType, string PrintSettingDirPath, string PrintSettingFilePath)
		{
			if (printType == PrintType.Default)
			{
				var pdSettingList = new List<string>();
				using (var pd = new PrintDialog())
				{
					pd.AllowSomePages = true;
					pd.AllowSelection = true;
					pd.ShowNetwork = true;
					pd.AllowPrintToFile = true;
					pd.UseEXDialog = true;
					pd.PrinterSettings.PrintRange = PrintRange.AllPages;

					if (Directory.Exists(PrintSettingDirPath) && File.Exists(PrintSettingFilePath))
					{
						var pf = File.ReadAllLines(PrintSettingFilePath);
						if (pf.Length == 5)
						{
							pd.PrinterSettings.PrinterName = pf[0];
							pd.PrinterSettings.Copies = (short)int.Parse(pf[1]);
							pd.PrinterSettings.PrintToFile = bool.Parse(pf[2]);
							pd.PrinterSettings.Collate = bool.Parse(pf[3]);
							//var pz = pd.PrinterSettings.FL_SetPaperSize(int.Parse(pf[4]));
							//pd.PrinterSettings.DefaultPageSettings.PaperSize = pz;
							//pd.PrinterSettings.DefaultPageSettings.Landscape = bool.Parse(pf[5]);
						}
					}

					if (pd.ShowDialog() != DialogResult.OK) return;
					pdSettingList.Add(pd.PrinterSettings.PrinterName);
					pdSettingList.Add(pd.PrinterSettings.Copies.ToString());
					pdSettingList.Add(pd.PrinterSettings.PrintToFile.ToString());
					pdSettingList.Add(pd.PrinterSettings.Collate.ToString());
					//pdSettingList.Add(pd.PrinterSettings.FL_GetPaperSize().ToString());
					pdSettingList.Add(pd.PrinterSettings.DefaultPageSettings.Landscape.ToString());
					setUpPrinter(pdSettingList, PrintSettingDirPath, PrintSettingFilePath);
				}
			}
			else
			{
				var pdSettingList = new List<string>();
				using (var pd = new PrintDialog())
				{
					pd.AllowSomePages = true;
					pd.AllowSelection = true;
					pd.ShowNetwork = true;
					pd.AllowPrintToFile = true;
					pd.UseEXDialog = true;
					pd.PrinterSettings.PrintRange = PrintRange.AllPages;

					if (Directory.Exists(PrintSettingDirPath) && File.Exists(PrintSettingFilePath))
					{
						var pf = File.ReadAllLines(PrintSettingFilePath);
						if (pf.Length == 6)
						{
							pd.PrinterSettings.PrinterName = pf[0];
							pd.PrinterSettings.Copies = (short)int.Parse(pf[1]);
							pd.PrinterSettings.PrintToFile = bool.Parse(pf[2]);
							pd.PrinterSettings.Collate = bool.Parse(pf[3]);
							var pz = pd.PrinterSettings.FL_SetPaperSize(int.Parse(pf[4]));
							pd.PrinterSettings.DefaultPageSettings.PaperSize = pz;
							pd.PrinterSettings.DefaultPageSettings.Landscape = bool.Parse(pf[5]);
						}
					}

					if (pd.ShowDialog() != DialogResult.OK) return;
					pdSettingList.Add(pd.PrinterSettings.PrinterName);
					pdSettingList.Add(pd.PrinterSettings.Copies.ToString());
					pdSettingList.Add(pd.PrinterSettings.PrintToFile.ToString());
					pdSettingList.Add(pd.PrinterSettings.Collate.ToString());
					pdSettingList.Add(pd.PrinterSettings.FL_GetPaperSize().ToString());
					pdSettingList.Add(pd.PrinterSettings.DefaultPageSettings.Landscape.ToString());
					setUpPrinter(pdSettingList, PrintSettingDirPath, PrintSettingFilePath);
				}
			}
		}

		private static void setUpPrinter(List<string> pds, string PrintSettingDirPath, string PrintSettingFilePath)
		{
			if (!Directory.Exists(PrintSettingDirPath))
			{
				Directory.CreateDirectory(PrintSettingDirPath);
			}
			File.Delete(PrintSettingFilePath);
			File.AppendAllLines(PrintSettingFilePath, pds);
			//pds.ForEach(e => );
		}

		public enum PrintType
		{
			Default = 1,
			Mod = 2
		}

		/*
                private static PrintDocument PrepareDoc(this Report report, PrinterSettings settings = null)
                {
                    if (report.PreparedPages.Count < 1)
                    {
                        report.Prepare();
                        if (report.PreparedPages.Count < 1) return null;
                    }

                    var page = 0;
                    var exp = new ImageExport { ImageFormat = ImageExportFormat.Png, Resolution = 600 };

                    var doc = new PrintDocument { DocumentName = report.Name };
                    if (settings != null)
                        doc.PrinterSettings = settings;

                    // Ajustando o tamanho da pagina
                    doc.QueryPageSettings += (sender, args) =>
                    {
                        var rPage = report.PreparedPages.GetPage(page);
                        args.PageSettings.Landscape = rPage.Landscape;
                        args.PageSettings.Margins = new Margins((int)(scaleFactor * rPage.LeftMargin * Units.HundrethsOfInch),
                                                                (int)(scaleFactor * rPage.RightMargin * Units.HundrethsOfInch),
                                                                (int)(scaleFactor * rPage.TopMargin * Units.HundrethsOfInch),
                                                                (int)(scaleFactor * rPage.BottomMargin * Units.HundrethsOfInch));

                        args.PageSettings.PaperSize = new PaperSize("Custom", (int)(ExportUtils.GetPageWidth(rPage) * scaleFactor * Units.HundrethsOfInch),
                                                                              (int)(ExportUtils.GetPageHeight(rPage) * scaleFactor * Units.HundrethsOfInch));
                    };

                    doc.PrintPage += (sender, args) =>
                    {
                        using (var ms = new MemoryStream())
                        {
                            exp.PageRange = PageRange.PageNumbers;
                            exp.PageNumbers = $"{page + 1}";
                            exp.Export(report, ms);

                            args.Graphics.DrawImage(Image.FromStream(ms), args.PageBounds);
                        }

                        page++;

                        args.HasMorePages = page < report.PreparedPages.Count;
                    };

                    doc.EndPrint += (sender, args) => page = 0;
                    doc.Disposed += (sender, args) => exp?.Dispose();

                    return doc;
                }

                private static PrintDocument PrepareDocMod(this Report report, PrinterSettings settings = null)
                {
                    if (report.PreparedPages.Count < 1)
                    {
                        report.Prepare();
                        if (report.PreparedPages.Count < 1) return null;
                    }

                    var page = 0;
                    var exp = new ImageExport { ImageFormat = ImageExportFormat.Png, Resolution = 600 };

                    var doc = new PrintDocument { DocumentName = report.Name };
                    if (settings != null)
                        doc.PrinterSettings = settings;

                    // Ajustando o tamanho da pagina
                    doc.QueryPageSettings += (sender, args) =>
                    {
                        if (settings != null)
                        {
                            args.PageSettings = settings.DefaultPageSettings;
                        }
                        else
                        {
                            var rPage = report.PreparedPages.GetPage(page);

                            args.PageSettings.Landscape = rPage.Landscape;
                            args.PageSettings.PaperSize = new PaperSize("Custom",
                                (int)(ExportUtils.GetPageWidth(rPage) * scaleFactor * Units.HundrethsOfInch),
                                (int)(ExportUtils.GetPageHeight(rPage) * scaleFactor * Units.HundrethsOfInch));
                            args.PageSettings.Margins =
                                new Margins(
                                (int)(scaleFactor * rPage.LeftMargin * Units.HundrethsOfInch),
                                (int)(scaleFactor * rPage.RightMargin * Units.HundrethsOfInch),
                                (int)(scaleFactor * rPage.TopMargin * Units.HundrethsOfInch),
                                (int)(scaleFactor * rPage.BottomMargin * Units.HundrethsOfInch));
                        }
                    };

                    doc.PrintPage += (sender, args) =>
                    {
                        using (var ms = new MemoryStream())
                        {
                            exp.PageRange = PageRange.PageNumbers;
                            exp.PageNumbers = $"{page + 1}";
                            exp.Export(report, ms);

                            args.Graphics.DrawImage(Image.FromStream(ms), args.PageBounds);
                        }

                        page++;

                        args.HasMorePages = page < report.PreparedPages.Count;
                    };

                    doc.EndPrint += (sender, args) => page = 0;
                    doc.Disposed += (sender, args) => exp?.Dispose();

                    return doc;
                }
        */

		#endregion Methods
	}
}