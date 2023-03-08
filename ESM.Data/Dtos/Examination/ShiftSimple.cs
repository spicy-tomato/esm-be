using ESM.Data.Dtos.Room;
using JetBrains.Annotations;

namespace ESM.Data.Dtos.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ShiftSimple
{
    public Guid Id { get; set; }
    public int ExamsCount { get; set; }
    public int CandidatesCount { get; set; }
    public int InvigilatorsCount { get; set; }
    public DateTime StartAt { get; set; }
    public RoomSummary Room { get; set; } = null!;
    public ShiftGroupSimple ShiftGroup { get; set; } = null!;
}