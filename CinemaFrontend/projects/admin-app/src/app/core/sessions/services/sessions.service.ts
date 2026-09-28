import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { environment } from '../../../environment/environment';
import { ISessionSummary } from '../models/session-summary.interface';
import { IPaginationResponse } from '../../shared/models/pagination-response.interface';
import { Observable } from 'rxjs';
import { ISessionRequest } from '../models/session-request.interface';
import { ISessionResponse } from '../models/session-response.interface';

@Service()
export class SessionsService {
  private readonly _httpClient = inject(HttpClient);
  private readonly REQUEST_URL = `${environment.apiUrl}/sessions`;

  createSession(session: ISessionRequest): Observable<ISessionResponse> {
    return this._httpClient.post<ISessionResponse>
      (`${this.REQUEST_URL}`, session);
  }

  getSessionById(id: string): Observable<ISessionResponse> {
    return this._httpClient.get<ISessionResponse>
      (`${this.REQUEST_URL}/${id}`);
  }

  getSessions(page: number, pageSize: number, code: string | null = null): Observable<IPaginationResponse<ISessionSummary>> {
    return this._httpClient.post<IPaginationResponse<ISessionSummary>>
      (`${this.REQUEST_URL}/filter?currentPage=${page}&pageSize=${pageSize}`, { code });
  }
}
