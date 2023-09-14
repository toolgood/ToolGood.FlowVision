using NPOI.HSSF.UserModel;
using NPOI.SS.Util;

namespace NPOI.SS.UserModel
{
	public static class ExcelExtensions
	{
		/// <summary>
		/// 合并行
		/// </summary>
		/// <param name="sheet"></param>
		/// <param name="row"></param>
		/// <param name="col"></param>
		/// <param name="count"></param>
		public static void MergedRow(this ISheet sheet, int row, int col, int count)
		{
			sheet.AddMergedRegion(new CellRangeAddress(row, row + count - 1, col, col));
		}

		/// <summary>
		/// 合并行
		/// </summary>
		/// <param name="row"></param>
		/// <param name="col"></param>
		/// <param name="count"></param>
		public static void MergedRow(this IRow row, int col, int count)
		{
			row.Sheet.AddMergedRegion(new CellRangeAddress(row.RowNum, row.RowNum + count - 1, col, col));
		}

		/// <summary>
		/// 合并列
		/// </summary>
		/// <param name="sheet"></param>
		/// <param name="row"></param>
		/// <param name="col"></param>
		/// <param name="count"></param>
		public static void MergedColumn(this ISheet sheet, int row, int col, int count)
		{
			sheet.AddMergedRegion(new CellRangeAddress(row, row, col, col + count - 1));
		}

		/// <summary>
		/// 合并列
		/// </summary>
		/// <param name="sheet"></param>
		/// <param name="row"></param>
		/// <param name="col"></param>
		/// <param name="count"></param>
		public static void MergedColumn(this IRow row, int col, int count)
		{
			row.Sheet.AddMergedRegion(new CellRangeAddress(row.RowNum, row.RowNum, col, col + count - 1));
		}

		/// <summary>
		/// 冻结首行
		/// </summary>
		/// <param name="sheet"></param>
		public static void FreezeFirstRow(this ISheet sheet)
		{
			sheet.CreateFreezePane(0, 1, 0, 1);
		}

		/// <summary>
		/// 冻结首列
		/// </summary>
		/// <param name="sheet"></param>
		public static void FreezeFirstColumn(this ISheet sheet)
		{
			sheet.CreateFreezePane(1, 0, 1, 0);
		}

		/// <summary>
		/// 冻结行列
		/// </summary>
		/// <param name="sheet"></param>
		/// <param name="row"></param>
		/// <param name="col"></param>
		public static void Freeze(this ISheet sheet, int row, int col)
		{
			sheet.CreateFreezePane(col, row, col, row);
		}

		#region 创建格式

		/// <summary>
		/// 创建居中格式
		/// </summary>
		/// <param name="workbook"></param>
		/// <param name="doublePoint"></param>
		/// <returns></returns>
		public static ICellStyle CreateCenterCellStyle(this IWorkbook workbook)
		{
			var style = workbook.CreateCellStyle();
			style.VerticalAlignment = VerticalAlignment.Center;
			style.Alignment = HorizontalAlignment.Center;
			return style;
		}

		/// <summary>
		/// 创建数字格式
		/// </summary>
		/// <param name="sheet"></param>
		/// <param name="doublePoint"></param>
		/// <returns></returns>
		public static ICellStyle CreateCenterCellStyle(this ISheet sheet)
		{
			var style = sheet.Workbook.CreateCellStyle();
			style.VerticalAlignment = VerticalAlignment.Center;
			style.Alignment = HorizontalAlignment.Center;
			return style;
		}

		/// <summary>
		/// 创建数字格式
		/// </summary>
		/// <param name="workbook"></param>
		/// <param name="doublePoint"></param>
		/// <returns></returns>
		public static ICellStyle CreateDoubleCellStyle(this IWorkbook workbook, int doublePoint = 2)
		{
			var style = workbook.CreateCellStyle();
			style.DataFormat = HSSFDataFormat.GetBuiltinFormat("0." + new string('0', doublePoint));
			return style;
		}

		/// <summary>
		/// 创建数字格式
		/// </summary>
		/// <param name="sheet"></param>
		/// <param name="doublePoint"></param>
		/// <returns></returns>
		public static ICellStyle CreateDoubleCellStyle(this ISheet sheet, int doublePoint = 2)
		{
			var style = sheet.Workbook.CreateCellStyle();
			style.DataFormat = HSSFDataFormat.GetBuiltinFormat("0." + new string('0', doublePoint));
			return style;
		}

		/// <summary>
		/// 创建百分数格式
		/// </summary>
		/// <param name="workbook"></param>
		/// <param name="doublePoint"></param>
		/// <returns></returns>
		public static ICellStyle CreatePercentageCellStyle(this IWorkbook workbook, int doublePoint = 2)
		{
			var style = workbook.CreateCellStyle();
			style.DataFormat = HSSFDataFormat.GetBuiltinFormat("0." + new string('0', doublePoint) + "%");
			return style;
		}

