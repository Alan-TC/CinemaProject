import { Component, inject } from '@angular/core';
import { OptionsMenuComponent, OptionsMenuContentDirective, OptionsMenuHeaderDirective } from '../../../../../shared-lib/src/lib/pages/options/options-menu/options-menu.component';
import { InfiniteSelectorComponent } from '../../../../../shared-lib/src/lib/io/infinite-selector/infinite-selector.component';
import { MoviesService } from '../../core/movies/services/movies.service';
import { map, Observable } from 'rxjs';
import { TheatersService } from '../../core/theater/services/theaters.service';
import { ButtonComponent } from '../../../../../shared-lib/src/lib/io/button/button.component';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { IMovieSelector } from '../../core/movies/models/movie-selector.interface';
import { ITheaterSelector } from '../../core/theater/models/theater-selector.interface';
import { ISessionRequest } from '../../core/sessions/models/session-request.interface';
import { SessionsService } from '../../core/sessions/services/sessions.service';
import { Router } from '@angular/router';

@Component({
  imports: [
    OptionsMenuComponent,
    OptionsMenuHeaderDirective,
    OptionsMenuContentDirective,
    InfiniteSelectorComponent,
    ButtonComponent,
    ReactiveFormsModule
  ],
  selector: 'app-sessions-create',
  styleUrl: './sessions-create.component.scss',
  templateUrl: './sessions-create.component.html',
})
export class SessionsCreateComponent {
  private readonly _router = inject(Router);

  private readonly _sessionsService = inject(SessionsService);
  private readonly _moviesService = inject(MoviesService);
  private readonly _theatersService = inject(TheatersService);

  thisSelectedMovie: IMovieSelector | null = null;
  thisSelectedTheater: ITheaterSelector | null = null;

  createSessionForm = new FormGroup({
    startSalesAt: new FormControl(null, [Validators.required]),
    startsAt: new FormControl(null, [Validators.required]),
    endsAt: new FormControl(null, [Validators.required]),
    pricing: new FormControl(null, [Validators.required]),
  });

  onMovieSelected(movie: IMovieSelector) {
    this.thisSelectedMovie = movie;
  }

  onTheaterSelected(theater: ITheaterSelector) {
    this.thisSelectedTheater = theater;
  }

  onSubmit() {
    if (this.createSessionForm.invalid) {
      this.createSessionForm.setErrors({validationError: true});
      return;
    }
    if (!this.thisSelectedMovie || !this.thisSelectedTheater) {
      this.createSessionForm.setErrors({validationError: true});
      return;
    }

    const createRequest: ISessionRequest = {
      startsAt: new Date(this.createSessionForm.value.startsAt!),
      endsAt: new Date(this.createSessionForm.value.endsAt!),
      startTicketSalesAt: new Date(this.createSessionForm.value.startSalesAt!),
      pricing: this.createSessionForm.value.pricing!,
      theaterLayoutId: this.thisSelectedTheater.id,
      movieId: this.thisSelectedMovie.id
    };

    this._sessionsService.createSession(createRequest)
      .subscribe({
        next: (response) => {
          this._router.navigate(['/sessions', response.id]);
        },
        error: (error) => {
          if (error.status === 400) {
            if (error.error) {
              this.createSessionForm.setErrors({serverError: error.error.errorMessages.join(', ')});
              return;
            }
          }
          this.createSessionForm.setErrors({unknownError: true});
        }
      })
  }

  moviesSelectorPaginationFunction(
    page: number, pageSize: number, searchByDisplayId: string | null)
    : Observable<{hasNext: boolean, itemsOptions: any[]}>{
      return this._moviesService.getMovieSelectors(page, pageSize, searchByDisplayId)
        .pipe(
          map((response) => ({
            hasNext: response.hasNext,
            itemsOptions: response.items
          }))
        );
  }

    theatersSelectorPaginationFunction(
      page: number, pageSize: number, searchByDisplayId: string | null)
      : Observable<{hasNext: boolean, itemsOptions: any[]}>{
        return this._theatersService.getTheaterSelectors(page, pageSize, searchByDisplayId)
          .pipe(
            map((response) => ({
              hasNext: response.hasNext,
              itemsOptions: response.items
            }))
          );
    }
}
