using JetBrains.Annotations;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
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

		public static IFont FL_GetDefaultSuperHeadFont(this IWorkbook workbook, short? FontSize = null, [CanBeNull] string FontName = null, bool? Bold = null)
		{
			var fontHead = workbook.CreateFont();
			fontHead.FontHeightInPoints = FontSize ?? 13;
			fontHead.FontName = FontName ?? "Calibri";
			fontHead.Boldweight = Bold.GetValueOrDefault(true) ? (short)FontBoldWeight.Bold : (short)FontBoldWeight.Normal;

			return fontHead;
		}

		public static ICellStyle FL_CreateDefaultSuperHeadStyle(this IWorkbook workbook, HorizontalAlignment? HTextAlign = null, VerticalAlignment? VTextAlign = null, bool? FontWrap = null, short? CellColor = null, CellFillPattern? fillPattern = null, short? FontSize = null, [CanBeNull] string FontName = null, bool? FontBold = null)
		{
			var CellStyle = workbook.CreateCellStyle();
			CellStyle.SetFont(workbook.FL_GetDefaultSuperHeadFont(FontSize, FontName, FontBold));
			CellStyle.Alignment = HTextAlign ?? HorizontalAlignment.Center;
			CellStyle.VerticalAlignment = VTextAlign ?? VerticalAlignment.Center;
			CellStyle.WrapText = FontWrap.GetValueOrDefault(true);
			CellStyle.FillForegroundColor = CellColor ?? IndexedColors.LightGreen.Index;
			CellStyle.FillPattern = (FillPattern)(fillPattern ?? CellFillPattern.SolidForeground);
			return CellStyle;
		}

		public static ICellStyle FL_CreateDefaultSuperHeadStyle(this IWorkbook workbook, FL_ExcelSuperHeadDataItem Item)
		{
			return workbook.FL_CreateDefaultSuperHeadStyle(Item.HTextAlign, Item.VTextAlign, Item.FontWrap, Item.CellColor, Item.CellFillPattern, Item.FontSize, Item.FontName, Item.FontBold);
		}

		public static IFont FL_GetDefaultHeadFont(this IWorkbook workbook, short? FontSize = null, [CanBeNull] string FontName = null, bool? Bold = null)
		{
			var fontHead = workbook.CreateFont();
			fontHead.FontHeightInPoints = FontSize ?? 12;
			fontHead.FontName = FontName ?? "Calibri";
			fontHead.Boldweight = Bold.GetValueOrDefault(true) ? (short)FontBoldWeight.Bold : (short)FontBoldWeight.Normal;

			return fontHead;
		}

		public static ICellStyle FL_CreateDefaultHeadStyle(this IWorkbook workbook, HorizontalAlignment? HTextAlign = null, VerticalAlignment? VTextAlign = null, bool? FontWrap = null, short? CellColor = null, CellFillPattern? fillPattern = null, short? FontSize = null, [CanBeNull] string FontName = null, bool? FontBold = null)
		{
			var CellStyle = workbook.CreateCellStyle();

			CellStyle.SetFont(workbook.FL_GetDefaultSuperHeadFont(FontSize, FontName, FontBold));
			CellStyle.Alignment = HTextAlign ?? HorizontalAlignment.Center;
			CellStyle.VerticalAlignment = VTextAlign ?? VerticalAlignment.Center;
			CellStyle.WrapText = FontWrap.GetValueOrDefault(true);
			CellStyle.FillForegroundColor = CellColor ?? IndexedColors.LightTurquoise.Index;
			CellStyle.FillPattern = (FillPattern)(fillPattern ?? CellFillPattern.SolidForeground);
			return CellStyle;
		}

		public static ICellStyle FL_CreateDefaultHeadStyle(this IWorkbook workbook, FL_ExcelDataItem Item)
		{
			return workbook.FL_CreateDefaultHeadStyle(Item.HTextAlign, Item.VTextAlign, Item.FontWrap, Item.CellColor, Item.CellFillPattern, Item.FontSize, Item.FontName, Item.FontBold);
		}

		public static IFont FL_GetDefaultFont(this IWorkbook workbook, short? FontSize = null, [CanBeNull] string FontName = null, bool? Bold = null)
		{
			var font = workbook.CreateFont();
			font.FontHeightInPoints = FontSize ?? 11;
			font.FontName = FontName ?? "Calibri";
			font.Boldweight = Bold.GetValueOrDefault(false) ? (short)FontBoldWeight.Bold : (short)FontBoldWeight.Normal;
			return font;
		}

		public static ICellStyle FL_CreateDefaultStyle(this IWorkbook workbook, HorizontalAlignment? HTextAlign = null, VerticalAlignment? VTextAlign = null, bool? FontWrap = null, short? CellColor = null, CellFillPattern? fillPattern = null, short? FontSize = null, [CanBeNull] string FontName = null, bool? FontBold = null, bool? IfNumber = null)
		{
			var CellStyle = workbook.CreateCellStyle();

			CellStyle.SetFont(workbook.FL_GetDefaultFont(FontSize, FontName, FontBold));
			CellStyle.Alignment = IfNumber.GetValueOrDefault(false) ? HorizontalAlignment.Right : HTextAlign ?? HorizontalAlignment.Center;
			CellStyle.VerticalAlignment = VTextAlign ?? VerticalAlignment.Center;
			CellStyle.WrapText = FontWrap.GetValueOrDefault(true);
			if (CellColor != null)
			{
				CellStyle.FillForegroundColor = CellColor.Value;

				CellStyle.FillPattern = (FillPattern)(fillPattern ?? CellFillPattern.SolidForeground);
			}

			return CellStyle;
		}

		public static ICellStyle FL_CreateDefaultStyle(this IWorkbook workbook, FL_ExcelDataItem Item, bool? IfNumber = null)
		{
			return workbook.FL_CreateDefaultStyle(Item.HTextAlign, Item.VTextAlign, Item.FontWrap, Item.CellColor, Item.CellFillPattern, Item.FontSize, Item.FontName, Item.FontBold, IfNumber);
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

	public enum CellFillPattern : short
	{
		NoFill = 0,
		SolidForeground = 1,
		FineDots = 2,
		AltBars = 3,
		SparseDots = 4,
		ThickHorizontalBands = 5,
		ThickVerticalBands = 6,
		ThickBackwardDiagonals = 7,
		ThickForwardDiagonals = 8,
		BigSpots = 9,
		Bricks = 10,
		ThinHorizontalBands = 11,
		ThinVerticalBands = 12,
		ThinBackwardDiagonals = 13,
		ThinForwardDiagonals = 14,
		Squares = 15,
		Diamonds = 16,
		LessDots = 17,
		LeastDots = 18
	}

	public class UserDetails
	{
		public string ID { get; set; }
		public int IDCODE { get; set; }
		public string Name { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
	}

	public class FL_ExcelDataItemBase
	{
		public FL_ExcelDataItemBase()
		{
		}

		public bool? ApplyStyles { get; set; }

		public HorizontalAlignment? HTextAlign { get; set; }
		public VerticalAlignment? VTextAlign { get; set; }
		public short? CellColor { get; set; }
		public CellFillPattern? CellFillPattern { get; set; }
	}

	public class FL_ExcelDataItem : FL_ExcelDataItemBase
	{
		public FL_ExcelDataItem()
		{
		}

		public FL_ExcelDataItem([CanBeNull] object _ItemValue, short? cellColor = null, CellFillPattern? fillPattern = null, bool Bold = false, bool _ApplyStyles = true)
		{
			ItemValue = _ItemValue == null ? "" : _ItemValue;
			FontBold = Bold;
			CellColor = cellColor;
			ApplyStyles = _ApplyStyles;
			CellFillPattern = fillPattern;
		}
		[CanBeNull]
		public object ItemValue { get; set; }
		public CellType? cellType { get; set; }
		public short? FontSize { get; set; }
		[CanBeNull]
		public string FontName { get; set; }
		public bool? FontBold { get; set; }
		public bool? FontWrap { get; set; }
	}

	public class FL_ExcelSuperHeadDataItem : FL_ExcelDataItem
	{
		public FL_ExcelSuperHeadDataItem()
		{
		}

		public FL_ExcelSuperHeadDataItem([CanBeNull] object _ItemValue, int _VItemPosition, List<FL_ExcelSuperHeadDataItem> _HItemPosition, VTextAlign vTextAlign = FL_Excel_Data_Interop.VTextAlign.Center, HTextAlign hTextAlign = FL_Excel_Data_Interop.HTextAlign.Left, short? cellColor = null, CellFillPattern? fillPattern = null, bool Bold = false, bool _ApplyStyles = true)
		{
			ItemValue = _ItemValue == null ? "" : _ItemValue.ToString();
			VItemPosition = _VItemPosition;
			HItemPosition = _HItemPosition.Count(e => e.VItemPosition == _VItemPosition);
			HTextAlign = (HorizontalAlignment)hTextAlign;
			VTextAlign = (VerticalAlignment)vTextAlign;
			FontBold = Bold;
			CellColor = cellColor;
			CellFillPattern = fillPattern;
			ApplyStyles = _ApplyStyles;
		}

		public FL_ExcelSuperHeadDataItem([CanBeNull] object _ItemValue, int _VItemPosition, int _HItemPosition, VTextAlign vTextAlign = FL_Excel_Data_Interop.VTextAlign.Center, HTextAlign hTextAlign = FL_Excel_Data_Interop.HTextAlign.Left, short? cellColor = null, CellFillPattern? fillPattern = null, bool Bold = false, bool _ApplyStyles = true)
		{
			ItemValue = _ItemValue == null ? "" : _ItemValue.ToString();
			VItemPosition = _VItemPosition;
			HItemPosition = _HItemPosition;
			HTextAlign = (HorizontalAlignment)hTextAlign;
			VTextAlign = (VerticalAlignment)vTextAlign;
			FontBold = Bold;
			CellColor = cellColor;
			CellFillPattern = fillPattern;
			ApplyStyles = _ApplyStyles;
		}
		public int VItemPosition { get; set; }
		public int HItemPosition { get; set; }
	}

	public class FL_ExcelDataIT
	{
		public string SheetName { get; set; }
		public int SheetNumber { get; set; }
		public int? ColSplit { get; set; }
		public bool? EnableNumberCheck { get; set; }
		public List<FL_ExcelSuperHeadDataItem> ExcelSuperHeadDataItems { get; set; }
		public List<FL_ExcelDataItem> ExcelHeads { get; set; }
		public List<List<FL_ExcelDataItem>> ExcelDataItems { get; set; }
		public DataTable SheetData { get; set; }
		public bool? ApplySuperHeadStyles { get; set; }
		public bool? ApplyHeadStyles { get; set; }
		public bool? ApplyStyles { get; set; }
		public bool? SaveExcelAsFile { get; set; }
		public string FilePath { get; set; }
	}

	public class FL_ExcelData
	{
		public string SheetName { get; set; }
		public int SheetNumber { get; set; }
		public int? ColSplit { get; set; }
		public List<FL_ExcelSuperHeadDataItem> ExcelSuperHeadDataItems { get; set; }
		public List<string> HeadNames { get; set; }
		public List<List<object>> SheetOData { get; set; }
		public List<List<string>> SheetData => SheetOData.Select(e => e.Select(f => f.ToString()).ToList()).ToList();
		public bool? EnableNumberCheck { get; set; }
		public bool? ApplySuperHeadStyles { get; set; }
		public bool? FontWrap { get; set; }
		public bool? ApplyHeadStyles { get; set; }
		public bool? ApplyStyles { get; set; }
		public bool? SaveExcelAsFile { get; set; }
		public string FilePath { get; set; }
	}
	public class FL_ExcelDataDT
	{
		public string SheetName { get; set; }
		public int SheetNumber { get; set; }
		public int? ColSplit { get; set; }
		public List<FL_ExcelSuperHeadDataItem> ExcelSuperHeadDataItems { get; set; }
		public List<string> HeadNames { get; set; }
		public DataTable SheetData { get; set; }
		public bool? EnableNumberCheck { get; set; }
		public bool? ApplySuperHeadStyles { get; set; }
		public bool? ApplyHeadStyles { get; set; }
		public bool? ApplyStyles { get; set; }
		public bool? SaveExcelAsFile { get; set; }
		public string FilePath { get; set; }
	}
}