import { MultipleSelectorComponent } from './../../../../../shared-lib/src/lib/io/multiple-selector/multiple-selector.component';
import { ImageDropComponent } from './../../../../../shared-lib/src/lib/io/image-drop/image-drop.component';
import { Component, inject } from '@angular/core';
import { OptionsMenuComponent, OptionsMenuContentDirective, OptionsMenuHeaderDirective } from '../../../../../shared-lib/src/lib/pages/options/options-menu/options-menu.component';
import { ButtonComponent } from '../../../../../shared-lib/src/lib/io/button/button.component';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { IMovieRequest } from '../../core/movies/models/movie-request.interface';
import { MoviesService } from '../../core/movies/services/movies.service';
import { Router } from '@angular/router';

@Component({
  imports: [
    OptionsMenuComponent,
    OptionsMenuHeaderDirective,
    OptionsMenuContentDirective,
    ButtonComponent,
    ReactiveFormsModule,
    MultipleSelectorComponent,
    ImageDropComponent
  ],
  selector: 'app-movies-create',
  styleUrl: './movies-create.component.scss',
  templateUrl: './movies-create.component.html',
})
export class MoviesCreateComponent {
  private readonly _moviesService = inject(MoviesService);
  private readonly _router = inject(Router);

  activatedMenu: 'left' | 'right' = 'left';

  createMovieForm = new FormGroup({
    title: new FormControl('', [ Validators.required ]),
    sinopse: new FormControl('', [ Validators.required ]),
    runtime: new FormControl('', [ Validators.required ]),
    releaseDate: new FormControl('', [ Validators.required ]),
    categories: new FormControl([], [ Validators.required ]),
    classInd: new FormControl('', [ Validators.required ]),
    poster: new FormControl(null),
    featured: new FormControl(null),
    standard: new FormControl(null)
  });

  categoriesOptions = [
    { id: 'Action', label: 'Ação' },
    { id: 'Adventure', label: 'Aventura' },
    { id: 'Comedy', label: 'Comédia' },
    { id: 'ScienceFiction', label: 'Ficção Científica' },
    { id: 'Horror', label: 'Terror' },
    { id: 'Romance', label: 'Romance' },
    { id: 'Drama', label: 'Drama' },
    { id: 'Animation', label: 'Animação' },
  ]

  onSubmit() {
    if (this.createMovieForm.invalid) {
      this.createMovieForm.setErrors({validationError: true});
      return;
    }

    const movieRequest: IMovieRequest = {
      title: this.createMovieForm.value.title!,
      sinopse: this.createMovieForm.value.sinopse!,
      duration: this.createMovieForm.value.runtime!,
      releaseDate: new Date(this.createMovieForm.value.releaseDate!),
      filmGenres: this.createMovieForm.value.categories!,
      classInd: this.createMovieForm.value.classInd!
    };

    this._moviesService
      .createMovie(
        movieRequest,
        this.createMovieForm.value.poster ?? null,
        this.createMovieForm.value.featured ?? null,
        this.createMovieForm.value.standard ?? null)
      .subscribe({
        next: (movie) => {
          this._router.navigate(['/movies', movie.id]);
        },
        error: (error) => {
          if (error.status === 400) {
            if (error.error) {
              this.createMovieForm.setErrors({serverError: error.error.errorMessages.join(', ')});
              return;
            }
          }
          this.createMovieForm.setErrors({unknownError: true});
        }
      });
  }

  selectMenu(menu: 'left' | 'right') {
    this.activatedMenu = menu;
  }
}
