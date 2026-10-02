import { Component, inject } from '@angular/core';
import { OptionsMenuComponent, OptionsMenuContentDirective, OptionsMenuHeaderDirective } from '../../../../../shared-lib/src/lib/pages/options/options-menu/options-menu.component';
import { TheaterLayoutEditorComponent } from '../../core/theaters-layout/components/theater-layout-editor/theater-layout-editor.component';
import { IEditorGridLayout } from '../../core/theaters-layout/components/theater-layout-editor/models/editor-grid-layout.interface';
import { ButtonComponent } from '../../../../../shared-lib/src/lib/io/button/button.component';
import { TheatersLayoutService } from '../../core/theaters-layout/services/theaters-layout.service';
import { ITheaterLayoutRequest } from '../../core/theaters-layout/models/theater-layout-request.interface';
import { IGridCell } from '../../core/theaters-layout/models/grid-cell.interface';
import { Router } from '@angular/router';

@Component({
  imports: [
    OptionsMenuComponent,
    OptionsMenuHeaderDirective,
    OptionsMenuContentDirective,
    TheaterLayoutEditorComponent,
    ButtonComponent
  ],
  selector: 'app-theaters-layout-create',
  styleUrl: './theaters-layout-create.component.scss',
  templateUrl: './theaters-layout-create.component.html',
})
export class TheatersLayoutCreateComponent {
  private readonly _theatersLayoutService = inject(TheatersLayoutService);
  private readonly _router = inject(Router);

  gridLayout: IEditorGridLayout = {
    tag: '',
    width: 0,
    height: 0,
    cells: []
  };

  errorMessage: string | null = null;

  setErrorMessage(message: string) {
    this.errorMessage = message;
    setTimeout(() => {
      this.errorMessage = null;
    }, 5000);
  }

  createTheaterLayout() {
    console.log('Criando layout do teatro com os seguintes dados:', this.gridLayout);
    if (this.gridLayout.width <= 0 || this.gridLayout.height <= 0) {
      this.setErrorMessage('Largura e altura devem ser maiores que 0.');
      return;
    }
    if (!this.gridLayout.tag || this.gridLayout.tag.trim() === '') {
      this.setErrorMessage('A tag do layout não pode estar vazia.');
      return;
    }

    const gridCells: IGridCell[] = [];

    for (let y = 0; y < this.gridLayout.height; y++) {
      for (let x = 0; x < this.gridLayout.width; x++) {
        if (this.gridLayout.cells[y][x].type === 'Seat') {
          gridCells.push({ posX: x, posY: this.gridLayout.height - 1 - y, type: 'Seat' });
        }
      }
    }

    const theaterLayoutRequest: ITheaterLayoutRequest = {
      tag: this.gridLayout.tag,
      layout: {
        width: this.gridLayout.width,
        height: this.gridLayout.height,
        grid: gridCells
      }
    };

    this._theatersLayoutService.createTheaterLayout(theaterLayoutRequest)
      .subscribe({
        next: (response) => {
          this._router.navigate(['/theaters-layout', response.id]);
        },
        error: (error: any) => {
          this.setErrorMessage('Erro ao criar o layout do teatro: ' + error.message);
        }
      });
  }
}
