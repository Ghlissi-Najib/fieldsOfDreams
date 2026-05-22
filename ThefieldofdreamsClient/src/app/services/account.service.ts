import { inject, Injectable, PLATFORM_ID } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { AccountProfile, UpdateProfileRequest } from '../models/user';
import { AuthService } from './auth.service';

@Injectable({ providedIn: 'root' })
export class AccountService {
  private readonly apiUrl = `${environment.apiUrl}/accounts`;
  private readonly authService = inject(AuthService);

  constructor(private http: HttpClient) {}

  private authHeaders(): { headers: HttpHeaders } {
    const token = this.authService.getToken();
    return {
      headers: token
        ? new HttpHeaders({ Authorization: `Bearer ${token}` })
        : new HttpHeaders(),
    };
  }

  getProfile(): Observable<AccountProfile> {
    return this.http.get<AccountProfile>(`${this.apiUrl}/profile`, this.authHeaders());
  }

  updateProfile(request: UpdateProfileRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/profile`, request, this.authHeaders());
  }
}
