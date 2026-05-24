using System;
using NodaTime;
using Volo.Abp.Domain.Entities.Auditing;

namespace Vosita.Slots;

public class Slot : AuditedAggregateRoot<Guid>
{
    public Instant StartInstant { get; set; }
    public Instant EndInstant { get; set; }
    public string CreationTimeZone { get; set; } = default!;
    public SlotStatus Status { get; set; }

    private Slot() { } 

    public Slot(
        Guid id,
        Instant startInstant,
        Instant endInstant,
        string creationTimeZone) : base(id)
    {
        // for the tests 
          if (startInstant >= endInstant)
            throw new ArgumentException("Start time must be before end time", nameof(startInstant));

        if (string.IsNullOrWhiteSpace(creationTimeZone))
            throw new ArgumentException("Time zone is required", nameof(creationTimeZone));

        StartInstant = startInstant;
        EndInstant = endInstant;
        CreationTimeZone = creationTimeZone;
        Status = SlotStatus.Available;
    }

    public void MarkAsBooked()
    {
                if (Status == SlotStatus.Booked) // for the tests 
            throw new InvalidOperationException("Cannot book an already booked slot");

        Status = SlotStatus.Booked;
    }
}