import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { environment } from '../../../environment/environment';
import { IPaginationResponse } from '../../shared/models/pagination-response.interface';
import { Observable } from 'rxjs';
import { IMovieSelector } from '../models/movie-selector.interface';
import { IMovie } from '../models/movie.interface';
import { IMovieRequest } from '../models/movie-request.interface';

@Service()
export class MoviesService {
  private readonly _httpClient = inject(HttpClient);
  private readonly REQUEST_URL = `${environment.apiUrl}/movies`;

  createMovie(
    movieRequest: IMovieRequest,
    poster: File | null,
    featuredBanner: File | null,
    standardBanner: File | null): Observable<IMovie>
  {
    const request = this.createMovieFormData(movieRequest, poster, featuredBanner, standardBanner);

    return this._httpClient.post<IMovie>(
      this.REQUEST_URL,
      request
    );
  }

  getMovieById(id: string):
    Observable<IMovie>
  {
    return this._httpClient.get<IMovie>(`${this.REQUEST_URL}/${id}`);
  }

  getMovieSelectors(page: number, pageSize: number, title: string | null = null):
    Observable<IPaginationResponse<IMovieSelector>>
  {
    return this._httpClient.post<IPaginationResponse<IMovieSelector>>
      (`${this.REQUEST_URL}/selector?currentPage=${page}&pageSize=${pageSize}`, { title });
  }

  getMovies(page: number, pageSize: number, searchTerm: string | null = null):
    Observable<IPaginationResponse<IMovie>>
  {
    return this._httpClient.post<IPaginationResponse<IMovie>>
      (`${this.REQUEST_URL}/filter?currentPage=${page}&pageSize=${pageSize}`, { searchTerm });
  }

  createMovieFormData(
    movieRequest: IMovieRequest,
    poster: File | null,
    featuredBanner: File | null,
    standardBanner: File | null
  ): FormData
  {
    const formData = new FormData();

    formData.append('MovieDetails.Title', movieRequest.title);
    formData.append('MovieDetails.Sinopse', movieRequest.sinopse);
    formData.append('MovieDetails.Duration', movieRequest.duration);
    formData.append('MovieDetails.ReleaseDate', movieRequest.releaseDate.toISOString());
    formData.append('MovieDetails.ClassInd', movieRequest.classInd);

    for(const [index, genre] of movieRequest.filmGenres.entries()) {
      formData.append(`MovieDetails.FilmGenres[${index}]`, genre);
    }

    if (poster) {
      formData.append('Poster', poster);
    }
    if (featuredBanner) {
      formData.append('FeaturedBanner', featuredBanner);
    }
    if (standardBanner) {
      formData.append('StandardBanner', standardBanner);
    }

    return formData;
  }
}
