import { Component, input, output } from '@angular/core';
import { IMovie } from '../../../core/movies/models/movie.interface';
import { DatePipe } from '@angular/common';
import { ClassIndPipe } from '../../../core/movies/pipes/class-ind-pipe';

@Component({
  imports: [
    DatePipe,
    ClassIndPipe
  ],
  selector: 'cln-movie-film',
  styleUrl: './movie-film.component.scss',
  templateUrl: './movie-film.component.html',
})
export class MovieFilmComponent {
  movie = input.required<IMovie>();
  movieClicked = output<string>();
}