		/// <summary>
		/// 创建百分数格式
		/// </summary>
		/// <param name="sheet"></param>
		/// <param name="doublePoint"></param>
		/// <returns></returns>
		public static ICellStyle CreatePercentageCellStyle(this ISheet sheet, int doublePoint = 2)
		{
			var style = sheet.Workbook.CreateCellStyle();
			style.DataFormat = HSSFDataFormat.GetBuiltinFormat("0." + new string('0', doublePoint) + "%");
			return style;
		}

		/// <summary>
		/// 创建日期时间格式
		/// </summary>
		/// <param name="workbook"></param>
		/// <param name="dataFormat"></param>
		/// <returns></returns>
		public static ICellStyle CreateDateCellStyle(this IWorkbook workbook, string dateFormat = "yyyy-MM-dd")
		{
			var style = workbook.CreateCellStyle();
			IDataFormat datastyle = workbook.CreateDataFormat();
			style.DataFormat = datastyle.GetFormat(dateFormat);
			return style;
		}

		/// <summary>
		/// 创建日期时间格式
		/// </summary>
		/// <param name="workbook"></param>
		/// <param name="dataFormat"></param>
		/// <returns></returns>
		public static ICellStyle CreateDateCellStyle(this ISheet sheet, string dateFormat = "yyyy-MM-dd")
		{
			var style = sheet.Workbook.CreateCellStyle();
			IDataFormat datastyle = sheet.Workbook.CreateDataFormat();
			style.DataFormat = datastyle.GetFormat(dateFormat);
			return style;
		}

		/// <summary>
		/// 创建日期时间格式
		/// </summary>
		/// <param name="workbook"></param>
		/// <param name="dataFormat"></param>
		/// <returns></returns>
		public static ICellStyle CreateDateTimeCellStyle(this IWorkbook workbook, string dateFormat = "yyyy-MM-dd hh:mm:ss")
		{
			var style = workbook.CreateCellStyle();
			IDataFormat datastyle = workbook.CreateDataFormat();
			style.DataFormat = datastyle.GetFormat(dateFormat);
			return style;
		}

		/// <summary>
		/// 创建日期时间格式
		/// </summary>
		/// <param name="workbook"></param>
		/// <param name="dataFormat"></param>
		/// <returns></returns>
		public static ICellStyle CreateDateTimeCellStyle(this ISheet sheet, string dateFormat = "yyyy-MM-dd hh:mm:ss")
		{
			var style = sheet.Workbook.CreateCellStyle();
			IDataFormat datastyle = sheet.Workbook.CreateDataFormat();
			style.DataFormat = datastyle.GetFormat(dateFormat);
			return style;
		}

		/// <summary>
		/// 创建时间格式
		/// </summary>
		/// <param name="workbook"></param>
		/// <param name="dataFormat"></param>
		/// <returns></returns>
		public static ICellStyle CreateTimeCellStyle(this IWorkbook workbook, string dateFormat = "hh:mm:ss")
		{
			var style = workbook.CreateCellStyle();
			IDataFormat datastyle = workbook.CreateDataFormat();
			style.DataFormat = datastyle.GetFormat(dateFormat);
			return style;
		}

		/// <summary>
		/// 创建时间格式
		/// </summary>
		/// <param name="workbook"></param>
		/// <param name="dataFormat"></param>
		/// <returns></returns>
		public static ICellStyle CreateTimeCellStyle(this ISheet sheet, string dateFormat = "hh:mm:ss")
		{
			var style = sheet.Workbook.CreateCellStyle();
			IDataFormat datastyle = sheet.Workbook.CreateDataFormat();
			style.DataFormat = datastyle.GetFormat(dateFormat);
			return style;
		}

		/// <summary>
		/// 创建千分位格式
		/// </summary>
		/// <param name="workbook"></param>
		/// <param name="doublePoint"></param>
		/// <returns></returns>
		public static ICellStyle CreateThousandBitCellStyle(this IWorkbook workbook, int doublePoint = 2)
		{
			var style = workbook.CreateCellStyle();
			style.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0." + new string('0', doublePoint));
			return style;
		}

		/// <summary>
		/// 创建千分位格式
		/// </summary>
		/// <param name="workbook"></param>
		/// <param name="doublePoint"></param>
		/// <returns></returns>
		public static ICellStyle CreateThousandBitCellStyle(this ISheet sheet, int doublePoint = 2)
		{
			var style = sheet.Workbook.CreateCellStyle();
			style.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0." + new string('0', doublePoint));
			return style;
		}

		#endregion 创建格式

		#region WriteDouble

