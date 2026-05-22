import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';

export interface Sponsor {
  id: string;
  name: string;
  logoUrl?: string;
  websiteUrl?: string;
  description?: string;
  isActive: boolean;
  tierLevel: number;
}

export interface CreateSponsorRequest {
  name: string;
  logoUrl?: string;
  websiteUrl?: string;
  description?: string;
  tierLevel: number;
}

export interface UpdateSponsorRequest extends CreateSponsorRequest {
  isActive: boolean;
}

@Injectable({ providedIn: 'root' })
export class SponsorService {
  private readonly apiUrl = `${environment.apiUrl}/sponsors`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Sponsor[]> {
    return this.http.get<Sponsor[]>(this.apiUrl);
  }

  getById(id: string): Observable<Sponsor> {
    return this.http.get<Sponsor>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateSponsorRequest): Observable<Sponsor> {
    return this.http.post<Sponsor>(this.apiUrl, dto);
  }

  update(id: string, dto: UpdateSponsorRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
