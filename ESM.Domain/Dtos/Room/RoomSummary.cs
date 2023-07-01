using JetBrains.Annotations;

namespace ESM.Domain.Dtos.Room;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class RoomSummary
{
    public Guid Id { get; set; }
    public string DisplayId { get; set; } = null!;
    public int? Capacity { get; set; }
}