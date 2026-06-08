using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Data;

namespace JeeSiteNET.Core.Utils;

public static class ExcelUtil
{
    public static byte[] ExportToExcel(DataTable data, string sheetName = "Sheet1")
    {
        IWorkbook workbook = new XSSFWorkbook();
        var sheet = workbook.CreateSheet(sheetName);
        var headerRow = sheet.CreateRow(0);
        for (int i = 0; i < data.Columns.Count; i++)
            headerRow.CreateCell(i).SetCellValue(data.Columns[i].ColumnName);

        for (int r = 0; r < data.Rows.Count; r++)
        {
            var row = sheet.CreateRow(r + 1);
            for (int c = 0; c < data.Columns.Count; c++)
                row.CreateCell(c).SetCellValue(data.Rows[r][c]?.ToString() ?? "");
        }

        using var ms = new MemoryStream();
        workbook.Write(ms);
        return ms.ToArray();
    }

    public static DataTable ImportFromExcel(byte[] bytes, string? sheetName = null)
    {
        IWorkbook workbook = bytes[0] == 0x50 ? new XSSFWorkbook(new MemoryStream(bytes)) : new HSSFWorkbook(new MemoryStream(bytes));
        var sheet = sheetName != null ? workbook.GetSheet(sheetName) : workbook.GetSheetAt(0);
        var dt = new DataTable();

        if (sheet.GetRow(0) != null)
        {
            foreach (var cell in sheet.GetRow(0).Cells)
                dt.Columns.Add(cell.StringCellValue);
        }

        for (int r = 1; r <= sheet.LastRowNum; r++)
        {
            var row = sheet.GetRow(r);
            if (row == null) continue;
            var dr = dt.NewRow();
            for (int c = 0; c < dt.Columns.Count; c++)
            {
                var cell = row.GetCell(c);
                dr[c] = cell?.ToString() ?? "";
            }
            dt.Rows.Add(dr);
        }

        return dt;
    }
}
