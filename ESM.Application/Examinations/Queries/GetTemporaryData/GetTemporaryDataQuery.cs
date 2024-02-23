using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Mappings;
using ESM.Application.Common.Models;
using ESM.Domain.Entities;
using ESM.Domain.Enums;
using MediatR;
using RoomHelper = ESM.Application.Common.Helpers.RoomHelper;

namespace ESM.Application.Examinations.Queries.GetTemporaryData;

public record GetTemporaryDataQuery(string Id, int PageNumber, int PageSize) : IRequest<Result<PaginatedList<ExaminationData>>>;

public class GetTemporaryDataQueryHandler : IRequestHandler<GetTemporaryDataQuery, Result<PaginatedList<ExaminationData>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IExaminationService _examinationService;

    public GetTemporaryDataQueryHandler(IExaminationService examinationService, IApplicationDbContext context)
    {
        _examinationService = examinationService;
        _context = context;
    }

    public async Task<Result<PaginatedList<ExaminationData>>> Handle(GetTemporaryDataQuery request,
        CancellationToken cancellationToken)
    {
        var guid = _examinationService.CheckIfExaminationExistAndReturnGuid(request.Id, ExaminationStatus.Setup);

        var data = await GetTemporaryData(guid, request);

        return Result<PaginatedList<ExaminationData>>.Get(data);
    }

    private async Task<PaginatedList<ExaminationData>> GetTemporaryData(Guid examinationId, GetTemporaryDataQuery request)
    {
        var data = await _context.ExaminationData
            .Where(e => e.ExaminationId == examinationId)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        if (!data.IsEmpty)
        {
            data = ValidateData(data);
        }

        return data;
    }

    private PaginatedList<ExaminationData> ValidateData(PaginatedList<ExaminationData> data)
    {
        var existedModules = _context.Modules
            .Select(m => m.DisplayId)
            .ToDictionary(m => m, _ => true);
        var existedRooms = _context.Rooms
            .Select(m => m.DisplayId)
            .ToDictionary(m => m, _ => true);

        foreach (var row in data.Items)
        {
            var fields = new[]
            {
                "ModuleId", "ModuleName", "ModuleClass", "Credit", "Method", "Date", "StartAt", "EndAt", "Shift",
                "CandidatesCount", "RoomsCount", "Rooms", "Faculty", "Department", "DepartmentAssign"
            };
            var acceptNullFields = new[] { "Shift", "Department" };

            foreach (var field in fields)
            {
                if (acceptNullFields.Contains(field))
                    continue;

                var fieldValue = typeof(ExaminationData).GetProperty(field)?.GetValue(row);
                var normalizeField = string.Concat(field[..1].ToLower(), field[1..]);

                if (fieldValue is null or "")
                    row.Errors.Add(normalizeField, new ExaminationDataError("Trường này chưa có giá trị"));
            }

            ValidateModuleId(row, existedModules);
            ValidateRoom(row, existedRooms);
        }

        return data;
    }

    /// <summary>
    /// Validate module ID
    /// </summary>
    /// <param name="row"></param>
    /// <param name="existedModules"></param>
    private static void ValidateModuleId(ExaminationData row, IReadOnlyDictionary<string, bool> existedModules)
    {
        if (row.ModuleId != null && !existedModules.ContainsKey(row.ModuleId))
        {
            row.Errors.Add("moduleId", new ExaminationDataError("Mã học phần này không tồn tại"));
        }
    }

    /// <summary>
    /// Validate rooms
    /// </summary>
    /// <param name="row"></param>
    /// <param name="existedRooms"></param>
    private static void ValidateRoom(ExaminationData row, IReadOnlyDictionary<string, bool> existedRooms)
    {
        var rooms = RoomHelper.GetRoomsFromString(row.Rooms);
        if (rooms.Length == 0)
        {
            row.Errors.Add("rooms", new ExaminationDataError("Chưa có phòng thi"));
            return;
        }

        var notExistedRooms = rooms.Where(room => !existedRooms.ContainsKey(room)).ToList();

        if (notExistedRooms.Count > 0)
        {
            row.Errors.Add("rooms",
                new ExaminationDataError<string>(
                    "Các phòng thi sau không tồn tại: " + string.Join(", ", notExistedRooms),
                    notExistedRooms)
            );
        }
    }
}