using System.Dynamic;
using ClosedXML.Excel;
using ESM.Common.Core.Exceptions;

namespace ESM.API.Services;

public static class ModuleService
{
    #region Public methods

    public static IEnumerable<dynamic> Import(IFormFile file)
    {
        using var wb = new XLWorkbook(file.OpenReadStream());
        var result = new List<dynamic>();

        var ws = wb.Worksheets.FirstOrDefault();
        if (ws == null)
            throw new BadRequestException("Worksheet is empty!");

        var currRow = 1;
        while (!ws.Cell(currRow + 1, 1).Value.IsBlank)
        {
            currRow++;
            var moduleId = ws.Row(currRow).Cell(1).GetText();
            var moduleName = ws.Row(currRow).Cell(2).GetText();
            var credits = (int)ws.Row(currRow).Cell(3).GetDouble();
            var facultyName = ws.Row(currRow).Cell(4).GetText();
            string departmentName;
            try
            {
                departmentName = ws.Row(currRow).Cell(5).Value.GetText();
            }
            catch
            {
                departmentName = "";
            }

            dynamic row = new ExpandoObject();
            row.moduleId = moduleId;
            row.moduleName = moduleName;
            row.credits = credits;
            row.facultyName = facultyName;
            row.departmentName = departmentName;

            result.Add(row);
        }

        return result;
    }

    #endregion
}