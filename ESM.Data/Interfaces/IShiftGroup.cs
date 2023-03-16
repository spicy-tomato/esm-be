using ESM.Data.Dtos.Examination;
using JetBrains.Annotations;

namespace ESM.Data.Interfaces;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public interface IShiftGroup
{
    public Dictionary<string, ShiftGroupDataCell> AssignNumerate { get; set; }
    public int InvigilatorsCount { get; set; }
}