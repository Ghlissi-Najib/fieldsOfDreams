import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { Mission, CreateMissionRequest, UpdateMissionRequest, UserMission, UpdateUserMissionProgressRequest } from '../models/mission';

@Injectable({ providedIn: 'root' })
export class MissionService {
  private readonly apiUrl = `${environment.apiUrl}/missions`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Mission[]> {
    return this.http.get<Mission[]>(this.apiUrl);
  }

  getById(id: string): Observable<Mission> {
    return this.http.get<Mission>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateMissionRequest): Observable<Mission> {
    return this.http.post<Mission>(this.apiUrl, dto);
  }

  update(id: string, dto: UpdateMissionRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getUserMissions(userId: string): Observable<UserMission[]> {
    return this.http.get<UserMission[]>(`${environment.apiUrl}/usermissions/user/${userId}`);
  }

  assignMission(userId: string, missionId: string): Observable<UserMission> {
    return this.http.post<UserMission>(`${environment.apiUrl}/usermissions`, { userId, missionId });
  }

  updateProgress(userMissionId: string, dto: UpdateUserMissionProgressRequest): Observable<void> {
    return this.http.put<void>(`${environment.apiUrl}/usermissions/${userMissionId}`, dto);
  }
}
