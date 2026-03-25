import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { finalize } from 'rxjs';
import { AstronautDutyService } from '../../services/astronaut-duty.service/astronaut-duty.service';
import { AstronautDuty, AstronautDutyResponse, PersonAstronaut } from '../../models/astronaut-duty.model';

@Component({
  selector: 'app-astronaut-duty-search',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './astronaut-duty-search.html',
  styleUrls: ['./astronaut-duty-search.css']
})
export class AstronautDutySearch {
  searchName = '';
  person: PersonAstronaut | null = null;
  duties: AstronautDuty[] = [];
  isLoading = false;
  errorMessage = '';
  hasSearched = false;

  constructor(
    private astronautDutyService: AstronautDutyService,
    private cdr: ChangeDetectorRef) {}

  search(): void {
  if (!this.searchName.trim() || this.isLoading) {
    return;
  }

  const trimmedName = this.searchName.trim();

  this.isLoading = true;
  this.errorMessage = '';
  this.hasSearched = true;
  this.person = null;
  this.duties = [];

  this.astronautDutyService.getAstronautDutiesByName(trimmedName)
    .pipe(
      finalize(() => {
        this.isLoading = false;
        this.cdr.detectChanges();
      }) 
    )
    .subscribe({
      next: (response: AstronautDutyResponse) => {
        console.log('next fired', response);
        this.isLoading = false;

        if (response.success === false) {
          this.errorMessage = response.message || `No astronaut duty history found for "${trimmedName}".`;
          return;
        }

        this.person = response.person;
        this.duties = response.astronautDuties ?? [];

        if (!this.person && this.duties.length === 0) {
          this.errorMessage = `No astronaut duty history found for "${trimmedName}".`;
        }
      },
      error: (error) => {
        console.log('error fired', error);
        this.isLoading = false;

        if (error.status === 404) {
          this.errorMessage = `No astronaut duty history found for "${trimmedName}".`;
        } else {
          this.errorMessage = 'An error occurred while retrieving astronaut duties.';
        }
      },
      complete: () => {
        console.log('complete fired');
      }
    });
  }

  onEnterKey(): void {
    this.search();
  }

  formatDate(value: string | null): string {
    if (!value) {
      return 'Current';
    }

    return new Date(value).toLocaleDateString();
  }

  reset(): void {
    this.searchName = '';
    this.person = null;
    this.duties = [];
    this.errorMessage = '';
    this.hasSearched = false;
    this.isLoading = false;
  }

}