using JetBrains.Annotations;

namespace ESM.Data.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class FacultyExaminationShiftGroup
{
    public int InvigilatorsCount { get; set; }
    public int CalculatedInvigilatorsCount { get; set; }

    public Guid FacultyId { get; set; }
    public Faculty Faculty { get; set; } = null!;

    public Guid ExaminationShiftGroupId { get; set; }
    public ExaminationShiftGroup ExaminationShiftGroup { get; set; } = null!;
}