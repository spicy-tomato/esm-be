using ESM.Data.Dtos.Faculty;
using ESM.Data.Models;
using JetBrains.Annotations;

namespace ESM.Data.Dtos.Examination;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class FacultyShiftGroupSimple
{
    public Guid Id { get; set; }
    public int InvigilatorsCount { get; set; }
    public int CalculatedInvigilatorsCount { get; set; }
    public FacultySummary Faculty { get; set; } = null!;
    public ShiftGroupInDepartmentShiftGroupSimple ShiftGroup { get; set; } = null!;
    public ICollection<DepartmentShiftGroup> DepartmentShiftGroups { get; set; } = new List<DepartmentShiftGroup>();
    
    // Additional properties
    public Dictionary<string, ShiftGroupDataCell> AssignNumerate = new();
}