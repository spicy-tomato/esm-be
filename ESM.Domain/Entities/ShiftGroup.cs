using System.ComponentModel.DataAnnotations;
using ESM.Data.Enums;
using JetBrains.Annotations;

namespace ESM.Domain.Entities;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ShiftGroup
{
    [Key]
    public Guid Id { get; set; }

    public ExamMethod Method { get; set; }

    public int InvigilatorsCount { get; set; }

    public int RoomsCount { get; set; }

    public DateTime StartAt { get; set; }

    public int? Shift { get; set; }

    public bool DepartmentAssign { get; set; }

    public Guid ExaminationId { get; set; }
    public Examination Examination { get; set; } = null!;

    public Guid ModuleId { get; set; }
    public Module Module { get; set; } = null!;

    public ICollection<Shift> Shifts { get; set; } = new List<Shift>();

    public ICollection<FacultyShiftGroup> FacultyShiftGroups { get; set; } = new List<FacultyShiftGroup>();
}