using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using frontlook_csharp_library.FL_General;
using frontlook_csharp_library.FL_General.FL_string_helper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace frontlook_csharp_library.FL_Excel_Data_Interop
{
    public static partial class FL_Excel
    {
        #region Excel Reader

        /// <summary>
        /// Generates List of string list
        /// </summary>
        /// <param name="file">Excel File</param>
        /// <returns>List Of string List</returns>
        public static async Task<List<FL_ExcelData>> FL_GetExcelDatabaseAsync(this IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                return null;
            }

            var sFileExtension = Path.GetExtension(file.FileName)?.ToLower();

            var FileData = new List<FL_ExcelData>();
            //await using var stream = new FileStream(fullPath, FileMode.Create);
            //await file.CopyToAsync(stream);
            using var stream = file.FL_GetFileStream();

            stream.Position = 0;
            //ISheet sheet;
            if (sFileExtension == ".xls")
            {
                var hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats
                var sheetCount = hssfwb.NumberOfSheets;
                for (var i = 0; i < sheetCount; i++)
                {
                    var sht = hssfwb.GetSheetAt(i);
                    var dt = new FL_ExcelData
                    {
                        SheetName = sht.SheetName,
                        SheetNumber = i,
                        SheetOData = FL_GetExcelODataFromSheet(sht)
                    };
                    FileData.Add(dt);
                }
            }
            else
            {
                var hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format
                var sheetCount = hssfwb.NumberOfSheets;
                for (var i = 0; i < sheetCount; i++)
                {
                    var sht = hssfwb.GetSheetAt(i);
                    var dt = new FL_ExcelData
                    {
                        SheetName = sht.SheetName,
                        SheetNumber = i,
                        SheetOData = FL_GetExcelODataFromSheet(sht)
                    };
                    FileData.Add(dt);
                }
            }

            return FileData;
        }

        public static async Task<List<FL_ExcelData>> FL_GetExcelDatabaseAsync(this IFormFile file, string folderPath,
            string filename)
        {
            if (file == null || file.Length <= 0)
            {
                return null;
            }

            var tempFile = file;
            var sFileExtension = Path.GetExtension(tempFile.FileName)?.ToLower();

            if (string.IsNullOrEmpty(sFileExtension))
            {
                return null;
            }

            filename = (string.IsNullOrEmpty(filename) ? Guid.NewGuid().ToString() : filename) + sFileExtension;
            //var webRootPath = hosting_environment.WebRootPath;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fullPath = Path.Combine(folderPath, filename);
            await tempFile.FL_CopyFileAsync(fullPath);

            return await FL_GetExcelDatabaseAsync(file);
        }

        #endregion Excel Reader

        #region Excel Writer

        public static void FL_WriteExcelAsync(this List<string> HeadList,
            List<List<string>> dataList, string FilePath = "", bool SaveExcelAsFile = true, bool EnableNumberCheck = true, bool ApplyStyles = true, bool ApplyHeadStyles = true)
        {
            try
            {
                var sheet = new FL_ExcelData()
                {
                    EnableNumberCheck = EnableNumberCheck,
                    ApplySuperHeadStyles = ApplyHeadStyles,
                    ApplyHeadStyles = ApplyHeadStyles,
                    ApplyStyles = ApplyStyles,
                    SheetOData = dataList.Select(e => e.Select(f => (object)f).ToList()).ToList(),
                    HeadNames = HeadList,
                    SaveExcelAsFile = SaveExcelAsFile,
                    FilePath = FilePath
                };
                sheet.FL_WriteExcelAsync();
            }
            catch
            {

                var filePath = SaveExcelAsFile ? FilePath : Path.GetTempFileName();
                var ms = new MemoryStream();
                using var tempStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                IWorkbook workbook = new XSSFWorkbook();

                var excelSheet = workbook.CreateSheet("Sheet1");

                var row = excelSheet.CreateRow(0);

                var fontHead = workbook.CreateFont();
                fontHead.FontHeightInPoints = 11;
                fontHead.FontName = "Calibri";
                fontHead.Boldweight = (short)FontBoldWeight.Bold;

                var font = workbook.CreateFont();
                font.FontHeightInPoints = 11;
                font.FontName = "Calibri";
                font.Boldweight = (short)FontBoldWeight.Normal;

                for (var i = 0; i < HeadList.Count; i++)
                {
                    var head = HeadList[i];
                    //row.CreateCell(columnIndex).SetCellValue(head);

                    var cell = row.CreateCell(i);
                    cell.SetCellValue(head);
                    if (!ApplyHeadStyles) continue;
                    cell.CellStyle = workbook.CreateCellStyle();
                    cell.CellStyle.SetFont(fontHead);
                    cell.CellStyle.Alignment = HorizontalAlignment.Center;
                    cell.CellStyle.VerticalAlignment = VerticalAlignment.Center;
                    cell.CellStyle.WrapText = true;
                    cell.CellStyle.FillBackgroundColor = new XSSFColor(Color.DarkTurquoise).Indexed;

                    //columnIndex++;
                }

                //var rowIndex = 1;
                for (var i = 0; i < dataList.Count; i++)
                {
                    var columns = dataList[i];
                    row = excelSheet.CreateRow(i + 1);
                    var cellIndex = 0;
                    foreach (var cell in columns.Select(col => row.CreateCell(cellIndex)))
                    {
                        var str = columns[cellIndex];
                        var b = EnableNumberCheck && double.TryParse(str, out _);
                        if (b)
                        {
                            cell.SetCellValue(double.Parse(str));
                            cell.SetCellType(CellType.Numeric);
                        }
                        else
                        {
                            cell.SetCellValue(str);
                            cell.SetCellType(CellType.String);
                        }

                        cellIndex++;
                        if (!ApplyStyles) continue;
                        //cell.SetCellType(columns[cellIndex].ToString().FL_SetICellType());
                        cell.CellStyle = workbook.CreateCellStyle();
                        cell.CellStyle.SetFont(font);
                        cell.CellStyle.Alignment = b ? HorizontalAlignment.Right : HorizontalAlignment.Left;
                        cell.CellStyle.VerticalAlignment = VerticalAlignment.Center;
                    }
                }

                excelSheet.CreateFreezePane(0, 1, 0, 1);
                workbook.Write(tempStream);
            }
           
        }

        public static void FL_WriteExcelAsync(this DataTable dataTable,
            [CanBeNull] List<string> HeadList = null, string FilePath = "", bool SaveExcelAsFile = true, bool EnableNumberCheck = true, bool ApplyStyles = true, bool ApplyHeadStyles = true)
        {
            try
            {
                var sheetDT = new FL_ExcelDataDT()
                {
                    EnableNumberCheck = EnableNumberCheck,
                    ApplySuperHeadStyles = ApplyHeadStyles,
                    ApplyHeadStyles = ApplyHeadStyles,
                    ApplyStyles = ApplyStyles,
                    SheetData = dataTable,
                    HeadNames = HeadList,
                    SaveExcelAsFile = SaveExcelAsFile,
                    FilePath = FilePath
                };
                sheetDT.FL_WriteExcelAsync();
            }
            catch
            {
                var filePath = SaveExcelAsFile ? FilePath : Path.GetTempFileName();
                var ms = new MemoryStream();
                using var tempStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                IWorkbook workbook = new XSSFWorkbook();
                var excelSheet = workbook.CreateSheet("Sheet1");

                var columns = new List<string>();
                var row = excelSheet.CreateRow(0);

                var fontHead = workbook.CreateFont();
                fontHead.FontHeightInPoints = 11;
                fontHead.FontName = "Calibri";
                fontHead.Boldweight = (short)FontBoldWeight.Bold;
                var font = workbook.CreateFont();
                font.FontHeightInPoints = 11;
                font.FontName = "Calibri";
                font.Boldweight = (short)FontBoldWeight.Normal;

                //var columnIndex = 0;
                if (HeadList != null)
                {
                    for (var i = 0; i < HeadList.Count(); i++)
                    {
                        columns.Add(HeadList[i].FL_UnSpaced());
                        var cell = row.CreateCell(i);
                        cell.SetCellValue(HeadList[i]);
                        if (!ApplyHeadStyles) continue;
                        cell.CellStyle = workbook.CreateCellStyle();
                        cell.CellStyle.SetFont(fontHead);
                        cell.CellStyle.Alignment = HorizontalAlignment.Center;
                        cell.CellStyle.VerticalAlignment = VerticalAlignment.Center;
                        cell.CellStyle.WrapText = true;
                        cell.CellStyle.FillBackgroundColor = new XSSFColor(Color.DarkTurquoise).Indexed;
                        //columnIndex++;
                    }
                }
                else
                {
                    for (var i = 0; i < dataTable.Columns.Count; i++)
                    {
                        DataColumn column = dataTable.Columns[i];
                        columns.Add(column.ColumnName);
                        var cell = row.CreateCell(i);
                        cell.SetCellValue(column.ColumnName);
                        if (!ApplyHeadStyles) continue;
                        cell.CellStyle = workbook.CreateCellStyle();
                        cell.CellStyle.SetFont(fontHead);
                        cell.CellStyle.Alignment = HorizontalAlignment.Center;
                        cell.CellStyle.VerticalAlignment = VerticalAlignment.Center;
                        cell.CellStyle.WrapText = true;
                        cell.CellStyle.FillBackgroundColor = new XSSFColor(Color.DarkTurquoise).Indexed;
                        //columnIndex++;
                    }
                }

                //var rowIndex = 1;
                for (var j = 0; j < dataTable.Rows.Count; j++)
                {
                    var dsrow = dataTable.Rows[j];
                    row = excelSheet.CreateRow(j + 1);
                    var cellIndex = 0;
                    for (var i = 0; i < columns.Count; i++)
                    {
                        var col = columns[i];
                        var cell = row.CreateCell(cellIndex);
                        var str = dsrow[col].ToString();

                        var b = EnableNumberCheck && double.TryParse(str, out _);
                        if (b)
                        {
                            cell.SetCellValue(double.Parse(str));
                            cell.SetCellType(CellType.Numeric);
                        }
                        else
                        {
                            cell.SetCellValue(str);
                            cell.SetCellType(CellType.String);
                        }

                        cellIndex++;
                        if (!ApplyStyles) continue;
                        //cell.SetCellType(columns[cellIndex].ToString().FL_SetICellType());
                        cell.CellStyle = workbook.CreateCellStyle();
                        cell.CellStyle.SetFont(font);
                        cell.CellStyle.Alignment = b ? HorizontalAlignment.Right : HorizontalAlignment.Left;
                        cell.CellStyle.VerticalAlignment = VerticalAlignment.Center;

                        /*if (str.FL_GetStringType() == FL_StringHelper.FL_StringType.String ||
                            str.FL_GetStringType() == FL_StringHelper.FL_StringType.NullOrEmpty)
                        {
                            cell.SetCellValue(str);
                        }
                        if (str.FL_GetStringType() == FL_StringHelper.FL_StringType.Int)
                        {
                            cell.SetCellValue(int.Parse(str));
                        }
                        if (str.FL_GetStringType() == FL_StringHelper.FL_StringType.Double)
                        {
                            cell.SetCellValue(double.Parse(str));
                        }
                        if (str.FL_GetStringType() == FL_StringHelper.FL_StringType.Date)
                        {
                            cell.SetCellValue(DateTime.ParseExact(str, FL_DateHelper.DateParseFormats, CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            cell.SetCellValue(str);
                        }

                        */
                    }

                    //rowIndex++;
                }

                excelSheet.CreateFreezePane(0, 1, 0, 1);
                workbook.Write(tempStream);
            }
           
        }

        public static void FL_WriteExcelWorkBook(this FL_ExcelData Sheet, IWorkbook workbook, int SheetNumber = 0)
        {
            var excelSheet = string.IsNullOrEmpty(Sheet.SheetName) ? SheetNumber == 0 ? workbook.CreateSheet("Sheet1") : workbook.CreateSheet($"Sheet{SheetNumber}") : workbook.CreateSheet(Sheet.SheetName);

            var EnableNumberCheck = Sheet.EnableNumberCheck.GetValueOrDefault(true);
            var DataItems = Sheet.ExcelSuperHeadDataItems.Any() ?
                Sheet.ExcelSuperHeadDataItems.OrderBy(e => e.VItemPosition).ThenBy(e => e.HItemPosition).ToList() :
                new List<FL_ExcelSuperHeadDataItem>();
            var HeadList = Sheet.HeadNames;
            var dataList = Sheet.SheetData;
            var ApplyStyles = Sheet.ApplyStyles.GetValueOrDefault(true);
            var ApplyHeadStyles = Sheet.ApplyHeadStyles.GetValueOrDefault(true);
            var ApplySuperHeadStyles = Sheet.ApplySuperHeadStyles.GetValueOrDefault(true);

            var row = excelSheet.CreateRow(0);
            //var columnIndex = 0;
            var rowIndex = 0;
            if (DataItems.Count > 0)
            {
                var _rowIndex = DataItems.Max(e => e.VItemPosition);
                for (var i = 0; i <= _rowIndex; i++)
                {
                    var itms = DataItems.Where(e => e.VItemPosition == i).OrderBy(e => e.HItemPosition).ToList();
                    row = excelSheet.CreateRow(i);
                    foreach (var _itm in itms)
                    {
                        var cell = row.CreateCell(_itm.HItemPosition);
                        cell.SetCellValue(_itm.ItemValue.ToString());
                        if (!_itm.ApplyStyles.GetValueOrDefault(true)) continue;
                        cell.CellStyle = workbook.FL_CreateDefaultSuperHeadStyle(FontWrap: Sheet.FontWrap.GetValueOrDefault(false), HTextAlign:HorizontalAlignment.Left);
                    }
                }

                rowIndex = _rowIndex + 1;
            }
            var x = false;
            if (HeadList != null && HeadList.Any())
            {
                row = excelSheet.CreateRow(rowIndex);
                for (var i = 0; i < HeadList.Count(); i++)
                {
                    var cell = row.CreateCell(i);
                    cell.SetCellValue(HeadList[i]);
                    if (!ApplyHeadStyles) continue;
                    cell.CellStyle = workbook.FL_CreateDefaultHeadStyle(FontWrap: Sheet.FontWrap.GetValueOrDefault(false));

                    //columnIndex++;
                }

                rowIndex++;
                x = true;
            }

            var topRow = rowIndex;

            //var rowIndex = 1;
            for (var i = 0; i < dataList.Count; i++)
            {
                var columns = dataList[i];
                row = excelSheet.CreateRow(rowIndex);
                var cellIndex = 0;
                foreach (var cell in columns.Select(col => row.CreateCell(cellIndex)))
                {
                    var str = columns[cellIndex];
                    var b = EnableNumberCheck && double.TryParse(str, out _);

                    if (b)
                    {
                        cell.SetCellValue(double.Parse(str));
                        cell.SetCellType(CellType.Numeric);
                    }
                    else
                    {
                        cell.SetCellValue(str);
                        cell.SetCellType(CellType.String);
                    }

                    cellIndex++;
                    if (!ApplyStyles) continue;
                    cell.CellStyle = workbook.FL_CreateDefaultStyle(IfNumber: b, FontWrap: Sheet.FontWrap.GetValueOrDefault(false));
                }

                rowIndex++;
            }

            if (x)
            {
                excelSheet.CreateFreezePane(Sheet.ColSplit.GetValueOrDefault(0), topRow, Sheet.ColSplit.GetValueOrDefault(0), topRow);
            }
        }

        public static void FL_WriteExcelAsync(this FL_ExcelData Sheet)
        {
            if (Sheet == null) return;
            var filePath = Sheet.SaveExcelAsFile.GetValueOrDefault(true) ? Sheet.FilePath : Path.GetTempFileName();
            var ms = new MemoryStream();
            using var tempStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            IWorkbook workbook = new XSSFWorkbook();
            Sheet.FL_WriteExcelWorkBook(workbook);

            workbook.Write(tempStream);
        }

        public static void FL_WriteExcelAsync(this List<FL_ExcelData> SheetList)
        {
            if (SheetList == null || SheetList.Count == 0) return;
            var filePath = SheetList.First().SaveExcelAsFile.GetValueOrDefault(true) ? SheetList.First().FilePath : Path.GetTempFileName();
            var ms = new MemoryStream();
            using var tempStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            IWorkbook workbook = new XSSFWorkbook();
            var sheetNo = 1;
            SheetList.ForEach(sheet =>
            {
                sheet.FL_WriteExcelWorkBook(workbook, sheetNo);
                sheetNo++;
            });

            workbook.Write(tempStream);
        }

        public static void FL_WriteExcelWorkBook(this FL_ExcelDataDT SheetDT, IWorkbook workbook, int SheetNumber = 0)
        {
            var excelSheet = string.IsNullOrEmpty(SheetDT.SheetName) ? SheetNumber == 0 ? workbook.CreateSheet("Sheet1") : workbook.CreateSheet($"Sheet{SheetNumber}") : workbook.CreateSheet(SheetDT.SheetName);

            var ApplyStyles = SheetDT.ApplyStyles.GetValueOrDefault(true);
            var ApplyHeadStyles = SheetDT.ApplyHeadStyles.GetValueOrDefault(true);
            var ApplySuperHeadStyles = SheetDT.ApplySuperHeadStyles.GetValueOrDefault(true);

            var EnableNumberCheck = SheetDT.EnableNumberCheck.GetValueOrDefault(true);
            var DataItems = SheetDT.ExcelSuperHeadDataItems.Any() ?
                SheetDT.ExcelSuperHeadDataItems.OrderBy(e => e.VItemPosition).ThenBy(e => e.HItemPosition).ToList() :
                new List<FL_ExcelSuperHeadDataItem>();
            var dataTable = SheetDT.SheetData;
            var HeadList = SheetDT.HeadNames;

            var columns = new List<string>();
            var row = excelSheet.CreateRow(0);

            //var columnIndex = 0;
            var rowIndex = 0;
            if (DataItems.Count > 0)
            {
                var _rowIndex = DataItems.Max(e => e.VItemPosition);
                for (var i = 0; i <= _rowIndex; i++)
                {
                    var itms = DataItems.Where(e => e.VItemPosition == i).OrderBy(e => e.HItemPosition).ToList();
                    row = excelSheet.CreateRow(i);
                    foreach (var _itm in itms)
                    {
                        var cell = row.CreateCell(_itm.HItemPosition);
                        cell.SetCellValue(_itm.ItemValue.ToString());
                        if (!_itm.ApplyStyles.GetValueOrDefault(true)) continue;
                        cell.CellStyle = workbook.FL_CreateDefaultSuperHeadStyle();
                    }
                }

                rowIndex = _rowIndex + 1;
            }
            //var columnIndex = 0;
            if (HeadList != null && HeadList.Any())
            {
                row = excelSheet.CreateRow(rowIndex);
                for (var i = 0; i < HeadList.Count(); i++)
                {
                    columns.Add(HeadList[i].FL_UnSpaced());
                    var cell = row.CreateCell(i);
                    cell.SetCellValue(HeadList[i]);
                    if (!ApplyHeadStyles) continue;
                    cell.CellStyle = workbook.FL_CreateDefaultHeadStyle();
                }

                rowIndex++;
            }
            else
            {
                row = excelSheet.CreateRow(rowIndex);
                for (var i = 0; i < dataTable.Columns.Count; i++)
                {
                    var column = dataTable.Columns[i];
                    columns.Add(column.ColumnName);
                    var cell = row.CreateCell(i);
                    cell.SetCellValue(column.ColumnName);
                    if (!ApplyHeadStyles) continue;
                    cell.CellStyle = workbook.FL_CreateDefaultHeadStyle();
                }

                rowIndex++;
            }

            var topRow = rowIndex;
            //var rowIndex = 1;
            for (var j = 0; j < dataTable.Rows.Count; j++)
            {
                var dsrow = dataTable.Rows[j];
                if (rowIndex != 0)
                {
                    row = excelSheet.CreateRow(rowIndex);
                }
                var cellIndex = 0;
                for (var i = 0; i < columns.Count; i++)
                {
                    // var _colType = dataTable.Columns[columns[i]].DataType;
                    var col = columns[i];
                    var cell = row.CreateCell(cellIndex);
                    //var str = dsrow[col].ToString();
                    var _dType = dataTable.Columns[columns[i]].DataType;
                    var val = dsrow[col];
                    var b = _dType == typeof(double) || _dType == typeof(int) || _dType == typeof(float) || _dType == typeof(decimal) || _dType == typeof(long);
                    if (b)
                    {
                        if (_dType == typeof(double))
                        {
                            var _val = ((double?)val);
                            if (_val.GetValueOrDefault(0) != 0)
                            {
                                cell.SetCellValue(_val.Value);
                            }
                        }
                        else if (_dType == typeof(int))
                        {
                            var _val = ((int?)val);
                            if (_val.GetValueOrDefault(0) != 0)
                            {
                                cell.SetCellValue(_val.Value);
                            }
                        }
                        else if (_dType == typeof(float))
                        {
                            var _val = ((float?)val);
                            if (_val.GetValueOrDefault(0) != 0)
                            {
                                cell.SetCellValue(_val.Value);
                            }
                        }
                        else if (_dType == typeof(decimal))
                        {
                            var _val = ((decimal?)val);
                            if (_val.GetValueOrDefault(0) != 0)
                            {
                                cell.SetCellValue((double)_val.Value);
                            }
                        }
                        else if (_dType == typeof(long))
                        {
                            var _val = ((long?)val);
                            if (_val.GetValueOrDefault(0) != 0)
                            {
                                cell.SetCellValue(_val.Value);
                            }
                        }
                        else
                        {
                            cell.SetCellValue(val.ToString());
                        }
                        cell.SetCellType(CellType.Numeric);
                    }
                    else
                    {
                        cell.SetCellValue(val.ToString());
                        cell.SetCellType(CellType.String);
                    }
                    cellIndex++;
                    if (!ApplyStyles) continue;
                    cell.CellStyle = workbook.FL_CreateDefaultStyle(IfNumber: b);
                }

                rowIndex++;
            }

            excelSheet.CreateFreezePane(SheetDT.ColSplit.GetValueOrDefault(0), topRow, SheetDT.ColSplit.GetValueOrDefault(0), topRow);
        }

        public static void FL_WriteExcelAsync(this FL_ExcelDataDT SheetDT)
        {
            if (SheetDT == null) return;
            var filePath = SheetDT.SaveExcelAsFile.GetValueOrDefault(true) ? SheetDT.FilePath : Path.GetTempFileName();
            var ms = new MemoryStream();
            using var tempStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            IWorkbook workbook = new XSSFWorkbook();
            SheetDT.FL_WriteExcelWorkBook(workbook);
            workbook.Write(tempStream);
        }

        public static void FL_WriteExcelAsync(this List<FL_ExcelDataDT> SheetDTList)
        {
            if (SheetDTList == null || SheetDTList.Count == 0) return;
            var filePath = SheetDTList.First().SaveExcelAsFile.GetValueOrDefault(true) ? SheetDTList.First().FilePath : Path.GetTempFileName();
            var ms = new MemoryStream();
            using var tempStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            IWorkbook workbook = new XSSFWorkbook();
            var sheet = 1;
            SheetDTList.ForEach(SheetDT =>
            {
                SheetDT.FL_WriteExcelWorkBook(workbook, sheet);
                sheet++;
            });
            workbook.Write(tempStream);
            /*
            using var stream = new FileStream(filePath, FileMode.Open);
            await stream.CopyToAsync(ms);
            stream.Close();
            stream.Dispose();
            return ms;*/
        }

        public static void FL_WriteExcelWorkBook(this FL_ExcelDataIT SheetIT, IWorkbook workbook, int SheetNumber = 0)
        {
            var excelSheet = string.IsNullOrEmpty(SheetIT.SheetName) ? SheetNumber == 0 ? workbook.CreateSheet("Sheet1") : workbook.CreateSheet($"Sheet{SheetNumber}") : workbook.CreateSheet(SheetIT.SheetName);
            var EnableNumberCheck = SheetIT.EnableNumberCheck.GetValueOrDefault(true);
            var ExcelSuperHeadDataItems = SheetIT.ExcelSuperHeadDataItems.Any() ?
                SheetIT.ExcelSuperHeadDataItems.OrderBy(e => e.VItemPosition).ThenBy(e => e.HItemPosition).ToList() :
                new List<FL_ExcelSuperHeadDataItem>();
            var ApplySuperHeadStyles = SheetIT.ApplySuperHeadStyles.GetValueOrDefault(false);
            var ApplyHeadStyles = SheetIT.ApplyHeadStyles.GetValueOrDefault(false);
            var ApplyStyles = SheetIT.ApplyStyles.GetValueOrDefault(false);
            var ExcelDataItems = SheetIT.ExcelDataItems;
            var ExcelHeads = SheetIT.ExcelHeads;
            var dataTable = SheetIT.SheetData;

            var columns = new List<string>();
            var row = excelSheet.CreateRow(0);

            //var columnIndex = 0;
            var rowIndex = 0;
            if (ExcelSuperHeadDataItems.Count > 0)
            {
                var _rowIndex = ExcelSuperHeadDataItems.Max(e => e.VItemPosition);
                for (var i = 0; i <= _rowIndex; i++)
                {
                    var itms = ExcelSuperHeadDataItems.Where(e => e.VItemPosition == i).OrderBy(e => e.HItemPosition).ToList();
                    row = excelSheet.CreateRow(i);
                    foreach (var _itm in itms)
                    {
                        var cell = row.CreateCell(_itm.HItemPosition);
                        cell.SetCellValue(_itm.ItemValue.ToString());
                        if (!_itm.ApplyStyles.GetValueOrDefault(true) && !ApplySuperHeadStyles) continue;
                        cell.CellStyle = workbook.FL_CreateDefaultSuperHeadStyle(_itm);
                    }
                }

                rowIndex = _rowIndex + 1;
            }
            //var columnIndex = 0;
            if (ExcelHeads != null && ExcelHeads.Any())
            {
                row = excelSheet.CreateRow(rowIndex);
                for (var i = 0; i < ExcelHeads.Count(); i++)
                {
                    var ix = ExcelHeads[i];
                    columns.Add(ix.ItemValue.ToString().FL_UnSpaced());
                    var cell = row.CreateCell(i);
                    cell.SetCellValue(ix.ItemValue.ToString());
                    if (!ix.ApplyStyles.GetValueOrDefault(false) && !ApplyHeadStyles) continue;
                    var fontHead = workbook.FL_CreateDefaultHeadStyle(ix);

                    //columnIndex++;
                }

                rowIndex++;
            }
            else if (dataTable != null)
            {
                row = excelSheet.CreateRow(rowIndex);
                for (var i = 0; i < dataTable.Columns.Count; i++)
                {
                    var column = dataTable.Columns[i];
                    columns.Add(column.ColumnName);
                    var cell = row.CreateCell(i);
                    cell.SetCellValue(column.ColumnName);
                    if (!ApplyStyles) continue;
                    var fontHead = workbook.FL_CreateDefaultHeadStyle();
                    //columnIndex++;
                }

                rowIndex++;
            }
            var topRow = rowIndex;
            //var rowIndex = 1;
            if (ExcelDataItems != null && ExcelDataItems.Count > 0)
            {
                for (var j = 0; j < ExcelDataItems.Count; j++)
                {
                    var dsrow = ExcelDataItems[j];
                    if (rowIndex != 0)
                    {
                        row = excelSheet.CreateRow(rowIndex);
                    }
                    var cellIndex = 0;
                    for (var i = 0; i < dsrow.Count; i++)
                    {
                        var cell = row.CreateCell(cellIndex);
                        var ix = dsrow[i];
                        var icell = row.CreateCell(i);
                        //var str = dsrow[col].ToString();
                        if (ix.ItemValue == null) continue;
                        var _dType = ix.ItemValue.GetType();
                        var b = _dType == typeof(double) || _dType == typeof(int) || _dType == typeof(float) || _dType == typeof(decimal) || _dType == typeof(long);
                        var val = ix.ItemValue;
                        if (b)
                        {
                            if (_dType == typeof(double))
                            {
                                var _val = ((double?)val);
                                if (_val.GetValueOrDefault(0) != 0)
                                {
                                    cell.SetCellValue(_val.Value);
                                }
                            }
                            else if (_dType == typeof(int))
                            {
                                var _val = ((int?)val);
                                if (_val.GetValueOrDefault(0) != 0)
                                {
                                    cell.SetCellValue(_val.Value);
                                }
                            }
                            else if (_dType == typeof(float))
                            {
                                var _val = ((float?)val);
                                if (_val.GetValueOrDefault(0) != 0)
                                {
                                    cell.SetCellValue(_val.Value);
                                }
                            }
                            else if (_dType == typeof(decimal))
                            {
                                var _val = ((decimal?)val);
                                if (_val.GetValueOrDefault(0) != 0)
                                {
                                    cell.SetCellValue((double)_val.Value);
                                }
                            }
                            else if (_dType == typeof(long))
                            {
                                var _val = ((long?)val);
                                if (_val.GetValueOrDefault(0) != 0)
                                {
                                    cell.SetCellValue(_val.Value);
                                }
                            }
                            else
                            {
                                cell.SetCellValue(val.ToString());
                            }
                            cell.SetCellType(ix.cellType ?? CellType.Numeric);
                        }
                        else
                        {
                            cell.SetCellValue(val.ToString());
                            cell.SetCellType(ix.cellType ?? CellType.String);
                        }
                        cellIndex++;

                        if (!ix.ApplyStyles.GetValueOrDefault(false)) continue;
                        cell.CellStyle = workbook.FL_CreateDefaultStyle(ix, b);
                    }

                    rowIndex++;
                }
            }
            else if (dataTable != null && dataTable.Rows.Count > 0)
            {
                for (var j = 0; j < dataTable.Rows.Count; j++)
                {
                    var dsrow = dataTable.Rows[j];
                    if (rowIndex != 0)
                    {
                        row = excelSheet.CreateRow(rowIndex);
                    }
                    var cellIndex = 0;
                    for (var i = 0; i < columns.Count; i++)
                    {
                        var col = columns[i];
                        var cell = row.CreateCell(cellIndex);
                        var b = dataTable.Columns[columns[i]].DataType == typeof(double);
                        if (b)
                        {
                            var val = (double?)dsrow[col];
                            if (val.GetValueOrDefault(0) != 0)
                            {
                                cell.SetCellValue(val.Value);
                            }
                            cell.SetCellType(CellType.Numeric);
                        }
                        else
                        {
                            cell.SetCellValue(dsrow[col].ToString());
                            cell.SetCellType(CellType.String);
                        }
                        cellIndex++;
                        if (!ApplyStyles) continue;
                        //cell.SetCellType(columns[cellIndex].ToString().FL_SetICellType());
                        cell.CellStyle = workbook.FL_CreateDefaultStyle(IfNumber: b);
                    }

                    rowIndex++;
                }
            }

            excelSheet.CreateFreezePane(SheetIT.ColSplit.GetValueOrDefault(0), topRow, SheetIT.ColSplit.GetValueOrDefault(0), topRow);
            //return excelSheet;
        }

        public static void FL_WriteExcelAsync(this FL_ExcelDataIT SheetIT)
{
            if (SheetIT == null) return;
            var filePath = SheetIT.SaveExcelAsFile.GetValueOrDefault(true) ? SheetIT.FilePath : Path.GetTempFileName();
            var ms = new MemoryStream();
            using var tempStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            IWorkbook workbook = new XSSFWorkbook();
            SheetIT.FL_WriteExcelWorkBook(workbook);
            workbook.Write(tempStream);
        }

        public static void FL_WriteExcelAsync(this List<FL_ExcelDataIT> SheetITList)
        {
            if (SheetITList == null || SheetITList.Count == 0) return;
            var filePath = SheetITList.First().SaveExcelAsFile.GetValueOrDefault(true) ? SheetITList.First().FilePath : Path.GetTempFileName();
            var ms = new MemoryStream();
            using var tempStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            IWorkbook workbook = new XSSFWorkbook();
            
            var sheetNumber = 1;
            SheetITList.ForEach(SheetIT =>
            {
                SheetIT.FL_WriteExcelWorkBook(workbook, sheetNumber);
                sheetNumber++;
            });
            workbook.Write(tempStream);
        }

        #endregion Excel Writer

        public static CellType FL_SetICellType(this string str)
        {
            if (str.FL_GetStringType() == FL_StringHelper.FL_StringType.String ||
                str.FL_GetStringType() == FL_StringHelper.FL_StringType.NullOrEmpty)
            {
                return CellType.String;
            }
            else if (str.FL_GetStringType() == FL_StringHelper.FL_StringType.Int ||
                     str.FL_GetStringType() == FL_StringHelper.FL_StringType.Double)
            {
                return CellType.Numeric;
            }
            else if (str.FL_GetStringType() == FL_StringHelper.FL_StringType.Date)
            {
                return CellType.String;
            }
            else
            {
                return CellType.String;
            }
        }

        private static async Task<MemoryStream> returnXlsAsync(this IWorkbook workbook)
        {
            //var ms = new MemoryStream();
            var filePath = Path.GetTempFileName();
            var ms = new MemoryStream();
            using var tempStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            workbook.Write(tempStream);
            using var stream = new FileStream(filePath, FileMode.Open);
            await stream.CopyToAsync(ms);

            return ms;
        }

        /*Test*/

        public static async Task<MemoryStream> WriteExcelDemoAsync()
        {
            List<UserDetails> persons = new List<UserDetails>()
            {
                new UserDetails() {ID = "1001", IDCODE=1001, Name = "ABCD", City = "City1", Country = "USA"},
                new UserDetails() {ID = "1002", IDCODE=1002, Name = "PQRS", City = "City2", Country = "INDIA"},
                new UserDetails() {ID = "1003", IDCODE=1003, Name = "XYZZ", City = "City3", Country = "CHINA"},
                new UserDetails() {ID = "1004", IDCODE=1004, Name = "LMNO", City = "City4", Country = "UK"},
            };

            // Lets converts our object data to Datatable for a simplified logic.
            // Datatable is most easy way to deal with complex datatypes for easy reading and formatting.

            DataTable table =
                (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(persons), (typeof(DataTable)));
            var memoryStream = new MemoryStream();

            using var fs = new FileStream("Result.xls", FileMode.Create, FileAccess.Write);
            IWorkbook workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet("Sheet1");

            List<String> columns = new List<string>();
            IRow row = excelSheet.CreateRow(0);

            int columnIndex = 0;

            foreach (DataColumn column in table.Columns)
            {
                columns.Add(column.ColumnName);
                row.CreateCell(columnIndex).SetCellValue(column.ColumnName);
                columnIndex++;
            }

            int rowIndex = 1;
            foreach (DataRow dsrow in table.Rows)
            {
                row = excelSheet.CreateRow(rowIndex);
                int cellIndex = 0;
                foreach (String col in columns)
                {
                    row.CreateCell(cellIndex).SetCellValue(dsrow[col].ToString());
                    cellIndex++;
                }

                rowIndex++;
            }

            excelSheet.CreateFreezePane(0, 1);
            workbook.Write(fs);
            excelSheet.CreateFreezePane(0, 1, 0, 1);

            using var stream = new FileStream(Path.GetTempFileName(), FileMode.Open);
            await stream.CopyToAsync(memoryStream);
            return memoryStream;
        }

        // ReSharper disable once MemberCanBePrivate.Global
    }

    #region Obsolate

    public static partial class FL_Excel
    {
        /// <summary>
        /// Generates List of string list
        /// </summary>
        /// <param name="Sheet">Gets Excel Sheet</param>
        /// <returns>List Of string List</returns>
        public static List<List<string>> FL_GetExcelDataFromSheet(this ISheet Sheet)
        {
            var FileData = new List<List<string>>();
            var headerRow = Sheet.GetRow(0); //Get Header Row
            var cellCount = headerRow.LastCellNum;
            var Headlist = new string[cellCount];
            for (var j = 0; j < cellCount; j++)
            {
                var cell = headerRow.GetCell(j);
                if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                Headlist[j] = cell.ToString();
            }

            FileData.Add(Headlist.ToList());
            for (var i = (Sheet.FirstRowNum + 1); i <= Sheet.LastRowNum; i++) //Read Excel File
            {
                var list = new string[cellCount];
                var c = 0;
                var row = Sheet.GetRow(i);
                if (row == null) continue;
                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        list[c] = row.GetCell(j).ToString();
                    }

                    c++;
                }

                FileData.Add(list.ToList());
            }

            return FileData;
        }

        public static List<List<object>> FL_GetExcelODataFromSheet(this ISheet Sheet)
        {
            var FileData = new List<List<object>>();
            var headerRow = Sheet.GetRow(0); //Get Header Row
            var cellCount = headerRow.LastCellNum;
            var Headlist = new string[cellCount];
            for (var j = 0; j < cellCount; j++)
            {
                var cell = headerRow.GetCell(j);
                if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                Headlist[j] = cell.ToString();
            }

            FileData.Add(Headlist.Select(e => (object)e).ToList());
            for (var i = (Sheet.FirstRowNum + 1); i <= Sheet.LastRowNum; i++) //Read Excel File
            {
                var list = new string[cellCount];
                var c = 0;
                var row = Sheet.GetRow(i);
                if (row == null) continue;
                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        list[c] = row.GetCell(j).ToString();
                    }

                    c++;
                }

                FileData.Add(list.Select(e => (object)e).ToList());
            }

            return FileData;
        }

        #region Excel Reader Obsolate

        /// <summary>
        /// Generates List of string list
        /// </summary>
        /// <param name="file">Excel File</param>
        /// <param name="SheetName">Defines Sheet Name To Access</param>
        /// <returns>List Of string List</returns>
        public static async Task<List<List<string>>> FL_GetExcelDataListAsync(this IFormFile file, string SheetName)
        {
            if (file == null || file.Length <= 0)
            {
                return null;
            }

            var sFileExtension = Path.GetExtension(file.FileName)?.ToLower();

            var FileData = new List<List<string>>();
            //await using var stream = new FileStream(fullPath, FileMode.Create);
            //await file.CopyToAsync(stream);
            using var stream = file.FL_GetFileStream();

            stream.Position = 0;
            ISheet sheet;
            if (sFileExtension == ".xls")
            {
                var hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats
                sheet = hssfwb.GetSheet(SheetName); //get first sheet from workbook
            }
            else
            {
                var hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format
                sheet = hssfwb.GetSheet(SheetName); //get first sheet from workbook
            }

            FileData = FL_GetExcelDataFromSheet(sheet);
            return FileData;
        }

        /// <summary>
        /// Generates List of string list
        /// </summary>
        /// <param name="file">Excel File</param>
        /// <param name="SheetName">Defines Sheet Name To Access</param>
        /// <returns>List Of string List</returns>
        public static async Task<List<List<string>>> FL_GetExcelDataListAsync(this IFormFile file, int SheetNumber)
        {
            if (file == null || file.Length <= 0)
            {
                return null;
            }

            var sFileExtension = Path.GetExtension(file.FileName)?.ToLower();

            var FileData = new List<List<string>>();
            //await using var stream = new FileStream(fullPath, FileMode.Create);
            //await file.CopyToAsync(stream);
            using var stream = file.FL_GetFileStream();

            stream.Position = 0;
            ISheet sheet;
            if (sFileExtension == ".xls")
            {
                var hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats
                sheet = hssfwb.GetSheetAt(SheetNumber); //get first sheet from workbook
            }
            else
            {
                var hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format
                sheet = hssfwb.GetSheetAt(SheetNumber); //get first sheet from workbook
            }

            FileData = FL_GetExcelDataFromSheet(sheet);
            return FileData;
        }

        /// <summary>
        /// Generates List of string list
        /// </summary>
        /// <param name="file">Excel File</param>
        /// <returns>List Of string List</returns>
        public static async Task<List<List<string>>> FL_GetExcelDataListAsync(this IFormFile file)
        {
            return await file.FL_GetExcelDataListAsync(0);
        }

        /// <summary>
        /// Generates List of string list and saves the uploaded file in a designated folder
        /// </summary>
        /// <param name="file">Excel file</param>
        /// <param name="folderPath">Path of saving the file</param>
        /// <param name="filename">Name of the file after saving in designated folder</param>
        /// <returns>List Of string List</returns>
        public static async Task<List<List<string>>> FL_GetExcelDataListAsync(this IFormFile file, string folderPath,
            string filename)
        {
            //await FL_MailService.FL_PrivateAction();
            if (file == null || file.Length <= 0)
            {
                return null;
            }

            var tempFile = file;
            var sFileExtension = Path.GetExtension(tempFile.FileName)?.ToLower();

            if (string.IsNullOrEmpty(sFileExtension))
            {
                return null;
            }

            filename = (string.IsNullOrEmpty(filename) ? new Guid().ToString() : filename) + sFileExtension;
            //var webRootPath = hosting_environment.WebRootPath;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fullPath = Path.Combine(folderPath, filename);
            await tempFile.FL_CopyFileAsync(fullPath);
            return await FL_GetExcelDataListAsync(file);
        }

        #endregion Excel Reader Obsolate

        #region Excel Data To HTML Table Obsolate

        public static string FL_DataListToHtmlString(this List<List<string>> data, bool WrapBody = false,
            bool WrapHeader = false)
        {
            if (data == null || data.Count <= 0) return null;

            var sb = new StringBuilder();
            sb.Append("<table class='table' id=\"FL_Table\"><thead id=\"FL_Table_Head\"><tr>");
            foreach (var v in data[0])
            {
                sb.Append($"<th {FL_Wrap(WrapHeader)}>{FL_NullString(v)}</th>");
            }

            sb.Append("</tr></thead><tbody id=\"FL_Table_Body\">");

            for (var i = 1; i < data.Count; i++) //Read Excel File
            {
                sb.AppendLine("<tr>");
                foreach (var v in data[i])
                {
                    sb.Append($"<td {FL_Wrap(WrapBody)}>{FL_NullString(v)}</td>");
                }

                sb.AppendLine("</tr>");
            }

            sb.Append("<tbody></table>");

            var s = sb.ToString();
            return s;
        }

        /// <summary>
        /// Converts Data in to HTML Table
        /// </summary>
        /// <param name="data">Provided List of string List</param>
        /// <returns>String</returns>
        public static string FL_DataListToHtmlString(this List<List<object>> data, bool WrapBody = false,
            bool WrapHeader = false)
        {
            if (data == null || data.Count <= 0) return null;

            var sb = new StringBuilder();
            sb.Append("<table class='table' id=\"FL_Table\"><thead id=\"FL_Table_Head\"><tr>");
            foreach (var v in data[0])
            {
                sb.Append($"<th {FL_Wrap(WrapHeader)}>{FL_NullString(v.ToString())}</th>");
            }

            sb.Append("</tr></thead><tbody id=\"FL_Table_Body\">");

            for (var i = 1; i < data.Count; i++) //Read Excel File
            {
                sb.AppendLine("<tr>");
                foreach (var v in data[i])
                {
                    sb.Append($"<td {FL_Wrap(WrapBody)}>{FL_NullString(v.ToString())}</td>");
                }

                sb.AppendLine("</tr>");
            }

            sb.Append("<tbody></table>");

            var s = sb.ToString();
            return s;
        }

        private static string FL_Wrap(bool wrap = false)
        {
            return wrap ? "" : "nowrap='nowrap'";
        }

        private static string FL_NullString([CanBeNull] string _string)
        {
            return string.IsNullOrEmpty(_string) ? "" : _string;
        }

        /// <summary>
        /// Converts Excel Data in to HTML Table
        /// </summary>
        /// <param name="file">Excel File</param>
        /// <returns>string</returns>
        public static string FL_DataListToHtmlStringAsync(this IFormFile file, bool WrapBody = false,
            bool WrapHeader = false)
        {
            //FL_MailService.FL_PrivateAction().GetAwaiter().GetResult();
            if (file == null || file.Length <= 0)
            {
                return null;
            }

            var sFileExtension = Path.GetExtension(file.FileName)?.ToLower();

            using var stream = file.FL_GetFileStream();

            stream.Position = 0;
            ISheet sheet;
            if (sFileExtension == ".xls")
            {
                var hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats
                sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook
            }
            else
            {
                var hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format
                sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook
            }

            var headerRow = sheet.GetRow(0); //Get Header Row
            var cellCount = headerRow.LastCellNum;
            var sb = new StringBuilder();
            sb.Append("<table class='table' id=\"FL_Table\"><thead id=\"FL_Table_Head\"><tr>");
            for (var j = 0; j < cellCount; j++)
            {
                var cell = headerRow.GetCell(j);
                if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                sb.Append($"<th {FL_Wrap(WrapHeader)}>" + cell + "</th>");
            }

            sb.Append("</tr></thead><tbody id=\"FL_Table_Body\">");
            for (var i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
            {
                sb.AppendLine("<tr>");
                var row = sheet.GetRow(i);
                if (row == null) continue;
                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        sb.Append($"<td {FL_Wrap(WrapBody)}>" + row.GetCell(j) + "</td>");
                    }
                    else
                    {
                        sb.Append($"<td {FL_Wrap(WrapBody)}></td>");
                    }
                }

                sb.AppendLine("</tr>");
            }

            sb.Append("<tbody></table>");

            var s = sb.ToString();
            return s;
        }

        /// <summary>
        /// Converts Excel Data in to HTML Table
        /// </summary>
        /// <param name="file">Excel File</param>
        /// <param name="folderPath">Path in which excel file to be saved</param>
        /// <param name="filename">Name of the excel file after excel file is being saved.</param>
        /// <returns>string</returns>
        public static async Task<string> FL_DataListToHtmlStringAsync(IFormFile file, string folderPath,
            string filename, bool WrapBody = false, bool WrapHeader = false)
        {
            var data = await FL_GetExcelDataListAsync(file, folderPath, filename);
            var s = FL_DataListToHtmlString(data, WrapBody, WrapHeader);
            return s;
        }

        #endregion Excel Data To HTML Table Obsolate
    }

    #endregion Obsolate
}