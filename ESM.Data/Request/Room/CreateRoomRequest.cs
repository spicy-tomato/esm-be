using JetBrains.Annotations;

namespace ESM.Data.Request.Room;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class CreateRoomRequest
{
    public string DisplayId { get; set; } = null!;
    public int? Capacity { get; set; }
}