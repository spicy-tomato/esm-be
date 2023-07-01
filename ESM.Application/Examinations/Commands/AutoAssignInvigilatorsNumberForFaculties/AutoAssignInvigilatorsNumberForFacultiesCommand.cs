using ESM.Application.Common.Interfaces;
using ESM.Application.Common.Models;
using ESM.Domain.Entities;
using ESM.Domain.Enums;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ESM.Application.Examinations.Commands.AutoAssignInvigilatorsNumberForFaculties;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record AutoAssignInvigilatorsNumberForFacultiesCommand(string Id) : IRequest<Result<bool>>;

public class AutoAssignInvigilatorsNumberForFacultiesCommandHandler
    : IRequestHandler<AutoAssignInvigilatorsNumberForFacultiesCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IExaminationService _examinationService;

    public AutoAssignInvigilatorsNumberForFacultiesCommandHandler(IApplicationDbContext context,
        IExaminationService examinationService)
    {
        _context = context;
        _examinationService = examinationService;
    }

    public async Task<Result<bool>> Handle(AutoAssignInvigilatorsNumberForFacultiesCommand request,
        CancellationToken cancellationToken)
    {
        var entity =
            _examinationService.CheckIfExaminationExistAndReturnEntity(request.Id, ExaminationStatus.AssignFaculty);

        await _context.Entry(entity)
            .Collection(e => e.ShiftGroups)
            .Query()
            .Include(eg => eg.Module)
            .Include(eg => eg.FacultyShiftGroups)
            .LoadAsync(cancellationToken);

        var faculties = _context.Faculties.AsNoTracking();
        var teachersNumberInFaculties = GetTeachersNumberInFaculties();
        var teachersTotal = teachersNumberInFaculties.Sum(t => t.Value);

        foreach (var group in entity.ShiftGroups)
        {
            var mainFacultyId = group.Module.FacultyId;
            var teachersNumberInRestFaculties =
                teachersTotal - teachersNumberInFaculties.GetValueOrDefault(mainFacultyId, 0);

            foreach (var facultyId in faculties.Select(f => f.Id))
            {
                var calculatedInvigilatorsCount = facultyId == mainFacultyId
                    ? group.RoomsCount
                    : Convert.ToInt32((group.InvigilatorsCount - group.RoomsCount) *
                                      (teachersNumberInFaculties.GetValueOrDefault(facultyId, 0) * 1.0 /
                                       teachersNumberInRestFaculties));
                var savedRecord = group.FacultyShiftGroups
                    .FirstOrDefault(feg => feg.FacultyId == facultyId);
                if (savedRecord == null)
                    group.FacultyShiftGroups.Add(new FacultyShiftGroup
                    {
                        FacultyId = facultyId,
                        ShiftGroup = group,
                        InvigilatorsCount = calculatedInvigilatorsCount,
                        CalculatedInvigilatorsCount = calculatedInvigilatorsCount
                    });
                else
                {
                    savedRecord.InvigilatorsCount = calculatedInvigilatorsCount;
                    savedRecord.CalculatedInvigilatorsCount = calculatedInvigilatorsCount;
                }
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Get(true);
    }

    private Dictionary<Guid, int> GetTeachersNumberInFaculties()
    {
        var teachers = _context.Teachers.Where(u => u.DepartmentId != null);
        var facultyTeachersCount = new Dictionary<Guid, int>();

        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (var teacher in teachers)
        {
            var faculty = teacher.Department!.Faculty;
            if (faculty == null)
            {
                continue;
            }

            var facultyId = faculty.Id;
            if (facultyTeachersCount.ContainsKey(facultyId))
            {
                facultyTeachersCount[facultyId]++;
            }
            else
            {
                facultyTeachersCount.Add(facultyId, 1);
            }
        }

        return facultyTeachersCount;
    }
}