using ESM.Data.Enums;
using JetBrains.Annotations;
using MongoDB.Bson.Serialization.Attributes;

namespace ESM.Data.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ExaminationEventDetails
{
    [BsonElement("status")]
    public ExaminationStatus Status;

    [BsonElement("dateTime")]
    public DateTime DateTime;
}

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ExaminationEvent
{
    [BsonId]
    public string? Id { get; set; }

    [BsonElement("events")]
    public List<ExaminationEventDetails> Events { get; set; } = null!;
}