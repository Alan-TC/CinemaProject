import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { environment } from '../../../environment/environment';
import { Observable } from 'rxjs';
import { IMovie } from '../models/movie.interface';
import { IPaginationResponse } from '../../shared/models/pagination-response.interface';

@Service()
export class MoviesService {
  private readonly _httpClient = inject(HttpClient);
  private readonly REQUEST_URL = `${environment.apiUrl}/movies`;

  getFeaturedMovies(): Observable<IPaginationResponse<IMovie>> {
    return this._httpClient.post<IPaginationResponse<IMovie>>(`${this.REQUEST_URL}/filter`, null);
  }

  getMovieById(id: string):
    Observable<IMovie>
  {
    return this._httpClient.get<IMovie>(`${this.REQUEST_URL}/${id}`);
  }
}
