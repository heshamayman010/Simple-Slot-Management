import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators, FormGroup } from '@angular/forms';
import { SlotService } from '@proxy/slots';
import { AVAILABLE_TIMEZONES } from 'src/app/shared/timezone.constants';
import { ToasterService } from '@abp/ng.theme.shared';

@Component({
  selector: 'app-add-slots',
  standalone: true,  
  imports: [CommonModule, ReactiveFormsModule],  
  templateUrl: './add-slots.component.html',
  styleUrl: './add-slots.component.scss',
})
export class AddSlotsComponent {
  private fb = inject(FormBuilder);
  private slotService = inject(SlotService);
  private toasterService = inject(ToasterService);

timeZones = AVAILABLE_TIMEZONES;
  isLoading = false;
  successMessage = '';
  errorMessage = '';
  
  slotForm: FormGroup = this.fb.group({
    startDate: ['', Validators.required],
    endDate: ['', Validators.required],
    timeZone: ['Africa/Cairo', Validators.required],
    slotDuration: [30, [Validators.required, Validators.min(1), Validators.max(1440)]]
  });
  
  submit() {
    if (this.slotForm.invalid) return;
    
    this.isLoading = true;
    this.successMessage = '';
    this.errorMessage = '';
    
    this.slotService.generateSlots(this.slotForm.value).subscribe({
      next: (result) => {
        this.successMessage = `Successfully generated ${result.totalSlotsCreated} slots!`;
        this.slotForm.reset({
          startDate: '',
          endDate: '',
          timeZone: 'Africa/Cairo',
          slotDuration: 30
        });
        setTimeout(() => this.successMessage = '', 4000);
      },
      error: (err) => {

        setTimeout(() => this.errorMessage = '', 4000);
        this.isLoading=false;
      },
      complete: () => this.isLoading = false
    });
  }
}