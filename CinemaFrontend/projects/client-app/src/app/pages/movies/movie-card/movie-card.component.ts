import { Component, input, output } from '@angular/core';
import { IMovie } from '../../../core/movies/models/movie.interface';
import { DatePipe } from '@angular/common';
import { ClassIndPipe } from '../../../core/movies/pipes/class-ind-pipe';

@Component({
  imports: [
    DatePipe,
    ClassIndPipe
],
  selector: 'cln-movie-card',
  styleUrl: './movie-card.component.scss',
  templateUrl: './movie-card.component.html',
})
export class MovieCardComponent {
  movie = input.required<IMovie>();
  movieClicked = output<string>();
}
