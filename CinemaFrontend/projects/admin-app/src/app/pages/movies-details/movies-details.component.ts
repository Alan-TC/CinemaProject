import { Component, inject, input } from '@angular/core';
import { OptionsMenuComponent, OptionsMenuContentDirective, OptionsMenuHeaderDirective } from '../../../../../shared-lib/src/lib/pages/options/options-menu/options-menu.component';
import { AsyncPipe, DatePipe } from '@angular/common';
import { IMovie } from '../../core/movies/models/movie.interface';
import { Observable, switchMap } from 'rxjs';
import { MoviesService } from '../../core/movies/services/movies.service';
import { toObservable } from '@angular/core/rxjs-interop';
import { JoinPipe } from '../../../../../shared-lib/src/lib/core/pipes/join-pipe';

@Component({
  imports: [
    AsyncPipe,
    JoinPipe,
    DatePipe,
    OptionsMenuComponent,
    OptionsMenuHeaderDirective,
    OptionsMenuContentDirective
  ],
  selector: 'app-movies-details',
  styleUrl: './movies-details.component.scss',
  templateUrl: './movies-details.component.html',
})
export class MoviesDetailsComponent {
  private readonly _moviesService = inject(MoviesService);

  activatedMenu: 'left' | 'right' = 'left';

  id = input.required<string>();

  movie$: Observable<IMovie> = toObservable(this.id)
    .pipe(
      switchMap(id => this._moviesService.getMovieById(id))
    );

  classIndTranslateMap: Record<string, string> = {
    'All': 'Livre',
    'Rated6Plus': '6 anos',
    'Rated10Plus': '10 anos',
    'Rated12Plus': '12 anos',
    'Rated14Plus': '14 anos',
    'Rated16Plus': '16 anos',
    'Rated18Plus': '18 anos'
  }

  selectMenu(menu: 'left' | 'right') {
    this.activatedMenu = menu;
  }
}
