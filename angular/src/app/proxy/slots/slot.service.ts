import type { GenerateSlotsResultDto, GetNextAvailableSlotsInputDto, SlotDto,GenerateSlotsInputDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class SlotService {
  private restService = inject(RestService);
  apiName = 'Default';
  

  bookSlot = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/slot/${id}/book-slot`,
    },
    { apiName: this.apiName,...config });
  

  generateSlots = (input: GenerateSlotsInputDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GenerateSlotsResultDto>({
      method: 'POST',
      url: '/api/app/slot/generate-slots',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  getNextAvailableSlots = (input: GetNextAvailableSlotsInputDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SlotDto[]>({
      method: 'GET',
      url: '/api/app/slot/next-available-slots',
      params: { timeZone: input.timeZone, count: input.count },
    },
    { apiName: this.apiName,...config });
}