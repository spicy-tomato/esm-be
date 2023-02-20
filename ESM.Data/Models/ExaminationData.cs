using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESM.Data.Enums;
using JetBrains.Annotations;

namespace ESM.Data.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ExaminationData
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string? ModuleId { get; set; }

    public string? ModuleName { get; set; }

    public string? ModuleClass { get; set; }

    public int? Credit { get; set; }

    public ExamMethod? Method { get; set; }

    public DateTime? Date { get; set; }

    public DateTime? StartAt { get; set; }

    public DateTime? EndAt { get; set; }

    public int? Shift { get; set; }

    public int? CandidateCount { get; set; }

    public int? RoomsCount { get; set; }

    public string? Rooms { get; set; }

    public string? Faculty { get; set; }

    public string? Department { get; set; }

    public bool? DepartmentAssign { get; set; }

    public Guid ExaminationId { get; set; }
    public Examination Examination { get; set; } = null!;

    [NotMapped]
    public Dictionary<string, string> Errors { get; set; } = new();

    [NotMapped]
    public Dictionary<string, List<KeyValuePair<string, string>>> Suggestions { get; set; } = new();
}