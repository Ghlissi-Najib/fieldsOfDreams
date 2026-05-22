import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { QRCode, CreateQRCodeRequest, UpdateQRCodeRequest, QRScan, CreateQRScanRequest } from '../models/qr-code';

@Injectable({ providedIn: 'root' })
export class QRCodeService {
  private readonly apiUrl = `${environment.apiUrl}/qrcodes`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<QRCode[]> {
    return this.http.get<QRCode[]>(this.apiUrl);
  }

  getById(id: string): Observable<QRCode> {
    return this.http.get<QRCode>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateQRCodeRequest): Observable<QRCode> {
    return this.http.post<QRCode>(this.apiUrl, dto);
  }

  update(id: string, dto: UpdateQRCodeRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  scan(dto: CreateQRScanRequest): Observable<QRScan> {
    return this.http.post<QRScan>(`${environment.apiUrl}/qrscans`, dto);
  }

  getScansByUser(userId: string): Observable<QRScan[]> {
    return this.http.get<QRScan[]>(`${environment.apiUrl}/qrscans/user/${userId}`);
  }
}
