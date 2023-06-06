using System.Runtime.Serialization;

namespace ESM.Data.Request.Examination;

[Serializable]
public class AssignInvigilatorsToShiftsRequest : Dictionary<string, string?>
{
    public AssignInvigilatorsToShiftsRequest() { }

    protected AssignInvigilatorsToShiftsRequest(SerializationInfo info, StreamingContext context) :
        base(info, context) { }
}