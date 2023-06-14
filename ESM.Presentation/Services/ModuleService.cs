using System.Dynamic;
using ClosedXML.Excel;
using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Interfaces;

namespace ESM.API.Services;

public class ModuleService : IModuleService
{
    #region Public methods

    public IEnumerable<dynamic> Import(IFormFile file)
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

            if (!ws.Row(currRow).Cell(5).Value.TryGetText(out var departmentName))
                departmentName = "";

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