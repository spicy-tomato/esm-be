using JetBrains.Annotations;

namespace ESM.Data.Enums;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public enum ExaminationStatus
{
    Inactive = 0,
    Idle = 1,
    Setup = 2,
    AssignFaculty = 3,
    AssignInvigilator = 4
}
