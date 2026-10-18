import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { environment } from '../../../environment/environment';
import { Observable } from 'rxjs';
import { IMovie } from '../models/movie.interface';

@Service()
export class MoviesService {
  private readonly _httpClient = inject(HttpClient);
  private readonly REQUEST_URL = `${environment.apiUrl}/movies`;

  getMovieById(id: string):
    Observable<IMovie>
  {
    return this._httpClient.get<IMovie>(`${this.REQUEST_URL}/${id}`);
  }
}
