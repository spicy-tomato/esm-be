using ClosedXML.Excel;
using ESM.Common.Core.Exceptions;

namespace ESM.API.Services;

public static class DepartmentService
{
    #region Public methods

    public static Dictionary<string, Dictionary<string, List<string>>> Import(IFormFile file)
    {
        using var wb = new XLWorkbook(file.OpenReadStream());
        var result = new Dictionary<string, Dictionary<string, List<string>>>();

        var ws = wb.Worksheets.Worksheet("KHOA");
        if (ws == null)
            throw new BadRequestException("Worksheet is empty!");

        var currCol = 0;
        while (!ws.Cell(1, currCol + 1).Value.IsBlank)
        {
            currCol++;
            var facultyName = ws.Row(1).Cell(currCol).GetText();
            var currRow = 2;

            result.Add(facultyName, new Dictionary<string, List<string>>());

            while (!ws.Cell(currRow, currCol).Value.IsBlank)
            {
                var departmentName = ws.Row(currRow).Cell(currCol).GetText();
                result[facultyName].Add(departmentName, new List<string>());
                currRow++;
            }
        }

        ws = wb.Worksheets.Worksheet("BM");
        currCol = 0;
        while (!ws.Cell(1, currCol + 1).Value.IsBlank)
        {
            currCol++;
            var departmentName = ws.Row(1).Cell(currCol).GetText();
            var currRow = 2;

            while (!ws.Cell(currRow, currCol).Value.IsBlank)
            {
                var teacherName = ws.Row(currRow).Cell(currCol).GetText();
                foreach (var (_, department) in result)
                {
                    foreach (var (deptName, teachers) in department)
                    {
                        if (deptName == departmentName)
                            teachers.Add(teacherName);
                    }
                }

                currRow++;
            }
        }


        return result;
    }

    #endregion
}