import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { AstronautDutyService } from '../../services/astronaut-duty.service/astronaut-duty.service';

@Component({
  selector: 'app-astronaut-duty-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './astronaut-duty-create.html',
  styleUrls: ['./astronaut-duty-create.css']
})
export class AstronautDutyCreate {
  createForm = new FormGroup({
    name: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    rank: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    dutyTitle: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    dutyStartDate: new FormControl('', { nonNullable: true, validators: [Validators.required] })
  });

  isSubmitting = false;
  successMessage = '';
  errorMessage = '';

  constructor(
    private astronautDutyService: AstronautDutyService,
    private cdr: ChangeDetectorRef
  ) {}

  submit(): void {
    if (this.createForm.invalid || this.isSubmitting) {
      this.createForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    this.successMessage = '';
    this.errorMessage = '';

    const formValue = this.createForm.getRawValue();

    const request = {
      name: formValue.name.trim(),
      rank: formValue.rank.trim(),
      dutyTitle: formValue.dutyTitle.trim(),
      dutyStartDate: new Date(formValue.dutyStartDate).toISOString()
    };

    this.astronautDutyService.createAstronautDuty(request)
      .pipe(
        finalize(() => {
          this.isSubmitting = false;
          this.cdr.detectChanges();
        })
      )
      .subscribe({
        next: () => {
          this.successMessage = `Astronaut duty created successfully for "${request.name}".`;
          this.createForm.reset({
            name: '',
            rank: '',
            dutyTitle: '',
            dutyStartDate: ''
          });
        },
        error: (error) => {
          if (error.status === 400) {
            this.errorMessage = error.error?.message || 'Unable to create astronaut duty.';
          } else if (error.status === 0) {
            this.errorMessage = 'The API request was blocked or unavailable.';
          } else {
            this.errorMessage = error.error?.message || 'An error occurred while creating astronaut duty.';
          }
        }
      });
  }

  reset(): void {
    this.createForm.reset({
      name: '',
      rank: '',
      dutyTitle: '',
      dutyStartDate: ''
    });
    this.successMessage = '';
    this.errorMessage = '';
    this.cdr.detectChanges();
  }
}