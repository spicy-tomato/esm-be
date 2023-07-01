using ESM.Application.Common.Interfaces;

namespace ESM.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}