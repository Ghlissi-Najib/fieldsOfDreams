import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject, throwError, of } from 'rxjs';
import { map, catchError, tap, shareReplay } from 'rxjs/operators';
import { isPlatformBrowser } from '@angular/common';
import { Router } from '@angular/router';
import { AuthResponse, LoginCredentials, RegisterData, User } from '../models/user';
import { Permission, ROLE_PERMISSIONS, UserRole } from '../models/rbac';
import { environment } from '../environments/environment';



@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = environment.apiUrl; 
  private tokenKey = 'cvc_auth_token';
  private refreshTokenKey = 'cvc_refresh_token';
  private userKey = 'cvc_user';
  private redirectUrlKey = 'cvc_redirect_url';
  
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();
  
  private isBrowser: boolean;
  private refreshTokenTimeout?: any;

  constructor(
    private http: HttpClient,
    private router: Router,
    @Inject(PLATFORM_ID) platformId: Object
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
    this.loadStoredUser();
  }

  /**
   * Load user from localStorage on app start
   */
  private loadStoredUser(): void {
    if (this.isBrowser) {
      const token = this.getToken();
      const userJson = localStorage.getItem(this.userKey) || sessionStorage.getItem(this.userKey);

      if (token && userJson) {
        try {
          const user: User = JSON.parse(userJson);
          // Always re-derive role + identity from the live JWT so stale storage
          // never causes wrong role detection after a re-login or role change.
          const fromToken = this.extractClaimsFromToken(token);
          if (fromToken.role) user.role = fromToken.role;
          if (fromToken.firstName) user.firstName = fromToken.firstName;
          if (fromToken.lastName)  user.lastName  = fromToken.lastName;
          if (fromToken.email)     user.email     = fromToken.email;
          if (fromToken.id)        user.id        = fromToken.id;
          this.currentUserSubject.next(user);
          this.startRefreshTokenTimer();
        } catch (e) {
          this.clearAuthData();
        }
      }
    }
  }

  /** Decode JWT payload and extract well-known claims. */
  private extractClaimsFromToken(token: string): Partial<User & { id: string }> {
    try {
      const parts = token.split('.');
      if (parts.length !== 3) return {};

      // atob may fail on non-ASCII — use the padding-safe variant
      const raw = parts[1].replace(/-/g, '+').replace(/_/g, '/');
      const payload = JSON.parse(atob(raw));

      // ASP.NET Core serialises ClaimTypes.Role to this long key
      const roleRaw: string | string[] | undefined =
        payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ??
        payload['role'];

      const normalized = (
        Array.isArray(roleRaw) ? roleRaw[0] : roleRaw ?? ''
      ).toLowerCase();

      let role: UserRole;
      switch (normalized) {
        case 'admin':    role = 'admin';    break;
        case 'merchant': role = 'merchant'; break;
        case 'partner':  role = 'partner';  break;
        default:         role = 'passenger';
      }

      return {
        id:        payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] ?? payload['sub'] ?? '',
        email:     payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] ?? payload['email'] ?? '',
        firstName: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname']   ?? '',
        lastName:  payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname']      ?? '',
        role,
      };
    } catch {
      return {};
    }
  }

  /**
   * Login with email and password
   */
  login(email: string, password: string, rememberMe: boolean = false): Observable<AuthResponse> {
    const credentials: LoginCredentials = { email, password, rememberMe };
    
    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/login`, credentials)
      .pipe(
        tap(response => this.handleAuthResponse(response, rememberMe)),
        catchError(this.handleError)
      );
  }

  /**
   * Register new user
   */
  register(userData: RegisterData): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/register`, userData)
      .pipe(
        tap(response => {
          if ((response.accessToken || response.token) && !response.requiresVerification) {
            this.handleAuthResponse(response, false);
          }
        }),
        catchError(this.handleError)
      );
  }

  /**
   * Verify email with OTP/token
   */
  verifyEmail(token: string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/verify-email`, { token })
      .pipe(
        tap(response => {
          if (response.user) {
            this.currentUserSubject.next(response.user);
            if (this.isBrowser) {
              localStorage.setItem(this.userKey, JSON.stringify(response.user));
            }
          }
        }),
        catchError(this.handleError)
      );
  }

  /**
   * Request password reset
   */
  requestPasswordReset(email: string): Observable<{ success: boolean; message: string }> {
    return this.http.post<{ success: boolean; message: string }>(`${this.apiUrl}/auth/forgot-password`, { email })
      .pipe(catchError(this.handleError));
  }

  /**
   * Reset password with token
   */
  resetPassword(token: string, newPassword: string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/reset-password`, { token, newPassword })
      .pipe(catchError(this.handleError));
  }

  /**
   * Change password (authenticated)
   */
  changePassword(currentPassword: string, newPassword: string): Observable<{ success: boolean; message: string }> {
    return this.http.post<{ success: boolean; message: string }>(`${this.apiUrl}/auth/change-password`, 
      { currentPassword, newPassword },
      { headers: this.getAuthHeaders() }
    ).pipe(catchError(this.handleError));
  }

  /**
   * Google Social Login
   */
  googleLogin(): Observable<AuthResponse> {
    // For Angular, you'd typically use Angular Social Login or Google Identity Services
    // This is a simplified version
    return new Observable(observer => {
      // @ts-ignore - Google Identity Services
      if (typeof google !== 'undefined') {
        // @ts-ignore
        google.accounts.id.initialize({
          client_id: 'YOUR_GOOGLE_CLIENT_ID',
          callback: (response: any) => {
            this.http.post<AuthResponse>(`${this.apiUrl}/google-auth`, { credential: response.credential })
              .subscribe({
                next: (authResponse) => {
                  this.handleAuthResponse(authResponse, true);
                  observer.next(authResponse);
                  observer.complete();
                },
                error: (err) => observer.error(err)
              });
          }
        });
        // @ts-ignore
        google.accounts.id.prompt();
      } else {
        // Fallback: redirect to OAuth endpoint
        window.location.href = `${this.apiUrl}/google`;
      }
    });
  }

  /**
   * WhatsApp login - request OTP
   */
  whatsappLogin(phone: string): Observable<{ otpSent: boolean; message: string }> {
    return this.http.post<{ otpSent: boolean; message: string }>(`${this.apiUrl}/auth/whatsapp-login`, { phone })
      .pipe(catchError(this.handleError));
  }

  /**
   * Verify WhatsApp OTP
   */
  verifyWhatsAppOtp(phone: string, otp: string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/verify-whatsapp`, { phone, otp })
      .pipe(
        tap(response => this.handleAuthResponse(response, true)),
        catchError(this.handleError)
      );
  }

  /**
   * Refresh token
   */
  refreshToken(): Observable<AuthResponse> {
    const refreshToken = this.getRefreshToken();
    if (!refreshToken) {
      return throwError(() => new Error('No refresh token available'));
    }

    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/refresh`, { refreshToken })
      .pipe(
        tap(response => {
          const token = response.accessToken || response.token;
          if (token) {
            this.setToken(token);
            if (response.refreshToken) {
              this.setRefreshToken(response.refreshToken);
            }
            this.startRefreshTokenTimer();
          }
        }),
        catchError(() => {
          this.logout();
          return throwError(() => new Error('Session expired. Please login again.'));
        })
      );
  }

  /**
   * Logout user
   */
  logout(redirectToLogin: boolean = true): void {
    const token = this.getToken();
    if (token) {
      this.http.post(`${this.apiUrl}/auth/revoke`, {}, { headers: this.getAuthHeaders() })
        .pipe(catchError(() => of(null)))
        .subscribe();
    }
    
    this.clearAuthData();
    this.currentUserSubject.next(null);
    this.stopRefreshTokenTimer();
    
    if (redirectToLogin) {
      this.router.navigate(['/login']);
    }
  }

  /**
   * Update user profile
   */
  updateProfile(profileData: Partial<User>): Observable<User> {
    return this.http.put<User>(`${this.apiUrl}/auth/profile`, profileData, { headers: this.getAuthHeaders() })
      .pipe(
        tap(updatedUser => {
          const currentUser = this.currentUserSubject.value;
          if (currentUser) {
            const newUser = { ...currentUser, ...updatedUser };
            this.currentUserSubject.next(newUser);
            if (this.isBrowser) {
              if (localStorage.getItem(this.tokenKey)) {
                localStorage.setItem(this.userKey, JSON.stringify(newUser));
              } else {
                sessionStorage.setItem(this.userKey, JSON.stringify(newUser));
              }
            }
          }
        }),
        catchError(this.handleError)
      );
  }

  /**
   * Get current user (synchronous)
   */
  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  getCurrentUserRole(): UserRole | null {
    const role = this.currentUserSubject.value?.role;
    return role ?? null;
  }

  /** Read role directly from the stored JWT — useful as fallback when the
   *  cached User object is missing the role field. */
  getRoleFromToken(): UserRole | null {
    const token = this.getToken();
    if (!token) return null;
    return this.extractClaimsFromToken(token).role ?? null;
  }

  hasRole(role: UserRole): boolean {
    return this.getCurrentUserRole() === role;
  }

  hasAnyRole(roles: UserRole[]): boolean {
    const role = this.getCurrentUserRole();
    return role ? roles.includes(role) : false;
  }

  hasPermission(permission: Permission): boolean {
    const role = this.getCurrentUserRole();
    if (!role) return false;
    return ROLE_PERMISSIONS[role].includes(permission);
  }

  /**
   * Check if user is logged in
   */
  isLoggedIn(): boolean {
    const token = this.getToken();
    if (!token) return false;
    
    // Check if token is expired
    try {
      const parts = token.split('.');
      if (parts.length !== 3) {
        console.warn('[AuthService] Malformed token detected; treating as not logged in.');
        return false;
      }
      const payload = JSON.parse(atob(parts[1]));
      const exp = payload.exp * 1000;
      return Date.now() < exp;
    } catch {
      return false;
    }
  }

  /**
   * Get auth token
   */
  getToken(): string | null {
    try {
      return localStorage.getItem(this.tokenKey) || sessionStorage.getItem(this.tokenKey);
    } catch {
      return null; // SSR — no window/storage
    }
  }

  public getRefreshToken(): string | null {
    try {
      return localStorage.getItem(this.refreshTokenKey) || sessionStorage.getItem(this.refreshTokenKey);
    } catch {
      return null;
    }
  }

  /**
   * Set redirect URL (for deep linking after login)
   */
  setRedirectUrl(url: string): void {
    if (this.isBrowser) {
      localStorage.setItem(this.redirectUrlKey, url);
    }
  }

  /**
   * Get and clear redirect URL
   */
  getRedirectUrl(): string | null {
    if (this.isBrowser) {
      const url = localStorage.getItem(this.redirectUrlKey);
      localStorage.removeItem(this.redirectUrlKey);
      return url;
    }
    return null;
  }

  /**
   * Get authentication headers
   */
  private getAuthHeaders(): HttpHeaders {
    const token = this.getToken();
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  /**
   * Handle authentication response
   * Supports both the backend format (accessToken + inline user fields) and
   * any legacy format (token + user object).
   */
  private handleAuthResponse(response: AuthResponse, rememberMe: boolean): void {
    const token = response.accessToken || response.token;
    if (!token) return;

    this.setToken(token, rememberMe);
    if (response.refreshToken) {
      this.setRefreshToken(response.refreshToken, rememberMe);
    }

    // Build a User object from either the nested user or inline fields.
    // Note: accountType, tickets, points and verified are frontend-only game fields
    // not returned by the auth endpoint; safe defaults are used here and can be
    // populated later by a dedicated profile/stats endpoint.
    const resolvedRole = this.resolveRole(response);
    const user: User = response.user ?? {
      id: response.userId ?? '',
      name: [response.firstName, response.lastName].filter(Boolean).join(' '),
      firstName: response.firstName,
      lastName: response.lastName,
      email: response.email ?? '',
      accountType: this.getAccountTypeFromRole(resolvedRole),
      role: resolvedRole,
      merchantId: response.merchantId,
      partnerId: response.partnerId,
      tickets: 0,
      points: 0,
      verified: true,
      createdAt: new Date(),
      updatedAt: new Date()
    };

    // Always override role + identity from JWT claims — more authoritative than body
    const fromToken = this.extractClaimsFromToken(token);
    if (fromToken.role)      user.role      = fromToken.role;
    if (fromToken.firstName) user.firstName = fromToken.firstName;
    if (fromToken.lastName)  user.lastName  = fromToken.lastName;
    if (fromToken.email)     user.email     = fromToken.email;
    if (fromToken.id)        user.id        = fromToken.id;
    user.name = [user.firstName, user.lastName].filter(Boolean).join(' ') || user.name;

    this.currentUserSubject.next(user);
    if (this.isBrowser) {
      const storage = rememberMe ? localStorage : sessionStorage;
      storage.setItem(this.userKey, JSON.stringify(user));
    }
    this.startRefreshTokenTimer();
  }

  private resolveRole(response: AuthResponse): UserRole {
    const normalizedRole = (response.role ?? response.roles?.[0] ?? '').toLowerCase();
    switch (normalizedRole) {
      case 'admin':
        return 'admin';
      case 'merchant':
        return 'merchant';
      case 'partner':
        return 'partner';
      default:
        return 'passenger';
    }
  }

  private getAccountTypeFromRole(role: UserRole): 'business' | 'player' {
    return role === 'merchant' || role === 'partner' ? 'business' : 'player';
  }

  /**
   * Set token in storage
   */
  private setToken(token: string, rememberMe: boolean = true): void {
    if (this.isBrowser) {
      const storage = rememberMe ? localStorage : sessionStorage;
      storage.setItem(this.tokenKey, token);
    }
  }

  /**
   * Set refresh token
   */
  private setRefreshToken(token: string, rememberMe: boolean = true): void {
    if (this.isBrowser) {
      const storage = rememberMe ? localStorage : sessionStorage;
      storage.setItem(this.refreshTokenKey, token);
    }
  }

  /**
   * Clear all auth data
   */
  private clearAuthData(): void {
    if (this.isBrowser) {
      localStorage.removeItem(this.tokenKey);
      localStorage.removeItem(this.refreshTokenKey);
      localStorage.removeItem(this.userKey);
      sessionStorage.removeItem(this.tokenKey);
      sessionStorage.removeItem(this.refreshTokenKey);
    }
  }

  /**
   * Start refresh token timer (5 minutes before expiry)
   */
  private startRefreshTokenTimer(): void {
    const token = this.getToken();
    if (!token) return;
    
    try {
      const parts = token.split('.');
      if (parts.length !== 3) return;
      const payload = JSON.parse(atob(parts[1]));
      const expires = payload.exp * 1000;
      const timeout = expires - Date.now() - (5 * 60 * 1000); // Refresh 5 min before expiry
      
      if (timeout > 0) {
        this.refreshTokenTimeout = setTimeout(() => {
          this.refreshToken().subscribe();
        }, timeout);
      }
    } catch (e) {
      // Invalid token format
    }
  }

  /**
   * Stop refresh token timer
   */
  private stopRefreshTokenTimer(): void {
    if (this.refreshTokenTimeout) {
      clearTimeout(this.refreshTokenTimeout);
      this.refreshTokenTimeout = null;
    }
  }

  /**
   * Error handler
   */
  private handleError(error: any): Observable<never> {
    let errorMessage = 'An error occurred';
    
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = error.error.message;
    } else if (error.error?.message) {
      // Server-side error with message
      errorMessage = error.error.message;
    } else if (error.status === 401) {
      errorMessage = 'Unauthorized. Please login again.';
      this.logout(true);
    } else if (error.status === 403) {
      errorMessage = 'You don\'t have permission to access this resource.';
    } else if (error.status === 409) {
      errorMessage = 'User already exists with this email.';
    } else if (error.status === 429) {
      errorMessage = 'Too many attempts. Please try again later.';
    } else if (error.status === 500) {
      errorMessage = 'Server error. Please try again later.';
    }
    
    console.error('[AuthService] Error:', error);
    return throwError(() => new Error(errorMessage));
  }
}
