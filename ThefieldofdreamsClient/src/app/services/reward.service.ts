import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { Reward, CreateRewardRequest, UpdateRewardRequest, UserReward, ClaimRewardRequest } from '../models/reward';

@Injectable({ providedIn: 'root' })
export class RewardService {
  private readonly apiUrl = `${environment.apiUrl}/rewards`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Reward[]> {
    return this.http.get<Reward[]>(this.apiUrl);
  }

  getById(id: string): Observable<Reward> {
    return this.http.get<Reward>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateRewardRequest): Observable<Reward> {
    return this.http.post<Reward>(this.apiUrl, dto);
  }

  update(id: string, dto: UpdateRewardRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getUserRewards(userId: string): Observable<UserReward[]> {
    return this.http.get<UserReward[]>(`${environment.apiUrl}/userrewards/user/${userId}`);
  }

  claimReward(dto: ClaimRewardRequest): Observable<UserReward> {
    return this.http.post<UserReward>(`${environment.apiUrl}/userrewards/claim`, dto);
  }

  validatePromoCode(usageCode: string): Observable<{ success: boolean; message: string }> {
    return this.http.post<{ success: boolean; message: string }>(
      `${environment.apiUrl}/userrewards/validate`,
      { usageCode }
    );
  }
}
