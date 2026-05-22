import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { Match, CreateMatchRequest, UpdateMatchRequest } from '../models/match';

@Injectable({ providedIn: 'root' })
export class MatchService {
  private readonly apiUrl = `${environment.apiUrl}/matchs`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Match[]> {
    return this.http.get<Match[]>(this.apiUrl);
  }

  getById(id: string): Observable<Match> {
    return this.http.get<Match>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateMatchRequest): Observable<Match> {
    return this.http.post<Match>(this.apiUrl, dto);
  }

  update(id: string, dto: UpdateMatchRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
