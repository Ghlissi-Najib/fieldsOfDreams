import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { Referral, CreateReferralRequest } from '../models/referral';

@Injectable({ providedIn: 'root' })
export class ReferralService {
  private readonly apiUrl = `${environment.apiUrl}/referrals`;

  constructor(private http: HttpClient) {}

  getByReferrer(userId: string): Observable<Referral[]> {
    return this.http.get<Referral[]>(`${this.apiUrl}/referrer/${userId}`);
  }

  getByReferred(userId: string): Observable<Referral[]> {
    return this.http.get<Referral[]>(`${this.apiUrl}/referred/${userId}`);
  }

  create(dto: CreateReferralRequest): Observable<Referral> {
    return this.http.post<Referral>(this.apiUrl, dto);
  }
}
