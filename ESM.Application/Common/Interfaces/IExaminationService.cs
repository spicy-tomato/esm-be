using ESM.Data.Enums;
using ESM.Data.Models;
using ESM.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace ESM.Application.Common.Interfaces;

public interface IExaminationService
{
    /// <summary>
    /// Import data
    /// </summary>
    /// <param name="file"></param>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    public List<ExaminationData> Import(IFormFile file, string examinationId);

    /// <summary>
    /// Validate temporary data
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public IQueryable<ExaminationData> ValidateTemporaryData(IEnumerable<ExaminationData> data);

    public Guid CheckIfExaminationExistAndReturnGuid(string examinationId, ExaminationStatus? acceptStatus = null);

    public Examination CheckIfExaminationExistAndReturnEntity(string examinationId,
        ExaminationStatus? acceptStatus = null);

    public IEnumerable<Shift> RetrieveShiftsFromTemporaryData(Guid examinationGuid, IEnumerable<ExaminationData> data);
}