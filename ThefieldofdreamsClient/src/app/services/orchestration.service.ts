import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of, tap } from 'rxjs';
import { environment } from '../environments/environment';
import { OrchestrationState } from '../models/orchestration';

@Injectable({ providedIn: 'root' })
export class OrchestrationService {
  private readonly http = inject(HttpClient);

  private readonly _state = signal<OrchestrationState | null>(null);
  readonly state = this._state.asReadonly();

  load(): Observable<OrchestrationState | null> {
    return this.http
      .get<OrchestrationState>(`${environment.apiUrl}/orchestration/state`)
      .pipe(
        tap(s => this._state.set(s)),
        catchError(() => {
          // Non-blocking: orchestration state is an enhancement, not a hard dependency
          return of(null);
        })
      );
  }

  isFlowEnabled(flow: string): boolean {
    const flows = this._state()?.enabledFlows;
    if (!flows) return true; // not yet loaded = permissive
    return flows[flow] ?? false;
  }

  isRewardVisible(rewardId: string): boolean {
    const ids = this._state()?.visibleRewardIds;
    if (!ids || ids.length === 0) return true;
    return ids.includes(rewardId);
  }

  isQRCodeValid(qrCodeId: string): boolean {
    const ids = this._state()?.validQRCodeIds;
    if (!ids || ids.length === 0) return true;
    return ids.includes(qrCodeId);
  }

  getActiveCampaigns() {
    return this._state()?.activeCampaigns ?? [];
  }
}
