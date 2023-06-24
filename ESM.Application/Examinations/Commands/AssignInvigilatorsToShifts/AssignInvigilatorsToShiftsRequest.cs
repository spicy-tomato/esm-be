using System.Runtime.Serialization;

namespace ESM.Application.Examinations.Commands.AssignInvigilatorsToShifts;

[Serializable]
public class AssignInvigilatorsToShiftsRequest : Dictionary<string, string?>
{
    public AssignInvigilatorsToShiftsRequest() { }

    protected AssignInvigilatorsToShiftsRequest(SerializationInfo info, StreamingContext context) :
        base(info, context) { }
}