import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { Campaign, CreateCampaignRequest, UpdateCampaignRequest, QRCampaign, CreateQRCampaignRequest } from '../models/campaign';

@Injectable({ providedIn: 'root' })
export class CampaignService {
  private readonly apiUrl = `${environment.apiUrl}/campaigns`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Campaign[]> {
    return this.http.get<Campaign[]>(this.apiUrl);
  }

  getById(id: string): Observable<Campaign> {
    return this.http.get<Campaign>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateCampaignRequest): Observable<Campaign> {
    return this.http.post<Campaign>(this.apiUrl, dto);
  }

  update(id: string, dto: UpdateCampaignRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getQRCampaigns(): Observable<QRCampaign[]> {
    return this.http.get<QRCampaign[]>(`${environment.apiUrl}/qrcampaigns`);
  }

  createQRCampaign(dto: CreateQRCampaignRequest): Observable<QRCampaign> {
    return this.http.post<QRCampaign>(`${environment.apiUrl}/qrcampaigns`, dto);
  }

  deleteQRCampaign(id: string): Observable<void> {
    return this.http.delete<void>(`${environment.apiUrl}/qrcampaigns/${id}`);
  }
}
