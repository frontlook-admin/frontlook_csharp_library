using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frontlook_csharp_library.FL_General
{
    public static class FL_PrintPageSettings
    {
        public static PaperSize FL_SetPaperSize(this PrinterSettings ps, int sizeName)
        {
            List<PaperSize> paperSizes = ps.PaperSizes.Cast<PaperSize>().ToList();
            PaperSize size = null;
            foreach (var pz in paperSizes)
            {
                size = paperSizes.First(e => e.RawKind == sizeName);  // setting paper size to A4 size

            }

            return size;
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
    }
}
