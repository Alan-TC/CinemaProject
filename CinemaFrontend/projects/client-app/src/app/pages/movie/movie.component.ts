import { Component, DestroyRef, inject, input, signal } from '@angular/core';
import { IMovie } from '../../core/movies/models/movie.interface';
import { MoviesService } from '../../core/movies/services/movies.service';
import { DatePipe } from '@angular/common';
import { ClassIndPipe } from '../../core/movies/pipes/class-ind-pipe';
import { ISessionSummary } from '../../core/sessions/models/session-summary.interface';
import { Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { SessionCardComponent } from '../../core/sessions/components/session-card/session-card.component';

@Component({
  imports: [
    ClassIndPipe,
    DatePipe,
    SessionCardComponent
  ],
  selector: 'cln-movie',
  styleUrl: './movie.component.scss',
  templateUrl: './movie.component.html',
})
export class MovieComponent {
  private readonly _movieService = inject(MoviesService);
  private readonly _destroyRef = inject(DestroyRef);

  private readonly _router = inject(Router);

  movieId = input.required<string>();
  movie = signal<IMovie | null>(null);

  sessions = signal<ISessionSummary[]>([]);

  currPage = 1;
  pageSize = 16;
  hasNext = true;
  isLoading = false;

  ngOnInit() {
    this._movieService.getMovieById(this.movieId()).subscribe({
      next: (movie) => {
        this.movie.set(movie);
      },
      error: (error) => {
        console.error('Error fetching movie:', error);
      }
    });

    this.loadMoreSessions();
  }

  sessionClicked(sessionId: string): void {
    this._router.navigate(['/sessions', sessionId]);
  }

  handleScroll(event: Event): void {
    const element = event.target as HTMLElement;

    const chegouNoFinal =
      element.scrollTop + element.clientHeight >=
      element.scrollHeight - 240;

    if (chegouNoFinal) {
      this.loadMoreSessions();
    }
  }

  loadMoreSessions(): void {
    if (this.isLoading || !this.hasNext) {
      return;
    }

    this.isLoading = true;

    this._movieService
      .getSessions(
        this.currPage,
        this.pageSize,
        this.movieId()
      )
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe({
        next: response => {
          this.sessions.update(current => [
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
