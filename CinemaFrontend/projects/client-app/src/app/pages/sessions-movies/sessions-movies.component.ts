import { SearchInputComponent } from './../../../../../shared-lib/src/lib/io/search-input/search-input.component';
import { IMovie } from './../../core/movies/models/movie.interface';
import { afterNextRender, Component, DestroyRef, inject, signal } from '@angular/core';
import { MovieFilmComponent } from './movie-film/movie-film.component';
import { MoviesService } from '../../core/movies/services/movies.service';
import { RouterModule , RouterOutlet, RouterLink, RouterLinkActive, Router } from '@angular/router';
import { SessionsMoviesService } from './services/sessions-movies.service';

@Component({
  imports: [
    MovieFilmComponent,
    RouterModule,
    RouterOutlet,
    SearchInputComponent,
    RouterLink,
    RouterLinkActive
  ],
  providers: [SessionsMoviesService],
  selector: 'cln-sessions-movies',
  styleUrl: './sessions-movies.component.scss',
  templateUrl: './sessions-movies.component.html',
})
export class SessionsMoviesComponent {
  private destroyRef = inject(DestroyRef);
  private readonly _moviesService = inject(MoviesService);
  private readonly _sessionsMoviesService = inject(SessionsMoviesService);

  private readonly _router = inject(Router);

  firstFeaturedMovieClass = signal<'left-film' | 'middle-film' | 'right-film'>('middle-film');
  secondFeaturedMovieClass = signal<'left-film' | 'middle-film' | 'right-film'>('right-film');
  thirdFeaturedMovieClass = signal<'left-film' | 'middle-film' | 'right-film'>('left-film');

  private _emptyMovie: IMovie = {
    id: '',
    title: '',
    sinopse: '',
    duration: '',
    releaseDate: new Date(),
    filmGenres: [],
    classInd: '',
    imagesUrl: {
      posterUrl: null,
      featuredBanner: null,
      standardBanner: null
    }
  };

  movie1 = signal<IMovie>(this._emptyMovie);
  movie2 = signal<IMovie>(this._emptyMovie);
  movie3 = signal<IMovie>(this._emptyMovie);

  constructor() {
    afterNextRender(() => {
      const intervalId = setInterval(() => {
        this.firstFeaturedMovieClass.update(c => this.nextFeaturedMovieClass(c));
        this.secondFeaturedMovieClass.update(c => this.nextFeaturedMovieClass(c));
        this.thirdFeaturedMovieClass.update(c => this.nextFeaturedMovieClass(c));
      }, 8050);

      this.destroyRef.onDestroy(() => clearInterval(intervalId));
    });
  }

  ngOnInit() {
    this._moviesService.getFeaturedMovies().subscribe({
      next: (response) => {
        if (response.items.length >= 3) {
          this.movie1.set(response.items[0]);
          this.movie2.set(response.items[1]);
          this.movie3.set(response.items[2]);
        }
        else if (response.items.length === 2) {
          this.movie1.set(response.items[0]);
          this.movie2.set(response.items[1]);
          this.movie3.set(response.items[0]);
        }
        else if (response.items.length === 1) {
          this.movie1.set(response.items[0]);
          this.movie2.set(response.items[0]);
          this.movie3.set(response.items[0]);
        }
      }
    });
  }

  onScroll(event: Event) {
    const element = event.target as HTMLElement;

    const scrollInfo = {
      scrollTop: element.scrollTop,
      clientHeight: element.clientHeight,
      scrollHeight: element.scrollHeight
    };

    this._sessionsMoviesService.emitOnScroll(scrollInfo);
  }

  onSearchClick(searchQuery: string) {
    let searchTerm = searchQuery.trim() || null;
    this._sessionsMoviesService.searchQuery.set(searchTerm);
  }

  nextFeaturedMovieClass(currClass: 'left-film' | 'middle-film' | 'right-film')
    : 'left-film' | 'middle-film' | 'right-film' {
      switch (currClass) {
        case 'left-film':
          return 'right-film';
        case 'middle-film':
          return 'left-film';
        case 'right-film':
          return 'middle-film';
      }
  }

  movieClicked(movieId: string) {
    this._router.navigate(['/movies', movieId]);
  }
}