		/// <summary>
		/// 写入数字
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="doublePoint"></param>
		public static void WriteDouble(this IRow row, int index, double value, int doublePoint = 2)
		{
			var style = row.Sheet.Workbook.CreateCellStyle();
			style.DataFormat = HSSFDataFormat.GetBuiltinFormat("0." + new string('0', doublePoint));

			var cell0 = row.GetOrCreateCell(index);
			cell0.CellStyle = style;
			cell0.SetCellValue(value);

			row.Sheet.SetColumnWidth2(index, value.ToString("F" + doublePoint).Length);
		}

		/// <summary>
		/// 写入数字
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="doublePoint"></param>
		public static void WriteDouble(this IRow row, int index, double? value, int doublePoint = 2)
		{
			if (value != null) {
				WriteDouble(row, index, value.Value, doublePoint);
			}
		}

		/// <summary>
		/// 写入数字
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="doublePoint"></param>
		public static void WriteDouble(this IRow row, int index, decimal value, int doublePoint = 2)
		{
			var style = row.Sheet.Workbook.CreateCellStyle();
			style.DataFormat = HSSFDataFormat.GetBuiltinFormat("0." + new string('0', doublePoint));

			var cell0 = row.GetOrCreateCell(index);
			cell0.CellStyle = style;
			cell0.SetCellValue((double)value);

			row.Sheet.SetColumnWidth2(index, value.ToString("F" + doublePoint).Length);

			//var width = row.Sheet.GetColumnWidth(index);
			//var text = value.ToString("F" + doublePoint);
			//if (width < (text.Length + 10) * 256) {
			//    row.Sheet.SetColumnWidth2(index, (text.Length + 10) * 256);
			//}
		}

		/// <summary>
		/// 写入数字
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="doublePoint"></param>
		public static void WriteDouble(this IRow row, int index, decimal? value, int doublePoint = 2)
		{
			if (value != null) {
				WriteDouble(row, index, value.Value, doublePoint);
			}
		}

		/// <summary>
		/// 写入数字
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public static void WriteDouble(this IRow row, int index, int value)
		{
			var cell0 = row.GetOrCreateCell(index);
			cell0.SetCellValue(value);

			row.Sheet.SetColumnWidth2(index, value.ToString().Length);
		}

		/// <summary>
		/// 写入数字
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public static void WriteDouble(this IRow row, int index, int? value)
		{
			if (value != null) {
				WriteDouble(row, index, value.Value);
			}
		}

		/// <summary>
		/// 写入数字
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public static void WriteDouble(this IRow row, int index, long value)
		{
			var cell0 = row.GetOrCreateCell(index);
			cell0.SetCellValue(value);

			row.Sheet.SetColumnWidth2(index, value.ToString().Length);
		}

		/// <summary>
		/// 写入数字
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public static void WriteDouble(this IRow row, int index, long? value)
		{
			if (value != null) {
				WriteDouble(row, index, value.Value);
			}
		}

		/// <summary>
		/// 写入数字
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="doublePoint"></param>
		public static void WriteDouble(this IRow row, int index, double value, ICellStyle style)
		{
			var cell0 = row.GetOrCreateCell(index);
			cell0.CellStyle = style;
			cell0.SetCellValue(value);

			row.Sheet.SetColumnWidth2(index, value.ToString().Length);
		}

		/// <summary>
		/// 写入数字
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="doublePoint"></param>
		public static void WriteDouble(this IRow row, int index, double? value, ICellStyle style)
		{
			if (value != null) {
				WriteDouble(row, index, value.Value, style);
			}
		}

		/// <summary>
		/// 写入数字
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="doublePoint"></param>
		public static void WriteDouble(this IRow row, int index, decimal value, ICellStyle style)
		{
			var cell0 = row.GetOrCreateCell(index);
			cell0.CellStyle = style;
			cell0.SetCellValue((double)value);

			row.Sheet.SetColumnWidth2(index, value.ToString().Length);
		}

		/// <summary>
		/// 写入数字
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="doublePoint"></param>
		public static void WriteDouble(this IRow row, int index, decimal? value, ICellStyle style)
		{
			if (value != null) {
				WriteDouble(row, index, value.Value, style);
			}
		}

		/// <summary>
		/// 写入数字
		/// </summary>
		/// <param name="sheet"></param>
		/// <param name="row"></param>
		/// <param name="col"></param>
		/// <param name="value"></param>
		/// <param name="doublePoint"></param>
		public static void WriteDouble(this ISheet sheet, int row, int index, double value, int doublePoint = 2)
		{
			var r = sheet.GetOrCreateRow(row);
			WriteDouble(r, index, value, doublePoint);
		}

