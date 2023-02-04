using JetBrains.Annotations;

namespace ESM.Common.Core;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class Error
{
    public int? Code { get; }

    public string Message { get; }

    public string? Property { get; }

    public Error(string message)
    {
        Message = message;
    }

    public Error(int code, string message)
    {
        Code = code;
        Message = message;
    }

    public Error(string property, string message)
    {
        Property = property;
        Message = message;
    }
}