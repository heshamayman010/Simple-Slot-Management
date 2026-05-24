using System;
using System.Linq;
using System.Threading.Tasks;
using NodaTime;
using Shouldly;
using Volo.Abp;
using Volo.Abp.Validation;
using Vosita.Slots;
using Xunit;

namespace Vosita.Application.Tests.Slots;

public class SlotAppServiceTests : VositaApplicationTestBase<VositaApplicationTestModule>
{
    private readonly ISlotAppService _slotAppService;

    public SlotAppServiceTests()
    {
        _slotAppService = GetRequiredService<ISlotAppService>();
    }

    [Fact]
    public async Task Generate_Slots_For_Single_Day_30_Minutes()
    {
        var input = new GenerateSlotsInputDto
        {
            StartDate = "2026-06-01",
            EndDate = "2026-06-01",
            TimeZone = "Africa/Cairo",
            SlotDuration = 30
        };

        var result = await _slotAppService.GenerateSlotsAsync(input);

        // here it is 24 hours × 2 slots per hour
        result.TotalSlotsCreated.ShouldBe(48);
    }

    [Fact]
    public async Task Generate_Slots_For_Single_Day_60_Minutes()
    {
        // Arrange
        var input = new GenerateSlotsInputDto
        {
            StartDate = "2026-06-01",
            EndDate = "2026-06-01",
            TimeZone = "Africa/Cairo",
            SlotDuration = 60
        };

        var result = await _slotAppService.GenerateSlotsAsync(input);

        result.TotalSlotsCreated.ShouldBe(24);
    }

    [Fact]
    public async Task Generate_Slots_For_Multiple_Days()
    {
        var input = new GenerateSlotsInputDto
        {
            StartDate = "2026-06-01",
            EndDate = "2026-06-03",
            TimeZone = "Africa/Cairo",
            SlotDuration = 30
        };

        var result = await _slotAppService.GenerateSlotsAsync(input);

        result.TotalSlotsCreated.ShouldBe(144);
    }

    [Fact]
    public async Task Exception_When_StartDate_After_EndDate()
    {
        var input = new GenerateSlotsInputDto
        {
            StartDate = "2026-06-10",
            EndDate = "2026-06-01",
            TimeZone = "Africa/Cairo",
            SlotDuration = 30
        };

        await Should.ThrowAsync<UserFriendlyException>(async () =>
        {
            await _slotAppService.GenerateSlotsAsync(input);
        });
    }

    [Fact]
    public async Task Exception_When_Duration_Is_Zero()
    {
        var input = new GenerateSlotsInputDto
        {
            StartDate = "2026-06-01",
            EndDate = "2026-06-01",
            TimeZone = "Africa/Cairo",
            SlotDuration = 0
        };

        await Should.ThrowAsync<AbpValidationException>(async () =>
        {
            await _slotAppService.GenerateSlotsAsync(input);
        });
    }

    [Fact]
    public async Task Exception_When_Duration_Exceeds_24_Hours()
    {
        var input = new GenerateSlotsInputDto
        {
            StartDate = "2026-06-01",
            EndDate = "2026-06-01",
            TimeZone = "Africa/Cairo",
            SlotDuration = 1500 // 25 hours
        };

        await Should.ThrowAsync<AbpValidationException>(async () =>
        {
            await _slotAppService.GenerateSlotsAsync(input);
        });
    }

    [Fact]
public async Task Exception_When_Invalid_TimeZone()
{
    var input = new GenerateSlotsInputDto
    {
        StartDate = "2026-06-01",
        EndDate = "2026-06-01",
        TimeZone = "Invalid/TimeZone",
        SlotDuration = 30
    };

    // Change AbpValidationException to UserFriendlyException
    await Should.ThrowAsync<UserFriendlyException>(async () =>
    {
        await _slotAppService.GenerateSlotsAsync(input);
    });
}
    [Fact]
    public async Task Get_Next_Available_Slots_After_Generation()
    {
        var generateInput = new GenerateSlotsInputDto
        {
            StartDate = "2026-06-01",
            EndDate = "2026-06-01",
            TimeZone = "Africa/Cairo",
            SlotDuration = 60
        };
        await _slotAppService.GenerateSlotsAsync(generateInput);

        var getInput = new GetNextAvailableSlotsInputDto
        {
            TimeZone = "Africa/Cairo",
            Count = 10
        };

        var slots = await _slotAppService.GetNextAvailableSlotsAsync(getInput);
        slots.ShouldNotBeNull();
        slots.Count.ShouldBe(10);
        slots.All(s => s.IsBookable).ShouldBeTrue();
    }

    [Fact]
    public async Task Return_Correct_Number_Of_Slots_Based_On_Count()
    {
        var generateInput = new GenerateSlotsInputDto
        {
            StartDate = "2026-06-01",
            EndDate = "2026-06-01",
            TimeZone = "Africa/Cairo",
            SlotDuration = 30
        };
        await _slotAppService.GenerateSlotsAsync(generateInput);

        var slots5 = await _slotAppService.GetNextAvailableSlotsAsync(new GetNextAvailableSlotsInputDto
        {
            TimeZone = "Africa/Cairo",
            Count = 5
        });

        var slots15 = await _slotAppService.GetNextAvailableSlotsAsync(new GetNextAvailableSlotsInputDto
        {
            TimeZone = "Africa/Cairo",
            Count = 15
        });

        slots5.Count.ShouldBe(5);
        slots15.Count.ShouldBe(15);
    }

    [Fact]
    public async Task Convert_Times_Correctly_For_Different_TimeZones()
    {
        var generateInput = new GenerateSlotsInputDto
        {
            StartDate = "2026-06-01",
            EndDate = "2026-06-01",
            TimeZone = "Africa/Cairo",
            SlotDuration = 60
        };
        await _slotAppService.GenerateSlotsAsync(generateInput);

        var cairoSlot = (await _slotAppService.GetNextAvailableSlotsAsync(new GetNextAvailableSlotsInputDto
        {
            TimeZone = "Africa/Cairo",
            Count = 1
        })).First();

        var nySlot = (await _slotAppService.GetNextAvailableSlotsAsync(new GetNextAvailableSlotsInputDto
        {
            TimeZone = "America/New_York",
            Count = 1
        })).First();

        // Assert
        cairoSlot.TimeZone.ShouldBe("Africa/Cairo");
        nySlot.TimeZone.ShouldBe("America/New_York");
        cairoSlot.LocalStartTime.ShouldNotBe(nySlot.LocalStartTime);
    }

    [Fact]
    public async Task Book_Slot_Successfully()
    {
        // Arrange - Generate slots first
        var generateInput = new GenerateSlotsInputDto
        {
            StartDate = "2026-06-01",
            EndDate = "2026-06-01",
            TimeZone = "Africa/Cairo",
            SlotDuration = 60
        };
        await _slotAppService.GenerateSlotsAsync(generateInput);

        var slots = await _slotAppService.GetNextAvailableSlotsAsync(new GetNextAvailableSlotsInputDto
        {
            TimeZone = "Africa/Cairo",
            Count = 1
        });
        var slotId = slots.First().Id;

        await _slotAppService.BookSlotAsync(slotId);

        var availableSlots = await _slotAppService.GetNextAvailableSlotsAsync(new GetNextAvailableSlotsInputDto
        {
            TimeZone = "Africa/Cairo",
            Count = 10
        });
        availableSlots.Any(s => s.Id == slotId).ShouldBeFalse();
    }
}