		/// <summary>
		/// 写入数字
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="doublePoint"></param>
		public static void WriteDouble(this ISheet sheet, int row, int index, double? value, int doublePoint = 2)
		{
			if (value != null) {
				WriteDouble(sheet, row, index, value.Value, doublePoint);
			}
		}

		/// <summary>
		/// 写入数字
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="doublePoint"></param>
		public static void WriteDouble(this ISheet sheet, int row, int index, decimal value, int doublePoint = 2)
		{
			var r = sheet.GetOrCreateRow(row);
			WriteDouble(r, index, value, doublePoint);
		}

		/// <summary>
		/// 写入数字
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="doublePoint"></param>
		public static void WriteDouble(this ISheet sheet, int row, int index, decimal? value, int doublePoint = 2)
		{
			if (value != null) {
				WriteDouble(sheet, row, index, value.Value, doublePoint);
			}
		}

		/// <summary>
		/// 写入数字
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public static void WriteDouble(this ISheet sheet, int row, int index, int value)
		{
			var r = sheet.GetOrCreateRow(row);
			WriteDouble(r, index, value);
		}

		/// <summary>
		/// 写入数字
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public static void WriteDouble(this ISheet sheet, int row, int index, int? value)
		{
			if (value != null) {
				WriteDouble(sheet, row, index, value.Value);
			}
		}

		/// <summary>
		/// 写入数字
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public static void WriteDouble(this ISheet sheet, int row, int index, long value)
		{
			var r = sheet.GetOrCreateRow(row);
			WriteDouble(r, index, value);
		}

		/// <summary>
		/// 写入数字
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public static void WriteDouble(this ISheet sheet, int row, int index, long? value)
		{
			if (value != null) {
				WriteDouble(sheet, row, index, value.Value);
			}
		}

		/// <summary>
		/// 写入数字
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="doublePoint"></param>
		public static void WriteDouble(this ISheet sheet, int row, int index, double value, ICellStyle style)
		{
			var r = sheet.GetOrCreateRow(row);
			WriteDouble(r, index, value, style);
		}

		/// <summary>
		/// 写入数字
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="doublePoint"></param>
		public static void WriteDouble(this ISheet sheet, int row, int index, double? value, ICellStyle style)
		{
			if (value != null) {
				WriteDouble(sheet, row, index, value.Value, style);
			}
		}

		/// <summary>
		/// 写入数字
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="doublePoint"></param>
		public static void WriteDouble(this ISheet sheet, int row, int index, decimal value, ICellStyle style)
		{
			var r = sheet.GetOrCreateRow(row);
			WriteDouble(r, index, value, style);
		}

		/// <summary>
		/// 写入数字
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="doublePoint"></param>
		public static void WriteDouble(this ISheet sheet, int row, int index, decimal? value, ICellStyle style)
		{
			if (value != null) {
				WriteDouble(sheet, row, index, value.Value, style);
			}
		}

		#endregion WriteDouble

		#region WritePercentage

		/// <summary>
		/// 写入百分数
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="doublePoint">转进百分数后，显示的小数位数</param>
		public static void WritePercentage(this IRow row, int index, double value, int doublePoint = 2)
		{
			var style = row.Sheet.Workbook.CreateCellStyle();
			style.DataFormat = HSSFDataFormat.GetBuiltinFormat("0." + new string('0', doublePoint) + "%");

			var cell0 = row.GetOrCreateCell(index);
			cell0.CellStyle = style;
			cell0.SetCellValue(value);

			row.Sheet.SetColumnWidth2(index, (value * 100).ToString("F" + doublePoint).Length + 1);
		}

		/// <summary>
		/// 写入百分数
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="doublePoint">转进百分数后，显示的小数位数</param>
		public static void WritePercentage(this IRow row, int index, double? value, int doublePoint = 2)
		{
			if (value != null) {
				WritePercentage(row, index, value.Value, doublePoint);
			}
		}

		/// <summary>
		/// 写入百分数
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="doublePoint">转进百分数后，显示的小数位数</param>
		public static void WritePercentage(this IRow row, int index, decimal value, int doublePoint = 2)
		{
			var style = row.Sheet.Workbook.CreateCellStyle();
			style.DataFormat = HSSFDataFormat.GetBuiltinFormat("0." + new string('0', doublePoint) + "%");

			var cell0 = row.GetOrCreateCell(index);
			cell0.CellStyle = style;
			cell0.SetCellValue((double)value);

			row.Sheet.SetColumnWidth2(index, (value * 100).ToString("F" + doublePoint).Length + 1);
		}

		/// <summary>
		/// 写入百分数
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="doublePoint">转进百分数后，显示的小数位数</param>
		public static void WritePercentage(this IRow row, int index, decimal? value, int doublePoint = 2)
		{
			if (value != null) {
				WritePercentage(row, index, value.Value, doublePoint);
			}
		}

