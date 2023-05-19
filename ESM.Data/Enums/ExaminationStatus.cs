using JetBrains.Annotations;

namespace ESM.Data.Enums;

[Flags]
[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public enum ExaminationStatus
{
    Void = 0,
    Idle = 1,
    Setup = 2,
    AssignFaculty = 4,
    AssignInvigilator = 8
}
