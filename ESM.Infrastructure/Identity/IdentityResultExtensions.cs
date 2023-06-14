using ESM.Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public static class IdentityResultExtensions
{
    public static Result<bool> ToApplicationResult(this IdentityResult result)
    {
        return result.Succeeded
            ? Result.Get(true)
            : Result.Failure(result.Errors);
    }
}