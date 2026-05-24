using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Vosita.Slots;

public interface ISlotAppService : IApplicationService
{
    Task<GenerateSlotsResultDto> GenerateSlotsAsync(GenerateSlotsInputDto input);
    Task<List<SlotDto>> GetNextAvailableSlotsAsync(GetNextAvailableSlotsInputDto input);
    Task BookSlotAsync(Guid id); 
}