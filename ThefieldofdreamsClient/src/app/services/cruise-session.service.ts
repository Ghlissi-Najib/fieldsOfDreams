import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { CruiseSession, CreateCruiseSessionRequest, UpdateCruiseSessionRequest } from '../models/cruise-session';
import { LeaderboardEntry } from '../models/leaderboard';

@Injectable({ providedIn: 'root' })
export class CruiseSessionService {
  private readonly apiUrl = `${environment.apiUrl}/cruisesessions`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<CruiseSession[]> {
    return this.http.get<CruiseSession[]>(this.apiUrl);
  }

  getById(id: string): Observable<CruiseSession> {
    return this.http.get<CruiseSession>(`${this.apiUrl}/${id}`);
  }

  getActive(): Observable<CruiseSession[]> {
    return this.http.get<CruiseSession[]>(`${this.apiUrl}/active`);
  }

  getByPort(portLocationId: string): Observable<CruiseSession[]> {
    return this.http.get<CruiseSession[]>(`${this.apiUrl}/port/${portLocationId}`);
  }

  getShipLeaderboard(cruiseSessionId: string): Observable<LeaderboardEntry[]> {
    return this.http.get<LeaderboardEntry[]>(`${this.apiUrl}/${cruiseSessionId}/leaderboard`);
  }

  create(dto: CreateCruiseSessionRequest): Observable<CruiseSession> {
    return this.http.post<CruiseSession>(this.apiUrl, dto);
  }

  update(id: string, dto: UpdateCruiseSessionRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
