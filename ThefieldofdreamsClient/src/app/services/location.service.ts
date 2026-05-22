import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { Location, CreateLocationRequest, UpdateLocationRequest } from '../models/location';

@Injectable({ providedIn: 'root' })
export class LocationService {
  private readonly apiUrl = `${environment.apiUrl}/locations`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Location[]> {
    return this.http.get<Location[]>(this.apiUrl);
  }

  getById(id: string): Observable<Location> {
    return this.http.get<Location>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateLocationRequest): Observable<Location> {
    return this.http.post<Location>(this.apiUrl, dto);
  }

  update(id: string, dto: UpdateLocationRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
