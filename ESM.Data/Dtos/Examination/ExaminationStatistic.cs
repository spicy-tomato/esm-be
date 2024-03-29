using JetBrains.Annotations;

namespace ESM.Data.Dtos.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ExaminationStatistic
{
    public Guid Id { get; set; }
    public string DisplayId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTime? StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public double TimePercent { get; set; }
    public int NumberOfModules { get; set; }
    public int NumberOfModulesOver { get; set; }
    public int NumberOfShifts { get; set; }
    public int NumberOfShiftsOver { get; set; }
    public int NumberOfCandidates { get; set; }
    public int NumberOfInvigilators { get; set; }
}