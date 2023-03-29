using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace ESM.Data.Request.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ImportExaminationRequest
{
    public IFormFile? File { get; set; }
    public DateTime CreatedAt { get; set; }
}