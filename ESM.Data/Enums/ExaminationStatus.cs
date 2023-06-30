using JetBrains.Annotations;

namespace ESM.Data.Enums;

[Flags]
[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public enum ExaminationStatus
{
    None = 0,
    Idle = 1,
    Setup = 2,
    AssignFaculty = 4,
    AssignInvigilator = 8,
    Closed = 16
}
