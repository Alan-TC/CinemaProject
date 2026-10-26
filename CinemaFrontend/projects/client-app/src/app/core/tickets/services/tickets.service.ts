import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { environment } from '../../../environment/environment';
import { IPaginationResponse } from '../../shared/models/pagination-response.interface';
import { Observable } from 'rxjs';
import { ITicketSummary } from '../models/tickets-summary.interface';
import { ITicket } from '../models/ticket.interface';

@Service()
export class TicketsService {
  private readonly _httpClient = inject(HttpClient);
  private readonly REQUEST_URL = `${environment.apiUrl}/cinematickets`;

  getTickets(page: number, pageSize: number): Observable<IPaginationResponse<ITicketSummary>> {
    return this._httpClient.get<IPaginationResponse<ITicketSummary>>
      (`${this.REQUEST_URL}?currentPage=${page}&pageSize=${pageSize}`);
  }

  getTicketById(id: string): Observable<ITicket> {
    return this._httpClient.get<ITicket>
      (`${this.REQUEST_URL}/${id}`);
  }
}
