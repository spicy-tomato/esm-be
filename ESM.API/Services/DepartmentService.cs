using ClosedXML.Excel;
using ESM.Common.Core.Exceptions;

namespace ESM.API.Services;

public static class DepartmentService
{
    #region Public methods

    public static Dictionary<string, List<string>> Import(IFormFile file, Guid schoolId)
    {
        using var wb = new XLWorkbook(file.OpenReadStream());
        var result = new Dictionary<string, List<string>>();

        var ws = wb.Worksheets.Worksheet("KHOA");

        if (ws == null)
        {
            throw new BadRequestException("Worksheet is empty!");
        }

        var currCol = 0;
        while (!ws.Cell(1, currCol + 1).Value.IsBlank)
        {
            currCol++;
            var facultyName = ws.Row(1).Cell(currCol).GetText();
            var currRow = 1;

            result.Add(facultyName, new List<string>());

            while (!ws.Cell(currRow, currCol).Value.IsBlank)
            {
                var departmentName = ws.Row(currRow).Cell(currCol).GetText();
                result[facultyName].Add(departmentName);
                currRow++;
            }
        }

        return result;
    }

    #endregion
}