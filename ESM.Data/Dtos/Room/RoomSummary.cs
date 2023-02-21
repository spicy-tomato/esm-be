using JetBrains.Annotations;

namespace ESM.Data.Dtos.Room;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class RoomSummary
{
    public Guid Id { get; set; }
    public string DisplayId { get; set; } = null!;
    public int? Capacity { get; set; }
}