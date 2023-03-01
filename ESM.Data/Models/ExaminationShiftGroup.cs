using System.ComponentModel.DataAnnotations;
using ESM.Data.Enums;
using JetBrains.Annotations;

namespace ESM.Data.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ExaminationShiftGroup
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

    public Guid? ModuleId { get; set; }
    public Module Module { get; set; } = null!;

    public ICollection<ExaminationShift> ExaminationShifts { get; set; } = new List<ExaminationShift>();
}