import { Component } from '@angular/core';
import { MovieFilmComponent } from './movie-film/movie-film.component';

@Component({
  imports: [MovieFilmComponent],
  selector: 'cln-sessions-movies',
  styleUrl: './sessions-movies.component.scss',
  templateUrl: './sessions-movies.component.html',
})
export class SessionsMoviesComponent {
  movie1 = {
    id: '01a0f2e8-f8d1-777a-9753-e7af5949de75',
    title: 'The Batman sfefwefewfwefwefewfewfe',
    sinopse: 'When a sadistic serial killer begins murdering key political figures in Gotham, Batman is forced to investigate the city\'s hidden corruption and question his family\'s involvement.',
    duration: "175",
    releaseDate: new Date('2022-03-04'),
    filmGenres: ['Action', 'Crime', 'Drama'],
    classInd: 'Rated12Plus',
    imagesUrl: {
      posterUrl: "http://localhost:5002/assets/movies/01a0f2e8-f8d1-777a-9753-e7af5949de75/featured-banner.jpeg",
      featuredBanner: "http://localhost:5002/assets/movies/01a0f2e8-f8d1-777a-9753-e7af5949de75/featured-banner.jpeg",
      standardBanner: "http://localhost:5002/assets/movies/01a0f2e8-f8d1-777a-9753-e7af5949de75/featured-banner.jpeg"
    }
  };

  movieClicked(movieId: string) {
    console.log('Movie clicked:', movieId);
  }
}
