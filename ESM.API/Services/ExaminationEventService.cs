using ESM.Data.Enums;
using ESM.Data.Models;
using MongoDB.Driver;

namespace ESM.API.Services;

public class ExaminationEventService
{
    private readonly IMongoCollection<ExaminationEvent> _examinationEventsCollection;

    public ExaminationEventService(IConfiguration configuration)
    {
        var client = new MongoClient(configuration["ExaminationEvent:ConnectionURI"]);
        var database = client.GetDatabase(configuration["ExaminationEvent:DatabaseName"]);
        _examinationEventsCollection =
            database.GetCollection<ExaminationEvent>(configuration["ExaminationEvent:CollectionName"]);
    }

    public async Task<ExaminationEvent> GetEvents(string examinationId)
    {
        var filter = Builders<ExaminationEvent>.Filter.Eq("Id", examinationId);
        return await _examinationEventsCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(string examinationId, DateTime dateTime)
    {
        await _examinationEventsCollection.InsertOneAsync(new ExaminationEvent
        {
            Id = examinationId,
            Events = new List<ExaminationEventDetails>
            {
                new()
                {
                    DateTime = dateTime,
                    Status = ExaminationStatus.Inactive
                }
            }
        });
    }

    public async Task AddEventAsync(string examinationId, ExaminationStatus status, DateTime dateTime)
    {
        var filter = Builders<ExaminationEvent>.Filter.Eq("Id", examinationId);
        var update = Builders<ExaminationEvent>.Update.Push("events",
            new ExaminationEventDetails
            {
                DateTime = dateTime,
                Status = status
            });
        await _examinationEventsCollection.UpdateOneAsync(filter, update);
    }
}