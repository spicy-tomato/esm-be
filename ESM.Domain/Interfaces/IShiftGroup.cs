using ESM.Domain.Dtos.Examination;
using JetBrains.Annotations;

namespace ESM.Domain.Interfaces;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public interface IShiftGroup
{
    public Dictionary<string, ShiftGroupDataCell> AssignNumerate { get; set; }
    public int InvigilatorsCount { get; set; }
}