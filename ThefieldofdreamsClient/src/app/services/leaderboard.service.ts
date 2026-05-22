import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { LeaderboardEntry, CreateLeaderboardEntryRequest, UpdateLeaderboardEntryRequest } from '../models/leaderboard';

@Injectable({ providedIn: 'root' })
export class LeaderboardService {
  private readonly apiUrl = `${environment.apiUrl}/leadersboard`;

  constructor(private http: HttpClient) {}

  getTop(type?: string, limit: number = 50): Observable<LeaderboardEntry[]> {
    let params = new HttpParams().set('limit', limit.toString());
    if (type) {
      params = params.set('type', type);
    }
    return this.http.get<LeaderboardEntry[]>(this.apiUrl, { params });
  }

  getById(id: string): Observable<LeaderboardEntry> {
    return this.http.get<LeaderboardEntry>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateLeaderboardEntryRequest): Observable<LeaderboardEntry> {
    return this.http.post<LeaderboardEntry>(this.apiUrl, dto);
  }

  update(id: string, dto: UpdateLeaderboardEntryRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
