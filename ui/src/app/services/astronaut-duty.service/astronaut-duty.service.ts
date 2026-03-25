import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AstronautDutyResponse, CreateAstronautDutyRequest, CreateAstronautDutyResponse } from '../../models/astronaut-duty.model';

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

  createAstronautDuty(request: CreateAstronautDutyRequest): Observable<CreateAstronautDutyResponse> {
    return this.http.post<CreateAstronautDutyResponse>(`${this.baseUrl}/AstronautDuty/`, request);
  }

}