		/// <summary>
		/// 写入百分数
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public static void WritePercentage(this IRow row, int index, double value, ICellStyle style)
		{
			var cell0 = row.GetOrCreateCell(index);
			cell0.CellStyle = style;
			cell0.SetCellValue(value);

			row.Sheet.SetColumnWidth2(index, value.ToString().Length + 1);
		}

		/// <summary>
		/// 写入百分数
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public static void WritePercentage(this IRow row, int index, double? value, ICellStyle style)
		{
			if (value != null) {
				WritePercentage(row, index, value.Value, style);
			}
		}

		/// <summary>
		/// 写入百分数
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public static void WritePercentage(this IRow row, int index, decimal value, ICellStyle style)
		{
			var cell0 = row.GetOrCreateCell(index);
			cell0.CellStyle = style;
			cell0.SetCellValue((double)value);

			row.Sheet.SetColumnWidth2(index, value.ToString().Length + 1);
		}

		/// <summary>
		/// 写入百分数
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public static void WritePercentage(this IRow row, int index, decimal? value, ICellStyle style)
		{
			if (value != null) {
				WritePercentage(row, index, value.Value, style);
			}
		}

		/// <summary>
		/// 写入百分数
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="doublePoint">转进百分数后，显示的小数位数</param>
		public static void WritePercentage(this ISheet sheet, int row, int index, double value, int doublePoint = 2)
		{
			var r = sheet.GetOrCreateRow(row);
			WritePercentage(r, index, value, doublePoint);
		}

		/// <summary>
		/// 写入百分数
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="doublePoint">转进百分数后，显示的小数位数</param>
		public static void WritePercentage(this ISheet sheet, int row, int index, double? value, int doublePoint = 2)
		{
			if (value != null) {
				WritePercentage(sheet, row, index, value.Value, doublePoint);
			}
		}

		/// <summary>
		/// 写入百分数
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="doublePoint">转进百分数后，显示的小数位数</param>
		public static void WritePercentage(this ISheet sheet, int row, int index, decimal value, int doublePoint = 2)
		{
			var r = sheet.GetOrCreateRow(row);
			WritePercentage(r, index, value, doublePoint);
		}

		/// <summary>
		/// 写入百分数
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="doublePoint">转进百分数后，显示的小数位数</param>
		public static void WritePercentage(this ISheet sheet, int row, int index, decimal? value, int doublePoint = 2)
		{
			if (value != null) {
				WritePercentage(sheet, row, index, value.Value, doublePoint);
			}
		}

		/// <summary>
		/// 写入百分数
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public static void WritePercentage(this ISheet sheet, int row, int index, double value, ICellStyle style)
		{
			var r = sheet.GetOrCreateRow(row);
			WritePercentage(r, index, value, style);
		}

		/// <summary>
		/// 写入百分数
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public static void WritePercentage(this ISheet sheet, int row, int index, double? value, ICellStyle style)
		{
			if (value != null) {
				WritePercentage(sheet, row, index, value.Value, style);
			}
		}

		/// <summary>
		/// 写入百分数
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public static void WritePercentage(this ISheet sheet, int row, int index, decimal value, ICellStyle style)
		{
			var r = sheet.GetOrCreateRow(row);
			WritePercentage(r, index, value, style);
		}

		/// <summary>
		/// 写入百分数
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public static void WritePercentage(this ISheet sheet, int row, int index, decimal? value, ICellStyle style)
		{
			if (value != null) {
				WritePercentage(sheet, row, index, value.Value, style);
			}
		}

		#endregion WritePercentage

		#region WriteDate

		/// <summary>
		/// 写入日期
		/// </summary>
		/// <param name="row"></param>
		public static void WriteDate(this IRow row, int index, DateTime value, string dataFormat = "yyyy-MM-dd")
		{
			var style = row.Sheet.Workbook.CreateCellStyle();
			IDataFormat datastyle = row.Sheet.Workbook.CreateDataFormat();
			style.DataFormat = datastyle.GetFormat(dataFormat);

			var cell0 = row.GetOrCreateCell(index);
			cell0.CellStyle = style;
			cell0.SetCellValue(value);

			row.Sheet.SetColumnWidth2(index, value.ToString().Length);
		}

		/// <summary>
		/// 写入日期
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="dataFormat"></param>
		public static void WriteDate(this IRow row, int index, DateTime? value, string dataFormat = "yyyy-MM-dd")
		{
			if (value != null) {
				WriteDate(row, index, value.Value, dataFormat);
			}
		}

		/// <summary>
		/// 写入日期
		/// </summary>
		/// <param name="row"></param>
		public static void WriteDate(this IRow row, int index, DateTime value, ICellStyle style)
		{
			var cell0 = row.GetOrCreateCell(index);
			cell0.CellStyle = style;
			cell0.SetCellValue(value);

			row.Sheet.SetColumnWidth2(index, value.ToString().Length);
		}

