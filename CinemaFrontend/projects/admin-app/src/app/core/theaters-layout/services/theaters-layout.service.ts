import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { environment } from '../../../environment/environment';
import { IPaginationResponse } from '../../shared/models/pagination-response.interface';
import { Observable } from 'rxjs';
import { ITheaterLayoutSelector } from '../models/theater-layout-selector.interface';
import { ITheaterLayout } from '../models/theater-layout.interface';
import { ITheaterLayoutRequest } from '../models/theater-layout-request.interface';

@Service()
export class TheatersLayoutService {
  private readonly _httpClient = inject(HttpClient);
  private readonly REQUEST_URL = `${environment.apiUrl}/theaterlayout`;

  createTheaterLayout(theaterLayout: ITheaterLayoutRequest): Observable<ITheaterLayout> {
    return this._httpClient.post<ITheaterLayout>(this.REQUEST_URL, theaterLayout);
  }

  getTheatersLayout(
    page: number,
    pageSize: number,
    tag: string | null = null):
    Observable<IPaginationResponse<ITheaterLayout>>
  {
    return this._httpClient.post<IPaginationResponse<ITheaterLayout>>
      (`${this.REQUEST_URL}/filter?currentPage=${page}&pageSize=${pageSize}`, { searchTerm: tag });
  }

  getTheaterSelectors(page: number, pageSize: number, tag: string | null = null): Observable<IPaginationResponse<ITheaterLayoutSelector>> {
    return this._httpClient.post<IPaginationResponse<ITheaterLayoutSelector>>
      (`${this.REQUEST_URL}/selector?currentPage=${page}&pageSize=${pageSize}`, { tag });
  }
}
