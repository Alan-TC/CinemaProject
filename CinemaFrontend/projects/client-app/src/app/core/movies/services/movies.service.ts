import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { environment } from '../../../environment/environment';
import { Observable } from 'rxjs';
import { IMovie } from '../models/movie.interface';
import { IPaginationResponse } from '../../shared/models/pagination-response.interface';
import { ISessionSummary } from '../../sessions/models/session-summary.interface';

@Service()
export class MoviesService {
  private readonly _httpClient = inject(HttpClient);
  private readonly REQUEST_URL = `${environment.apiUrl}/movies`;

  getFeaturedMovies(): Observable<IPaginationResponse<IMovie>> {
    return this._httpClient.post<IPaginationResponse<IMovie>>(`${this.REQUEST_URL}/filter`, null);
  }

  getMovies(page: number, pageSize: number, searchTerm: string | null): Observable<IPaginationResponse<IMovie>> {
    return this._httpClient.post<IPaginationResponse<IMovie>>(
      `${this.REQUEST_URL}/filter?currentPage=${page}&pageSize=${pageSize}`,
      { searchTerm });
  }

  getSessions(page: number, pageSize: number, movieId: string): Observable<IPaginationResponse<ISessionSummary>> {
    return this._httpClient.post<IPaginationResponse<ISessionSummary>>
      (`${this.REQUEST_URL}/${movieId}/sessions?currentPage=${page}&pageSize=${pageSize}`, null);
  }

  getMovieById(id: string):
    Observable<IMovie>
  {
    return this._httpClient.get<IMovie>(`${this.REQUEST_URL}/${id}`);
  }
}
