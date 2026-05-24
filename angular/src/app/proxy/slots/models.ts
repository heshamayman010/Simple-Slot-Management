import type { EntityDto } from '@abp/ng.core';

export interface GenerateSlotsResultDto {
  totalSlotsCreated?: number;
}

export interface GetNextAvailableSlotsInputDto {
  timeZone?: string;
  count?: number;
}

export interface SlotDto extends EntityDto<string> {
  localStartTime?: string;
  localEndTime?: string;
  timeZone?: string;
  isBookable?: boolean;
}


export interface GenerateSlotsInputDto {
  startDate?: string;   
  endDate?: string;    
  timeZone?: string;
  slotDuration?: number;
}