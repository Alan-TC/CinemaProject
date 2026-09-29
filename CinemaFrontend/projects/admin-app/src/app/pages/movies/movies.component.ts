import { JoinPipe } from './../../../../../shared-lib/src/lib/core/pipes/join-pipe';
import { Component, inject, signal } from '@angular/core';
import { SearchInputComponent } from '../../../../../shared-lib/src/lib/io/search-input/search-input.component';
import { OptionsMenuComponent, OptionsMenuContentDirective, OptionsMenuHeaderDirective } from '../../../../../shared-lib/src/lib/pages/options/options-menu/options-menu.component';
import { DatePipe } from '@angular/common';
import { MoviesService } from '../../core/movies/services/movies.service';
import { Router } from '@angular/router';
import { IMovie } from '../../core/movies/models/movie.interface';
import { delay } from 'rxjs';

@Component({
  imports: [
    SearchInputComponent,
    OptionsMenuComponent,
    OptionsMenuHeaderDirective,
    OptionsMenuContentDirective,
    DatePipe,
    JoinPipe
  ],
  selector: 'app-movies',
  styleUrl: './movies.component.scss',
  templateUrl: './movies.component.html',
})
export class MoviesComponent {
  private readonly _moviesService = inject(MoviesService);
  private readonly _router = inject(Router);

  movies = signal<IMovie[]>([]);

  currPage = 1;
  pageSize = 10;
  hasNext = true;
  isLoading = false;

  searchTerm: string | null = null;

  ngOnInit(): void {
    this.loadMoreMovies();
  }

  onScroll(event: Event): void {
    const element = event.target as HTMLElement;

    const chegouNoFinal =
      element.scrollTop + element.clientHeight >=
      element.scrollHeight - 240;

    if (chegouNoFinal) {
      this.loadMoreMovies();
    }
  }

  onSearchClick(event: string) {
    if (event.trim() === '') {
      this.searchTerm = null;
    } else {
      this.searchTerm = event.trim();
    }

    this.resetPagination();
  }

  onMovieClick(id: string) {
    this._router.navigate(['/movies', id]);
  }

  onCreateMovieClick() {
    this._router.navigate(['/movies/create']);
  }

  resetPagination() {
    this.currPage = 1;
    this.hasNext = true;
    this.movies.set([]);
    this.loadMoreMovies();
  }

  loadMoreMovies(): void {
    if (this.isLoading || !this.hasNext) {
      return;
    }
    this.isLoading = true;

    this._moviesService.getMovies(this.currPage, this.pageSize, this.searchTerm)
      .subscribe({
        next: (response) => {
          this.currPage++;

          this.hasNext = response.hasNext;
          this.movies.update((curr) => curr.concat(response.items));
          this.isLoading = false;
        },
        error: (_) => {
          this.isLoading = false;
        }
      });
  }
}
