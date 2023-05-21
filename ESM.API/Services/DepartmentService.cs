using System.Globalization;
using ClosedXML.Excel;
using ESM.Common.Core.Exceptions;

namespace ESM.API.Services;

public static class DepartmentService
{
    #region Public methods

    public static Dictionary<string, Dictionary<string, List<KeyValuePair<string, string>>>> Import(IFormFile file)
    {
        using var wb = new XLWorkbook(file.OpenReadStream());
        var result = new Dictionary<string, Dictionary<string, List<KeyValuePair<string, string>>>>();

        var ws = wb.Worksheets.Worksheet("KHOA");
        if (ws == null)
            throw new BadRequestException("Worksheet is empty!");

        var currCol = 0;
        int currRow;

        while (!CellIsBlank(1, currCol + 1, ws))
        {
            currCol++;
            var facultyName = ws.Row(1).Cell(currCol).GetText();
            currRow = 2;

            result.Add(facultyName, new Dictionary<string, List<KeyValuePair<string, string>>>());

            while (!ws.Cell(currRow, currCol).Value.IsBlank)
            {
                var departmentName = ws.Row(currRow).Cell(currCol).GetText();
                result[facultyName].Add(departmentName, new List<KeyValuePair<string, string>>());
                currRow++;
            }
        }

        ws = wb.Worksheets.Worksheet("BM");
        currRow = 1;

        while (!CellIsBlank(++currRow, 2, ws))
        {
            var teacherName = ws.Row(currRow).Cell(2).GetText();
            var departmentName = ws.Row(currRow).Cell(4).GetText();
            var facultyName = ws.Row(currRow).Cell(5).GetText();

            string teacherId;
            if (ws.Row(currRow).Cell(6).IsEmpty())
                teacherId = "";
            else
                try
                {
                    teacherId = ws.Row(currRow).Cell(6).GetDouble().ToString(CultureInfo.InvariantCulture);
                }
                catch
                {
                    teacherId = ws.Row(currRow).Cell(6).GetString();
                }

            var facultyNameInDict = result.Keys.FirstOrDefault(k => k.Contains(facultyName));
            if (facultyNameInDict == null) 
                throw new BadRequestException($"Faculty does not exist: {facultyName}");

            var departments = result[facultyNameInDict];
            if (!departments.TryGetValue(departmentName, out var teachers))
                throw new BadRequestException($"Department does not exist: {departmentName}");

            teachers.Add(new KeyValuePair<string, string>(teacherId, teacherName));
        }

        return result;
    }

    private static bool CellIsBlank(int row, int col, IXLWorksheet ws)
    {
        return ws.Cell(row, col).Value.IsBlank;
    }

    #endregion
}