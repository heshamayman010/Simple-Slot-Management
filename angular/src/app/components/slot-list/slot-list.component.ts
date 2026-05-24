import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { GetNextAvailableSlotsInputDto, SlotDto } from '@proxy/slots/models';
import { SlotService } from '@proxy/slots';
import { AVAILABLE_TIMEZONES } from 'src/app/shared/timezone.constants';
import { ToasterService } from '@abp/ng.theme.shared';
@Component({
  selector: 'app-slot-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './slot-list.component.html',
  styleUrl: './slot-list.component.scss',
})
export class SlotListComponent implements OnInit {
  private slotService = inject(SlotService);
  private toasterService = inject(ToasterService);

  slots: SlotDto[] = [];
  selectedTimeZone = 'Africa/Cairo';
  timeZones = AVAILABLE_TIMEZONES;
  isLoading = false;
  errorMessage = '';
  CountOfSlots: number = 20;
  selectedSlotId: string = '';
  isBooking: boolean = false;
  showConfirmModal: boolean = false;

  ngOnInit() {
    this.loadSlots();
  }

  loadSlots() {
    this.isLoading = true;
    this.errorMessage = '';

    const input: GetNextAvailableSlotsInputDto = {
      timeZone: this.selectedTimeZone,
      count: this.CountOfSlots,
    };

    this.slotService.getNextAvailableSlots(input).subscribe({
      next: data => {
        this.slots = data;
      },
      error: err => {
        this.errorMessage = ` Failed to load slots: ${err.error?.message || err.message}`;
        setTimeout(() => (this.errorMessage = ''), 5000);
        this.slots = [];
      },
      complete: () => {
        this.isLoading = false;
      },
    });
  }

  onTimeZoneChange() {
    this.loadSlots();
  }

  onFilterChange(): void {
    if (this.CountOfSlots < 1) this.CountOfSlots = 20;
    this.loadSlots();
  }

  // for the booking modal
  openConfirmModal(slotId: string) {
    this.selectedSlotId = slotId;
    this.showConfirmModal = true;
  }

  closeModal() {
    this.showConfirmModal = false;
    this.selectedSlotId = '';
  }

  confirmBooking() {
    this.isBooking = true;

    this.slotService.bookSlot(this.selectedSlotId).subscribe({
      next: () => {
        this.toasterService.success('Slot booked successfully!', 'Success');
        this.closeModal();
        this.loadSlots();
      },
      error: err => {
        this.toasterService.error(err.error?.message || 'Failed to book slot', 'Error');
        this.closeModal();
      },
      complete: () => {
        this.isBooking = false;
      },
    });
  }

  bookSlot(slotId: string) {
    this.openConfirmModal(slotId);
  }

}
