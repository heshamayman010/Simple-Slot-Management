using System;
using Volo.Abp.Application.Dtos;

namespace Vosita.Slots;

public class SlotDto : EntityDto<Guid>
{
    public string LocalStartTime { get; set; } = default!;
    public string LocalEndTime { get; set; } = default!;
    public string TimeZone { get; set; } = default!;
    public bool IsBookable { get; set; }
}