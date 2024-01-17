using ESM.Application.Common.Exceptions.Core;
using ESM.Application.Common.Models;
using ESM.Application.Examinations.Commands.AssignInvigilatorsToShifts;
using ESM.Application.Examinations.Commands.AutoAssignInvigilatorsNumberForFaculties;
using ESM.Application.Examinations.Commands.AutoAssignInvigilatorsToGroups;
using ESM.Application.Examinations.Commands.AutoAssignInvigilatorsToShifts;
using ESM.Application.Examinations.Commands.ChangeStatus;
using ESM.Application.Examinations.Commands.Create;
using ESM.Application.Examinations.Commands.Import;
using ESM.Application.Examinations.Commands.Update;
using ESM.Application.Examinations.Commands.UpdateExamsNumber;
using ESM.Application.Examinations.Commands.UpdateTeacherAssignment;
using ESM.Application.Examinations.Queries.GetAllGroups;
using ESM.Application.Examinations.Queries.GetAllShifts;
using ESM.Application.Examinations.Queries.GetAllShiftsDetails;
using ESM.Application.Examinations.Queries.GetAvailableInvigilatorsInGroups;
using ESM.Application.Examinations.Queries.GetEvents;
using ESM.Application.Examinations.Queries.GetGroupsByFacultyId;
using ESM.Application.Examinations.Queries.GetHandoverData;
using ESM.Application.Examinations.Queries.GetRelatedExaminations;
using ESM.Application.Examinations.Queries.GetSummary;
using ESM.Application.Examinations.Queries.GetTemporaryData;
using ESM.Domain.Dtos.Examination;
using ESM.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESM.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ExaminationController : ApiControllerBase
{
    #region Public Methods

    /// <summary>
    /// Create new examination
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns> 
    [HttpPost(Name = "CreateExamination")]
    public async Task<Result<Guid>> Create(CreateCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Get related examinations of current user
    /// </summary>
    /// <returns></returns>
    [HttpGet("related", Name = nameof(GetRelated))]
    public async Task<Result<List<RelatedExaminationDto>>> GetRelated([FromQuery] GetRelatedExaminationsQuery query)
    {
        return await Mediator.Send(query);
    }

    /// <summary>
    /// Get examination data
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    [HttpGet("{examinationId}", Name = nameof(GetAllShifts))]
    public async Task<Result<List<GetAllShiftDto>>> GetAllShifts(string examinationId)
    {
        var query = new GetAllShiftsQuery(examinationId);
        return await Mediator.Send(query);
    }

    /// <summary>
    /// Import data
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="UnsupportedMediaTypeException"></exception>
    [HttpPost("{examinationId}", Name = "ImportExamination")]
    public async Task<Result<bool>> Import([FromForm] ImportCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Import data
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="UnsupportedMediaTypeException"></exception>
    [HttpPatch("{examinationId}", Name = "UpdateExamination")]
    public async Task<Result<bool>> Update(UpdateCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Get events of examination
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    [HttpGet("{examinationId}/events", Name = nameof(GetEvents))]
    public async Task<Result<List<ExaminationEvent>>> GetEvents(string examinationId)
    {
        var query = new GetEventsQuery(examinationId);
        return await Mediator.Send(query);
    }

    /// <summary>
    /// Get handover data
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    [HttpGet("{examinationId}/handover", Name = nameof(GetHandoverData))]
    public async Task<Result<List<HandoverDataDto>>> GetHandoverData(string examinationId)
    {
        var query = new GetHandoverDataQuery(examinationId);
        return await Mediator.Send(query);
    }

    /// <summary>
    /// Get all shifts in an examination
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    [HttpGet("{examinationId}/shift", Name = nameof(GetShifts))]
    public async Task<Result<List<ShiftDetailsDto>>> GetShifts(string examinationId)
    {
        var query = new GetAllShiftsDetailsQuery(examinationId);
        return await Mediator.Send(query);
    }

    /// <summary>
    /// Update invigilators in shift
    /// </summary>
    /// <param name="examinationId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPatch("{examinationId}/shift", Name = nameof(AssignInvigilatorsToShifts))]
    public async Task<Result<bool>> AssignInvigilatorsToShifts(string examinationId,
        [FromBody] AssignInvigilatorsToShiftsRequest request)
    {
        var command = new AssignInvigilatorsToShiftsCommand(examinationId, request);
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Update shift data (handover report, handover person)
    /// </summary>
    /// <returns></returns>
    [HttpPatch("{examinationId}/shift/{shiftId}", Name = "UpdateShiftExamination")]
    [Obsolete]
    public Result<bool> UpdateShift(object key)
    {
        // Moved to /shift/{shiftId}

        return Result<bool>.Get(true);
    }

    /// <summary>
    /// Auto assign teachers to shift group
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    [HttpPost("{examinationId}/shift/calculate", Name = nameof(AutoAssignTeachersToShift))]
    public async Task<Result<bool>> AutoAssignTeachersToShift(string examinationId)
    {
        var command = new AutoAssignInvigilatorsToShiftsCommand(examinationId);
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Change examination status
    /// </summary>
    /// <param name="examinationId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("{examinationId}/status", Name = nameof(ChangeStatus))]
    public async Task<Result<bool>> ChangeStatus(string examinationId, [FromBody] ChangeStatusRequest request)
    {
        var command = new ChangeStatusCommand(examinationId, request);
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Update exams number
    /// </summary>
    /// <param name="request"></param>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    [HttpPatch("{examinationId}/exams-number", Name = nameof(UpdateExamsCount))]
    public async Task<Result<bool>> UpdateExamsCount(string examinationId, [FromBody] UpdateExamsNumberRequest request)
    {
        var command = new UpdateExamsNumberCommand(examinationId, request);
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Get shift by faculty ID
    /// </summary>
    /// <param name="examinationId"></param>
    /// <param name="facultyId"></param>
    /// <returns></returns>
    [HttpGet("{examinationId}/faculty/{facultyId}/group", Name = nameof(GetGroupsByFacultyId))]
    public async Task<Result<List<GetGroupsByFacultyIdDto>>> GetGroupsByFacultyId(string examinationId,
        string facultyId)
    {
        var command = new GetGroupsByFacultyIdQuery(examinationId, facultyId);
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Update teacher assignment
    /// </summary>
    /// <param name="examinationId"></param>
    /// <param name="facultyId"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    [HttpPost("{examinationId}/faculty/{facultyId}/group", Name = nameof(UpdateTeacherAssignment))]
    public async Task<Result<bool>> UpdateTeacherAssignment(string examinationId,
        string facultyId,
        [FromBody] UpdateTeacherAssignmentRequest data)
    {
        var command = new UpdateTeacherAssignmentCommand(examinationId, facultyId, data);
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Auto assign teachers in faculty to shift groups
    /// </summary>
    /// <param name="examinationId"></param>
    /// <param name="facultyId"></param>
    /// <returns></returns>
    [HttpPost("{examinationId}/faculty/{facultyId}/group/calculate", Name = nameof(AutoAssignTeachersToGroups))]
    public async Task<Result<bool>> AutoAssignTeachersToGroups(string examinationId, string facultyId)
    {
        var command = new AutoAssignInvigilatorsToGroupsCommand(examinationId, facultyId);
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Get all shift groups in examination
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    [HttpGet("{examinationId}/group", Name = nameof(GetAllGroups))]
    public async Task<Result<List<GetAllGroupsDto>>> GetAllGroups(string examinationId)
    {
        var command = new GetAllGroupsQuery(examinationId);
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Calculate invigilators number of shift for each faculty
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BadRequestException"></exception>
    [HttpPost("{examinationId}/group/calculate", Name = nameof(CalculateInvigilatorNumerateOfShiftForEachFaculty))]
    public async Task<Result<bool>> CalculateInvigilatorNumerateOfShiftForEachFaculty(string examinationId)
    {
        var command = new AutoAssignInvigilatorsNumberForFacultiesCommand(examinationId);
        return await Mediator.Send(command);
    }

    [HttpPost("{examinationId}/group/{groupId}/department/{departmentId}",
        Name = nameof(UpdateTemporaryTeacherToUserIdInDepartmentShiftGroup))]
    [Obsolete]
    public Result<bool> UpdateTemporaryTeacherToUserIdInDepartmentShiftGroup(object key)
    {
        // Moved to /group/{groupId}

        return Result<bool>.Get(true);
    }

    /// <summary>
    /// Assign invigilator number of a shift for a faculty
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BadRequestException"></exception>
    [HttpPost("{examinationId}/group/{groupId}/{facultyId}", Name = nameof(AssignInvigilatorNumerateOfShiftToFaculty))]
    [Obsolete]
    public Result<bool> AssignInvigilatorNumerateOfShiftToFaculty(object key)
    {
        // Moved to /group/{groupId}/{facultyId}

        return Result<bool>.Get(true);
    }

    /// <summary>
    /// Get invigilators for each shift group
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BadRequestException"></exception>
    [HttpGet("{examinationId}/invigilator", Name = nameof(GetAvailableInvigilatorsInShiftGroup))]
    public async Task<Result<GetAvailableInvigilatorsInGroupsDto>> GetAvailableInvigilatorsInShiftGroup(
        string examinationId)
    {
        var command = new GetAvailableInvigilatorsInGroupsQuery(examinationId);
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Get summary
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns> 
    [HttpGet("{examinationId}/summary", Name = nameof(GetSummary))]
    public async Task<Result<ExaminationSummary>> GetSummary(string examinationId)
    {
        var command = new GetSummaryQuery(examinationId);
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Get temporary data
    /// </summary>
    /// <param name="examinationId"></param>
    /// <returns></returns>
    [HttpGet("{examinationId}/temporary", Name = nameof(GetTemporaryData))]
    public async Task<Result<List<ExaminationData>>> GetTemporaryData(string examinationId)
    {
        var command = new GetTemporaryDataQuery(examinationId);
        return await Mediator.Send(command);
    }

    #endregion
}