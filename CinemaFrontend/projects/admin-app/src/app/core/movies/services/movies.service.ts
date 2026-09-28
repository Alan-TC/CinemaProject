import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { environment } from '../../../environment/environment';
import { IPaginationResponse } from '../../shared/models/pagination-response.interface';
import { Observable } from 'rxjs';
import { IMovieSelector } from '../models/movie-selector.interface';

@Service()
export class MoviesService {
  private readonly _httpClient = inject(HttpClient);
  private readonly REQUEST_URL = `${environment.apiUrl}/movies`;

  getMovieSelectors(page: number, pageSize: number, title: string | null = null): Observable<IPaginationResponse<IMovieSelector>> {
    return this._httpClient.post<IPaginationResponse<IMovieSelector>>
      (`${this.REQUEST_URL}/selector?currentPage=${page}&pageSize=${pageSize}`, { title });
  }
}
