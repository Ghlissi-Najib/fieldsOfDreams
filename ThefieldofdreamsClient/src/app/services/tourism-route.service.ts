import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { TourismRoute, CreateTourismRouteRequest, UpdateTourismRouteRequest, UserRouteCompletion } from '../models/tourism-route';

@Injectable({ providedIn: 'root' })
export class TourismRouteService {
  private readonly apiUrl = `${environment.apiUrl}/tourismroutes`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<TourismRoute[]> {
    return this.http.get<TourismRoute[]>(this.apiUrl);
  }

  getById(id: string): Observable<TourismRoute> {
    return this.http.get<TourismRoute>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateTourismRouteRequest): Observable<TourismRoute> {
    return this.http.post<TourismRoute>(this.apiUrl, dto);
  }

  update(id: string, dto: UpdateTourismRouteRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getUserCompletions(userId: string): Observable<UserRouteCompletion[]> {
    return this.http.get<UserRouteCompletion[]>(`${environment.apiUrl}/userroutecompletions/user/${userId}`);
  }

  startRoute(routeId: string, userId: string): Observable<UserRouteCompletion> {
    return this.http.post<UserRouteCompletion>(`${environment.apiUrl}/userroutecompletions/start`, { routeId, userId });
  }
}