		/// <summary>
		/// 写入日期
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="dataFormat"></param>
		public static void WriteDate(this IRow row, int index, DateTime? value, ICellStyle style)
		{
			if (value != null) {
				WriteDate(row, index, value.Value, style);
			}
		}

		/// <summary>
		/// 写入日期
		/// </summary>
		/// <param name="row"></param>
		public static void WriteDate(this ISheet sheet, int row, int index, DateTime value, string dataFormat = "yyyy-MM-dd")
		{
			var r = sheet.GetOrCreateRow(row);
			WriteDate(r, index, value, dataFormat);
		}

		/// <summary>
		/// 写入日期
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="dataFormat"></param>
		public static void WriteDate(this ISheet sheet, int row, int index, DateTime? value, string dataFormat = "yyyy-MM-dd")
		{
			if (value != null) {
				WriteDate(sheet, row, index, value.Value, dataFormat);
			}
		}

		/// <summary>
		/// 写入日期
		/// </summary>
		/// <param name="row"></param>
		public static void WriteDate(this ISheet sheet, int row, int index, DateTime value, ICellStyle style)
		{
			var r = sheet.GetOrCreateRow(row);
			WriteDate(r, index, value, style);
		}

		/// <summary>
		/// 写入日期
		/// </summary>
		/// <param name="row"></param>
		/// <param name="index"></param>
		/// <param name="value"></param>
		/// <param name="dataFormat"></param>
		public static void WriteDate(this ISheet sheet, int row, int index, DateTime? value, ICellStyle style)
		{
			if (value != null) {
				WriteDate(sheet, row, index, value.Value, style);
			}
		}

		#endregion WriteDate

		#region WriteDateTime

		/// <summary>
		/// 写入日期时间
		/// </summary>
		/// <param name="row"></param>
		public static void WriteDateTime(this IRow row, int index, DateTime value, string dataFormat = "yyyy-MM-dd hh:mm:ss")
		{
			var style = row.Sheet.Workbook.CreateCellStyle();
			IDataFormat datastyle = row.Sheet.Workbook.CreateDataFormat();
			style.DataFormat = datastyle.GetFormat(dataFormat);

			var cell0 = row.GetOrCreateCell(index);
			cell0.CellStyle = style;
			cell0.SetCellValue(value);

			row.Sheet.SetColumnWidth2(index, value.ToString().Length);
		}

		/// <summary>
		/// 写入日期时间
		/// </summary>
		/// <param name="row"></param>
		public static void WriteDateTime(this IRow row, int index, DateTime? value, string dataFormat = "yyyy-MM-dd hh:mm:ss")
		{
			if (value != null) {
				WriteDateTime(row, index, value.Value, dataFormat);
			}
		}

		/// <summary>
		/// 写入日期时间
		/// </summary>
		/// <param name="row"></param>
		public static void WriteDateTime(this IRow row, int index, DateTime value, ICellStyle style)
		{
			var cell0 = row.GetOrCreateCell(index);
			cell0.CellStyle = style;
			cell0.SetCellValue(value);

			row.Sheet.SetColumnWidth2(index, value.ToString().Length);
		}

		/// <summary>
		/// 写入日期时间
		/// </summary>
		/// <param name="row"></param>
		public static void WriteDateTime(this IRow row, int index, DateTime? value, ICellStyle style)
		{
			if (value != null) {
				WriteDateTime(row, index, value.Value, style);
			}
		}

		/// <summary>
		/// 写入日期时间
		/// </summary>
		/// <param name="row"></param>
		public static void WriteDateTime(this ISheet sheet, int row, int index, DateTime value, string dataFormat = "yyyy-MM-dd hh:mm:ss")
		{
			var r = sheet.GetOrCreateRow(row);
			WriteDateTime(r, index, value, dataFormat);
		}

		/// <summary>
		/// 写入日期时间
		/// </summary>
		/// <param name="row"></param>
		public static void WriteDateTime(this ISheet sheet, int row, int index, DateTime? value, string dataFormat = "yyyy-MM-dd hh:mm:ss")
		{
			if (value != null) {
				WriteDateTime(sheet, row, index, value.Value, dataFormat);
			}
		}

		/// <summary>
		/// 写入日期时间
		/// </summary>
		/// <param name="row"></param>
		public static void WriteDateTime(this ISheet sheet, int row, int index, DateTime value, ICellStyle style)
		{
			var r = sheet.GetOrCreateRow(row);
			WriteDateTime(r, index, value, style);
		}

		/// <summary>
		/// 写入日期时间
		/// </summary>
		/// <param name="row"></param>
		public static void WriteDateTime(this ISheet sheet, int row, int index, DateTime? value, ICellStyle style)
		{
			if (value != null) {
				WriteDateTime(sheet, row, index, value.Value, style);
			}
		}

