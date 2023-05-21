using JetBrains.Annotations;

namespace ESM.Data.Params.User;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record GetUsersParams(bool? IsInvigilator, bool? IsFaculty);