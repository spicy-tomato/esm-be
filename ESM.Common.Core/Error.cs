using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace ESM.Common.Core;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class Error
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Code { get; }

    public string Message { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
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