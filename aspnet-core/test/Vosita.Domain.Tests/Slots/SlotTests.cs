using System;
using NodaTime;
using Shouldly;
using Vosita.Slots;
using Xunit;

namespace Vosita.Domain.Tests.Slots;

public class SlotTests
{
    private readonly Instant _now = Instant.FromUtc(2026, 5, 24, 10, 0);
    private readonly Instant _later = Instant.FromUtc(2026, 5, 24, 11, 0);

    [Fact]
    public void Create_Valid_Slot()
    {
        var slot = new Slot(
            Guid.NewGuid(),
            _now,
            _later,
            "Africa/Cairo"
        );

        // for the assert
        slot.StartInstant.ShouldBe(_now);
        slot.EndInstant.ShouldBe(_later);
        slot.CreationTimeZone.ShouldBe("Africa/Cairo");
        slot.Status.ShouldBe(SlotStatus.Available);
    }

    [Fact]
    public void Exception_When_StartTime_Is_After_EndTime()
    {
        Should.Throw<ArgumentException>(() =>
        {
            new Slot(
                Guid.NewGuid(),
                _later, 
                _now,
                "Africa/Cairo"
            );
        });
    }

    [Fact]
    public void Exception_When_TimeZone_Is_Empty()
    {
        Should.Throw<ArgumentException>(() =>
        {
            new Slot(
                Guid.NewGuid(),
                _now,
                _later,
                string.Empty
            );
        });
    }

    [Fact]
    public void Mark_Slot_As_Booked()
    {
        // Arrange
        var slot = new Slot(
            Guid.NewGuid(),
            _now,
            _later,
            "Africa/Cairo"
        );

        // Act
        slot.MarkAsBooked();

        // Assert
        slot.Status.ShouldBe(SlotStatus.Booked);
    }

    [Fact]
    public void Exception_When_Booking_Already_Booked_Slot()
    {
        var slot = new Slot(
            Guid.NewGuid(),
            _now,
            _later,
            "Africa/Cairo"
        );
        slot.MarkAsBooked();

        Should.Throw<InvalidOperationException>(() => slot.MarkAsBooked());
    }

    [Fact]
    public void Calculate_Correct_Duration()
    {
        var start = Instant.FromUtc(2026, 5, 24, 10, 0);
        var end = Instant.FromUtc(2026, 5, 24, 10, 30);
        var slot = new Slot(Guid.NewGuid(), start, end, "Africa/Cairo");

        var duration = slot.EndInstant - slot.StartInstant;

        duration.ShouldBe(Duration.FromMinutes(30));
    }
}