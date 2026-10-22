import { Component, input, output } from '@angular/core';
import { IMovie } from '../../../core/movies/models/movie.interface';
import { DatePipe } from '@angular/common';

@Component({
  imports: [
    DatePipe
  ],
  selector: 'cln-movie-card',
  styleUrl: './movie-card.component.scss',
  templateUrl: './movie-card.component.html',
})
export class MovieCardComponent {
  movie = input.required<IMovie>();
  movieClicked = output<string>();
}
