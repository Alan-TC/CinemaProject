import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { environment } from '../../../environment/environment';
import { IPaginationResponse } from '../../shared/models/pagination-response.interface';
import { Observable } from 'rxjs';
import { ITheaterSelector } from '../models/theater-selector.interface';

@Service()
export class TheatersService {
  private readonly _httpClient = inject(HttpClient);
  private readonly REQUEST_URL = `${environment.apiUrl}/theaterlayout`;

  getTheaterSelectors(page: number, pageSize: number, tag: string | null = null): Observable<IPaginationResponse<ITheaterSelector>> {
    return this._httpClient.post<IPaginationResponse<ITheaterSelector>>
      (`${this.REQUEST_URL}/selector?currentPage=${page}&pageSize=${pageSize}`, { tag });
  }
}
