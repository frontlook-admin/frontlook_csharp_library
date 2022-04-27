using FastReport;
using FastReport.Export;
using FastReport.Utils;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;

namespace frontlook_csharp_library.FL_General
{
	public static class FL_PrintPageSettings
	{
		private const float scaleFactor = 300 / 96f;

		public static PaperSize FL_SetPaperSize(this PrinterSettings ps, int sizeName, bool Check = true)
		{
			List<PaperSize> paperSizes = ps.PaperSizes.Cast<PaperSize>().ToList();
			if (Check)
			{
				PaperSize size = null;
				/*foreach (var pz in paperSizes)
                {
                    // setting paper size to A4 size
                    size = paperSizes.First(e => e.RawKind == sizeName);
                }*/
				size = paperSizes.First(e => e.RawKind == sizeName);
				return size;
			}
			else
			{
				return paperSizes.First();
			}
		}

		public static PaperSize FL_SetPaperSize(this PrinterSettings ps, int width, int height)
		{
			List<PaperSize> paperSizes = ps.PaperSizes.Cast<PaperSize>().ToList();
			PaperSize size = null;

			var b = paperSizes.Any(e => e.Width == width && e.Height == height);
			if (!b)
			{
				var v = new PaperSize("", width, height);
				return v;
			}
			else
			{
				size = paperSizes.First(e => e.Width == width && e.Height == height);
				return size;
			}
		}

		public static int FL_GetPaperSize(this PrinterSettings ps)
		{
			var sizeName = ps.DefaultPageSettings.PaperSize.RawKind;

			return sizeName;
		}

		public static int FL_GetPaperSize(this PageSettings ps)
		{
			var sizeName = ps.PaperSize.RawKind;

			return sizeName;
		}

		public static PageSettings FL_GetPageSettings(this Report report)
		{
			var page = 0;
			var rPage = report.PreparedPages.GetPage(page);
			var p = new PageSettings();
			p.Landscape = rPage.Landscape;
			p.PaperSize = new PaperSize("Custom",
				(int)(ExportUtils.GetPageWidth(rPage) * scaleFactor * Units.HundrethsOfInch),
				(int)(ExportUtils.GetPageHeight(rPage) * scaleFactor * Units.HundrethsOfInch));
			p.Margins =
				new Margins(
					(int)(scaleFactor * rPage.LeftMargin * Units.HundrethsOfInch),
					(int)(scaleFactor * rPage.RightMargin * Units.HundrethsOfInch),
					(int)(scaleFactor * rPage.TopMargin * Units.HundrethsOfInch),
					(int)(scaleFactor * rPage.BottomMargin * Units.HundrethsOfInch));

			return p;
		}
	}
}