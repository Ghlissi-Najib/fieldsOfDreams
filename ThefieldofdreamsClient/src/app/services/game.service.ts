import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';

@Injectable({ providedIn: 'root' })
export class GameService {
  private readonly apiUrl = `${environment.apiUrl}`;

  constructor(private http: HttpClient) {}

  getLeaderboard(type?: string, limit = 50): Observable<any[]> {
    const params: Record<string, string> = { limit: limit.toString() };
    if (type) params['type'] = type;
    return this.http.get<any[]>(`${this.apiUrl}/leadersboard`, { params });
  }

  getMatches(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/matchs`);
  }

  getPredictions(userId: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/predictions/user/${userId}`);
  }
}
