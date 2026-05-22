import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { AuthService } from './auth.service';
import {
  AdminDashboardDto,
  DashboardStats,
  MerchantDashboardDto,
  PassengerDashboardDto,
} from '../models/dashboard';

@Injectable({ providedIn: 'root' })
export class DashboardService {
  private readonly apiUrl = `${environment.apiUrl}/Dashboard`;

  constructor(
    private readonly http: HttpClient,
    private readonly authService: AuthService
  ) {}

  private authOptions(): { headers: HttpHeaders } {
    const token = this.authService.getToken();
    return {
      headers: token
        ? new HttpHeaders({ Authorization: `Bearer ${token}` })
        : new HttpHeaders(),
    };
  }

  getSummary(): Observable<DashboardStats> {
    return this.http.get<DashboardStats>(`${this.apiUrl}/summary`, this.authOptions());
  }

  getAdminDashboard(): Observable<AdminDashboardDto> {
    return this.http.get<AdminDashboardDto>(`${this.apiUrl}/admin`, this.authOptions());
  }

  getPassengerDashboard(): Observable<PassengerDashboardDto> {
    return this.http.get<PassengerDashboardDto>(`${this.apiUrl}/passenger`, this.authOptions());
  }

  getMerchantDashboard(): Observable<MerchantDashboardDto> {
    return this.http.get<MerchantDashboardDto>(`${this.apiUrl}/merchant`, this.authOptions());
  }

  getPartnerDashboard(): Observable<AdminDashboardDto> {
    return this.http.get<AdminDashboardDto>(`${this.apiUrl}/partner`, this.authOptions());
  }
}
