using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// using Microsoft.EntityFrameworkCore;
using NodaTime;
using NodaTime.Text;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp;
namespace Vosita.Slots;

public class SlotAppService : ApplicationService, ISlotAppService
{
    private readonly IRepository<Slot, Guid> _slotRepository;
    private readonly IClock _clock;

    public SlotAppService(
        IRepository<Slot, Guid> slotRepository,
        IClock clock)
    {
        _slotRepository = slotRepository;

        _clock = clock;
    }

    public async Task<GenerateSlotsResultDto> GenerateSlotsAsync(GenerateSlotsInputDto input)
    {
        // LocalDatePattern.Iso.Parse is used to convert the string into nodatime local data object 
        //"2026-06-01" → { Year: 2026, Month: 6, Day: 1 }
        var startParseResult = LocalDatePattern.Iso.Parse(input.StartDate);
        if (!startParseResult.Success)
            throw new UserFriendlyException("Invalid start date format. you must  Use YYYY-MM-DD");
        var startLocalDate = startParseResult.Value;

        // Validate end date
        var endParseResult = LocalDatePattern.Iso.Parse(input.EndDate);
        if (!endParseResult.Success)
            throw new UserFriendlyException("Invalid end date format. Use YYYY-MM-DD");
        var endLocalDate = endParseResult.Value;

        if (startLocalDate > endLocalDate)
            throw new UserFriendlyException("Start date must be before or equal to end date.");

        if (input.SlotDuration <= 0)
            throw new UserFriendlyException("Slot duration must be positive.");

        if (input.SlotDuration > 1440)
            throw new UserFriendlyException("Slot duration cannot exceed 1440 minutes (24 hours).");


        // DateTimeZoneProviders.Tzdb.GetZoneOrNull is a NodaTime method that converts a time zone name into an actual time zone object
        var timeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(input.TimeZone);
        if (timeZone == null)
            throw new UserFriendlyException($"Invalid time zone: {input.TimeZone}");

        // generating slots 
        var slotsToCreate = new List<Slot>();
        var currentDate = startLocalDate;

        while (currentDate <= endLocalDate)
        {

            // here we get the day boundaries 
            //finds the exact UTC moments when the day starts and ends in the LOCAL time zone for.

            var startOfDay = currentDate.AtMidnight().InZoneLeniently(timeZone).ToInstant();
            var endOfDay = currentDate.PlusDays(1).AtMidnight().InZoneLeniently(timeZone).ToInstant();

            var slotStartInstant = startOfDay;
            var slotDurationDuration = Duration.FromMinutes(input.SlotDuration);

            while (slotStartInstant < endOfDay)
            {
                // here we add the slot duration to the start time to get the end time of this slot
                var slotEndInstant = slotStartInstant.Plus(slotDurationDuration);

                if (slotEndInstant > endOfDay)
                    break;

                slotsToCreate.Add(new Slot(
                    GuidGenerator.Create(),
                    slotStartInstant,
                    slotEndInstant,
                    input.TimeZone
                ));

                slotStartInstant = slotEndInstant;
            }

            currentDate = currentDate.PlusDays(1);
        }

        await _slotRepository.InsertManyAsync(slotsToCreate);

        return new GenerateSlotsResultDto
        {
            TotalSlotsCreated = slotsToCreate.Count
        };
    }


    public async Task<List<SlotDto>> GetNextAvailableSlotsAsync(GetNextAvailableSlotsInputDto input)
    {
        var timeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(input.TimeZone);
        if (timeZone == null)
            throw new UserFriendlyException($"Invalid time zone: {input.TimeZone}");

        var now = _clock.GetCurrentInstant();

        // get querable async is like query builder
        var queryable = await _slotRepository.GetQueryableAsync();

        var query = queryable
            .Where(s => s.Status == SlotStatus.Available && s.StartInstant > now)
            .OrderBy(s => s.StartInstant)
            .Take(input.Count);

        //AsyncExecuter is an abp helper for async operations 
        var slots = await AsyncExecuter.ToListAsync(query);

        var slotDtos = slots.Select(slot =>
        {
            // here we will convert the utc time to the time zone of the user 
            var zonedStart = slot.StartInstant.InZone(timeZone);
            var zonedEnd = slot.EndInstant.InZone(timeZone);

            return new SlotDto
            {
                Id = slot.Id,
                LocalStartTime = zonedStart.ToString("yyyy-MM-dd HH:mm:ss", null),
                LocalEndTime = zonedEnd.ToString("yyyy-MM-dd HH:mm:ss", null),
                TimeZone = input.TimeZone,
                IsBookable = true
            };
        }).ToList();

        return slotDtos;
    }
    public async Task BookSlotAsync(Guid id)
    {
        var slot = await _slotRepository.GetAsync(id);

        if (slot.Status == SlotStatus.Booked)
            throw new UserFriendlyException("This slot is already booked.");

        if (slot.StartInstant < _clock.GetCurrentInstant())
            throw new UserFriendlyException("Cannot book slots in the past.");

        slot.MarkAsBooked();
        await _slotRepository.UpdateAsync(slot);
    }
}