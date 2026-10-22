import { Component, DestroyRef, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { SessionsMoviesService } from '../sessions-movies/services/sessions-movies.service';
import { MoviesService } from '../../core/movies/services/movies.service';
import { distinctUntilChanged, switchMap, tap } from 'rxjs';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { IMovie } from '../../core/movies/models/movie.interface';
import { MovieCardComponent } from './movie-card/movie-card.component';

@Component({
  imports: [
    MovieCardComponent
  ],
  selector: 'cln-movies',
  styleUrl: './movies.component.scss',
  templateUrl: './movies.component.html',
})
export class MoviesComponent {
  private readonly _sessionsMoviesService = inject(SessionsMoviesService);
  private readonly _moviesService = inject(MoviesService);
  private readonly _destroyRef = inject(DestroyRef);

  private readonly _router = inject(Router);

  movies = signal<IMovie[]>([]);

  currPage = 1;
  pageSize = 16;
  hasNext = true;
  isLoading = true;

  constructor() {
    toObservable(this._sessionsMoviesService.searchQuery)
      .pipe(
        distinctUntilChanged(),
        tap(() => {
          this.currPage = 1;
          this.hasNext = true;
          this.movies.set([]);
          this.isLoading = true;
        }),
        switchMap(query =>
          this._moviesService.getMovies(1, this.pageSize, query)
        ),
        takeUntilDestroyed()
      )
      .subscribe({
        next: response => {
          this.movies.set(response.items);
          this.hasNext = response.hasNext;
          this.currPage = 2;
          this.isLoading = false;
        },
        error: () => {
          this.isLoading = false;
        }
      });

    this._sessionsMoviesService.onScroll$
    .pipe(takeUntilDestroyed())
    .subscribe(scrollInfo => {
      this.handleScroll(scrollInfo);
    });
  }

  movieClicked(movieId: string): void {
    this._router.navigate(['/movies', movieId]);
  }

  handleScroll(scrollInfo: {scrollTop: number, clientHeight: number, scrollHeight: number}): void {
    const chegouNoFinal =
      scrollInfo.scrollTop + scrollInfo.clientHeight >=
      scrollInfo.scrollHeight - 240;

    if (chegouNoFinal) {
      this.loadMoreMovies();
    }
  }

  loadMoreMovies(): void {
    if (this.isLoading || !this.hasNext) {
      return;
    }

    this.isLoading = true;

    this._moviesService
      .getMovies(
        this.currPage,
        this.pageSize,
        this._sessionsMoviesService.searchQuery()
      )
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe({
        next: response => {
          this.movies.update(current => [
            ...current,
            ...response.items
          ]);

          this.currPage++;
          this.hasNext = response.hasNext;
          this.isLoading = false;
        },
        error: () => {
          this.isLoading = false;
        }
      });
  }
}
