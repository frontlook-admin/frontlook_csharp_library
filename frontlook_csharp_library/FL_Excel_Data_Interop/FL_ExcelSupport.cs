using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JetBrains.Annotations;
using NPOI.SS.UserModel;
using HorizontalAlignment = NPOI.SS.UserModel.HorizontalAlignment;

namespace frontlook_csharp_library.FL_Excel_Data_Interop
{
    public static class FL_ExcelSupport
    {
        public static string GetFilePath()
        {
            var excelFilePath = "";
            var sf = new SaveFileDialog();
            sf.DefaultExt = "xlsx";
            sf.FileName = Path.GetFileName(excelFilePath);
            sf.InitialDirectory = Path.GetDirectoryName(excelFilePath);
            sf.Title = "Save Excel";
            sf.ShowDialog();

            excelFilePath = sf.FileName;
            return excelFilePath;
        }
    }

    public enum VTextAlign
    {
        None = -1, // 0xFFFFFFFF
        Top = 0,
        Center = 1,
        Bottom = 2,
        Justify = 3,
        Distributed = 4,
    }

    public enum HTextAlign
    {
        General,
        Left,
        Center,
        Right,
        Fill,
        Justify,
        CenterSelection,
        Distributed,
    }

    public class UserDetails
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }

    public class FL_ExcelDataItem
    {
        public FL_ExcelDataItem()
        {
        }

        public FL_ExcelDataItem([CanBeNull] object _ItemValue, int _VItemPosition, List<FL_ExcelDataItem> _HItemPosition, VTextAlign vTextAlign = FL_Excel_Data_Interop.VTextAlign.Center, HTextAlign hTextAlign = FL_Excel_Data_Interop.HTextAlign.Left, bool Bold = false)
        {
            ItemValue = _ItemValue == null ? "" : _ItemValue.ToString();
            VItemPosition = _VItemPosition;
            HItemPosition = _HItemPosition.Count(e => e.VItemPosition == _VItemPosition);
            HTextAlign = (HorizontalAlignment)hTextAlign;
            VTextAlign = (VerticalAlignment)vTextAlign;
            bold = Bold;
        }

        public FL_ExcelDataItem([CanBeNull] object _ItemValue, int _VItemPosition, int _HItemPosition, VTextAlign vTextAlign = FL_Excel_Data_Interop.VTextAlign.Center, HTextAlign hTextAlign = FL_Excel_Data_Interop.HTextAlign.Left, bool Bold = false)
        {
            ItemValue = _ItemValue == null ? "" : _ItemValue.ToString();
            VItemPosition = _VItemPosition;
            HItemPosition = _HItemPosition;
            HTextAlign = (HorizontalAlignment)hTextAlign;
            VTextAlign = (VerticalAlignment)vTextAlign;
            bold = Bold;
        }

        public string ItemValue { get; set; }
        public int VItemPosition { get; set; }
        public int HItemPosition { get; set; }
        public bool? bold { get; set; }
        public HorizontalAlignment? HTextAlign { get; set; }
        public VerticalAlignment? VTextAlign { get; set; }
    }

    public class FL_ExcelData
    {
        public string SheetName { get; set; }
        public int SheetNumber { get; set; }
        public int? ColSplit { get; set; }
        public List<FL_ExcelDataItem> ExcelDataItems { get; set; }
        public List<string> HeadNames { get; set; }
        public List<List<object>> SheetOData { get; set; }
        public List<List<string>> SheetData => SheetOData.Select(e => e.Select(f => f.ToString()).ToList()).ToList();
        public bool? EnableNumberCheck { get; set; }
        public bool? SaveExcelAsFile { get; set; }
        public string FilePath { get; set; }
    }

    public class FL_ExcelDataDT
    {
        public string SheetName { get; set; }
        public int SheetNumber { get; set; }
        public int? ColSplit { get; set; }
        public List<FL_ExcelDataItem> ExcelDataItems { get; set; }
        public List<string> HeadNames { get; set; }
        public DataTable SheetData { get; set; }
        public bool? EnableNumberCheck { get; set; }

        public bool? SaveExcelAsFile { get; set; }
        public string FilePath { get; set; }
    }
}