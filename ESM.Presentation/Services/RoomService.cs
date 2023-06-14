using ClosedXML.Excel;
using ESM.Application.Common.Exceptions;
using ESM.Application.Common.Interfaces;

namespace ESM.API.Services;

public class RoomService : IRoomService
{
    #region Public methods

    public IEnumerable<string> Import(IFormFile file)
    {
        using var wb = new XLWorkbook(file.OpenReadStream());
        var result = new List<string>();

        var ws = wb.Worksheets.FirstOrDefault();
        if (ws == null)
            throw new BadRequestException("Worksheet is empty!");

        var currRow = 0;
        while (!ws.Cell(currRow + 1, 1).Value.IsBlank)
        {
            currRow++;
            var room = ws.Row(currRow).Cell(1).GetText();
            result.Add(room);
        }

        return result;
    }

    #endregion
}