		#endregion WriteDateTime

		#region WriteTime

		/// <summary>
		/// 写入时间
		/// </summary>
		/// <param name="row"></param>
		public static void WriteTime(this IRow row, int index, DateTime value, string dataFormat = "hh:mm:ss")
		{
			var style = row.Sheet.Workbook.CreateCellStyle();
			IDataFormat datastyle = row.Sheet.Workbook.CreateDataFormat();
			style.DataFormat = datastyle.GetFormat(dataFormat);

			var cell0 = row.GetOrCreateCell(index);
			cell0.CellStyle = style;
			cell0.SetCellValue(value);

			row.Sheet.SetColumnWidth2(index, value.ToString().Length);
		}

		/// <summary>
		/// 写入时间
		/// </summary>
		/// <param name="row"></param>
		public static void WriteTime(this IRow row, int index, DateTime? value, string dataFormat = "hh:mm:ss")
		{
			if (value != null) {
				WriteTime(row, index, value.Value, dataFormat);
			}
		}

		/// <summary>
		/// 写入时间
		/// </summary>
		/// <param name="row"></param>
		public static void WriteTime(this IRow row, int index, DateTime value, ICellStyle style)
		{
			var cell0 = row.GetOrCreateCell(index);
			cell0.CellStyle = style;
			cell0.SetCellValue(value);

			row.Sheet.SetColumnWidth2(index, value.ToString().Length);
		}

		/// <summary>
		/// 写入时间
		/// </summary>
		/// <param name="row"></param>
		public static void WriteTime(this IRow row, int index, DateTime? value, ICellStyle style)
		{
			if (value != null) {
				WriteTime(row, index, value.Value, style);
			}
		}

		/// <summary>
		/// 写入时间
		/// </summary>
		/// <param name="row"></param>
		public static void WriteTime(this ISheet sheet, int row, int index, DateTime value, string dataFormat = "hh:mm:ss")
		{
			var r = sheet.GetOrCreateRow(row);
			WriteTime(r, index, value, dataFormat);
		}

		/// <summary>
		/// 写入时间
		/// </summary>
		/// <param name="row"></param>
		public static void WriteTime(this ISheet sheet, int row, int index, DateTime? value, string dataFormat = "hh:mm:ss")
		{
			if (value != null) {
				WriteTime(sheet, row, index, value.Value, dataFormat);
			}
		}

		/// <summary>
		/// 写入时间
		/// </summary>
		/// <param name="row"></param>
		public static void WriteTime(this ISheet sheet, int row, int index, DateTime value, ICellStyle style)
		{
			var r = sheet.GetOrCreateRow(row);
			WriteTime(r, index, value, style);
		}

		/// <summary>
		/// 写入时间
		/// </summary>
		/// <param name="row"></param>
		public static void WriteTime(this ISheet sheet, int row, int index, DateTime? value, ICellStyle style)
		{
			if (value != null) {
				WriteTime(sheet, row, index, value.Value, style);
			}
		}

		#endregion WriteTime

		#region WriteString

		public static void WriteString(this IRow row, int index, string value, ICellStyle style)
		{
			var cell0 = row.GetOrCreateCell(index);
			cell0.CellStyle = style;
			if (value != null) {
				cell0.SetCellValue(value);

				row.Sheet.SetColumnWidth2(index, value.ToString().Length);
			}
		}

		public static void WriteString(this IRow row, int index, string value)
		{
			var cell0 = row.GetOrCreateCell(index);
			if (value != null) {
				cell0.SetCellValue(value);

				row.Sheet.SetColumnWidth2(index, value.ToString().Length);
			}
		}

		public static void WriteString(this IRow row, int index, decimal value)
		{
			var cell0 = row.GetOrCreateCell(index);
			cell0.SetCellValue(value.ToString());

			row.Sheet.SetColumnWidth2(index, value.ToString().Length);
		}

		public static void WriteString(this IRow row, int index, decimal? value)
		{
			var cell0 = row.GetOrCreateCell(index);
			if (value != null) {
				cell0.SetCellValue((value ?? 0).ToString());

				row.Sheet.SetColumnWidth2(index, value?.ToString().Length ?? 0);
			}
		}

		public static void WriteString(this IRow row, int index, int value)
		{
			var cell0 = row.GetOrCreateCell(index);
			cell0.SetCellValue(value.ToString());

			row.Sheet.SetColumnWidth2(index, value.ToString().Length);
		}

		public static void WriteString(this IRow row, int index, int? value)
		{
			var cell0 = row.GetOrCreateCell(index);
			if (value != null) {
				cell0.SetCellValue(value?.ToString() ?? "");
				row.Sheet.SetColumnWidth2(index, value?.ToString().Length ?? 0);
			}
		}

