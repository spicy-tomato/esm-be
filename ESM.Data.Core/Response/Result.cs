using ESM.Common.Core;
using JetBrains.Annotations;

namespace ESM.Data.Core.Response;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class Result<T>
{
    public T Data { get; set; } = default!;

    public bool Success { get; set; }

    public IEnumerable<Error>? Errors { get; set; }

    public static Result<T1> Get<T1>(T1 data)
    {
        return new Result<T1>
        {
            Success = true,
            Data = data
        };
    }
}