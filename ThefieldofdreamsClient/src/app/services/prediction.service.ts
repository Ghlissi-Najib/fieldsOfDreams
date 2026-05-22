import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { Prediction, CreatePredictionRequest, UpdatePredictionRequest } from '../models/prediction';

@Injectable({ providedIn: 'root' })
export class PredictionService {
  private readonly apiUrl = `${environment.apiUrl}/predictions`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Prediction[]> {
    return this.http.get<Prediction[]>(this.apiUrl);
  }

  getById(id: string): Observable<Prediction> {
    return this.http.get<Prediction>(`${this.apiUrl}/${id}`);
  }

  getByUserId(userId: string): Observable<Prediction[]> {
    return this.http.get<Prediction[]>(`${this.apiUrl}/user/${userId}`);
  }

  getByMatchId(matchId: string): Observable<Prediction[]> {
    return this.http.get<Prediction[]>(`${this.apiUrl}/match/${matchId}`);
  }

  create(dto: CreatePredictionRequest): Observable<Prediction> {
    return this.http.post<Prediction>(this.apiUrl, dto);
  }

  update(id: string, dto: UpdatePredictionRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