		public static void WriteString(this IRow row, int index, DateTime dateTime, string format)
		{
			var cell0 = row.GetOrCreateCell(index);
			var value = dateTime.ToString(format);
			cell0.SetCellValue(value.ToString());

			row.Sheet.SetColumnWidth2(index, value.Length);
		}

		public static void WriteString(this IRow row, int index, DateTime? dateTime, string format)
		{
			var cell0 = row.GetOrCreateCell(index);
			if (dateTime != null) {
				var value = dateTime.Value.ToString(format);
				cell0.SetCellValue(value.ToString());

				row.Sheet.SetColumnWidth2(index, value.Length);
			}
		}

		public static void WriteString(this ISheet sheet, int row, int index, string value, ICellStyle style)
		{
			var r = sheet.GetOrCreateRow(row);
			WriteString(r, index, value, style);
		}

		public static void WriteString(this ISheet sheet, int row, int index, string value)
		{
			var r = sheet.GetOrCreateRow(row);
			WriteString(r, index, value);
		}

		public static void WriteString(this ISheet sheet, int row, int index, decimal value)
		{
			var r = sheet.GetOrCreateRow(row);
			WriteString(r, index, value);
		}

		public static void WriteString(this ISheet sheet, int row, int index, decimal? value)
		{
			var r = sheet.GetOrCreateRow(row);
			WriteString(r, index, value);
		}

		public static void WriteString(this ISheet sheet, int row, int index, int value)
		{
			var r = sheet.GetOrCreateRow(row);
			WriteString(r, index, value);
		}

		public static void WriteString(this ISheet sheet, int row, int index, int? value)
		{
			var r = sheet.GetOrCreateRow(row);
			WriteString(r, index, value);
		}

		public static void WriteString(this ISheet sheet, int row, int index, DateTime dateTime, string format)
		{
			var r = sheet.GetOrCreateRow(row);
			WriteString(r, index, dateTime, format);
		}

		public static void WriteString(this ISheet sheet, int row, int index, DateTime? dateTime, string format)
		{
			var r = sheet.GetOrCreateRow(row);
			WriteString(r, index, dateTime, format);
		}

		#endregion WriteString

		public static ICell GetOrCreateCell(this IRow row, int col)
		{
			var cell0 = row.GetCell(col);
			if (cell0 == null) {
				cell0 = row.CreateCell(col);
			}
			return cell0;
		}

		public static IRow GetOrCreateRow(this ISheet sheet, int row)
		{
			var cell0 = sheet.GetRow(row);
			if (cell0 == null) {
				cell0 = sheet.CreateRow(row);
			}
			return cell0;
		}

		private static void SetColumnWidth2(this ISheet sheet, int index, int textLength)
		{
			var colWidth = (textLength + 5) * 256;
			var width = sheet.GetColumnWidth(index);
			if (width < colWidth && width != 20000) {
				if (colWidth < 20000) {
					sheet.SetColumnWidth(index, colWidth);
				} else {
					sheet.SetColumnWidth(index, 20000);
				}
			}
		}

		#region Read

		public static string ReadString(this IRow row, int col)
		{
			var cell0 = row.GetCell(col);
			if (cell0 == null) { return null; }

			//HSSFFormulaEvaluator evalor = new HSSFFormulaEvaluator(row.Sheet.Workbook);
			switch (cell0.CellType) {
				case CellType.Numeric:
					if (DateUtil.IsCellDateFormatted(cell0)) {
						return cell0.DateCellValue.ToString("yyyy-MM-dd");
					}
					return cell0.NumericCellValue.ToString();

				case CellType.String:
					return cell0.StringCellValue.Trim();

				case CellType.Formula:
					if (cell0.CachedFormulaResultType == CellType.String) {
						return cell0.StringCellValue.Trim();
					} else if (DateUtil.IsCellDateFormatted(cell0)) {
						return cell0.DateCellValue.ToString("yyyy-MM-dd");
					} else if (cell0.CachedFormulaResultType == CellType.Numeric) {
						return cell0.NumericCellValue.ToString();
					} else if (cell0.CachedFormulaResultType == CellType.Boolean) {
						return cell0.BooleanCellValue.ToString().ToLower();
					}
					break;

				case CellType.Boolean: return cell0.BooleanCellValue.ToString();
				case CellType.Blank: break;
				case CellType.Error: break;
				case CellType.Unknown: break;
				default: break;
			}
			return cell0.ToString().Trim();
		}

		public static int ReadNumber(this IRow row, int col, int def)
		{
			var txt = ReadString(row, col);
			if (int.TryParse(txt, out int i)) {
				return i;
			}
			return def;
		}

		#endregion Read
	}
}