import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AstronautDutyResponse } from '../../models/astronaut-duty.model';

@Injectable({
  providedIn: 'root'
})
export class AstronautDutyService {
  private readonly baseUrl = 'http://localhost:5204';

  constructor(private http: HttpClient) {}

  getAstronautDutiesByName(name: string): Observable<AstronautDutyResponse> {
    return this.http.get<AstronautDutyResponse>(
      `${this.baseUrl}/AstronautDuty/${encodeURIComponent(name)}`
    );
  }
}