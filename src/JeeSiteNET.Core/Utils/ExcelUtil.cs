using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Data;

namespace JeeSiteNET.Core.Utils;

/// <summary>
/// Excel 导入导出工具类（基于 NPOI，支持 .xlsx 与 .xls 两种格式）
/// </summary>
public static class ExcelUtil
{
    /// <summary>
    /// 将 DataTable 导出为 Excel（XLSX 格式），首行写入列名
    /// </summary>
    /// <param name="data">要导出的数据表</param>
    /// <param name="sheetName">工作表名称，默认 "Sheet1"</param>
    /// <returns>XLSX 文件字节数组</returns>
    public static byte[] ExportToExcel(DataTable data, string sheetName = "Sheet1")
    {
        IWorkbook workbook = new XSSFWorkbook();
        var sheet = workbook.CreateSheet(sheetName);
        var headerRow = sheet.CreateRow(0);
        for (int i = 0; i < data.Columns.Count; i++)
            headerRow.CreateCell(i).SetCellValue(data.Columns[i].ColumnName);

        // 遍历 DataRow，逐行逐列写入单元格（空值用空字符串占位）
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

    /// <summary>
    /// 从字节数组导入 Excel（自动识别 .xlsx / .xls），首行作为列名
    /// </summary>
    /// <param name="bytes">Excel 文件字节数据</param>
    /// <param name="sheetName">可选工作表名称；为 null 时读取第一个工作表</param>
    /// <returns>解析得到的 DataTable</returns>
    public static DataTable ImportFromExcel(byte[] bytes, string? sheetName = null)
    {
        // 通过首字节识别 ZIP 签名（XLSX 为 ZIP 打包格式），否则按旧式 HSSF 处理
        IWorkbook workbook = bytes[0] == 0x50 ? new XSSFWorkbook(new MemoryStream(bytes)) : new HSSFWorkbook(new MemoryStream(bytes));
        var sheet = sheetName != null ? workbook.GetSheet(sheetName) : workbook.GetSheetAt(0);
        var dt = new DataTable();

        if (sheet.GetRow(0) != null)
        {
            foreach (var cell in sheet.GetRow(0).Cells)
                dt.Columns.Add(cell.StringCellValue);
        }

        // 从第二行开始读取数据行
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
