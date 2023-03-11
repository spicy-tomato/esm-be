using ESM.Data.Dtos.Module;
using ESM.Data.Enums;
using ESM.Data.Models;
using JetBrains.Annotations;

namespace ESM.Data.Dtos.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ShiftGroupSimple
{
    public Guid Id { get; set; }
    public ExamMethod Method { get; set; }
    public int InvigilatorsCount { get; set; }
    public int RoomsCount { get; set; }
    public DateTime StartAt { get; set; }
    public int? Shift { get; set; }
    public bool DepartmentAssign { get; set; }
    public ModuleSimple Module { get; set; } = null!;
    public ICollection<FacultyShiftGroup> FacultyShiftGroups { get; set; } = new List<FacultyShiftGroup>();

    // Additional properties
    public Dictionary<string, ShiftGroupDataCell> AssignNumerate = new();
}