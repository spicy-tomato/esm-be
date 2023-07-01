using ESM.Domain.Entities;
using ESM.Domain.Enums;
using ESM.Domain.Interfaces;
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

    public Guid CheckIfExaminationExistAndReturnGuid(string examinationId, ExaminationStatus? acceptStatus = null);

    public Examination CheckIfExaminationExistAndReturnEntity(string examinationId,
        ExaminationStatus? acceptStatus = null);

    public void CalculateInvigilatorsNumberInShift<T>(T group,
        ICollection<FacultyShiftGroup> facultyShiftGroup,
        IReadOnlyDictionary<Guid, int> invigilatorsNumberInFaculties) where T : IShiftGroup;
}