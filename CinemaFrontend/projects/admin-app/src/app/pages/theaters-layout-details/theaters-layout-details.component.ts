import { Component, inject, input, OnInit, signal } from '@angular/core';
import { TheatersLayoutService } from '../../core/theaters-layout/services/theaters-layout.service';
import { IEditorGridLayout } from '../../core/theaters-layout/components/theater-layout-editor/models/editor-grid-layout.interface';
import { TheaterLayoutEditorComponent } from '../../core/theaters-layout/components/theater-layout-editor/theater-layout-editor.component';
import { OptionsMenuComponent, OptionsMenuContentDirective, OptionsMenuHeaderDirective } from '../../../../../shared-lib/src/lib/pages/options/options-menu/options-menu.component';
import { ITheaterLayout } from '../../core/theaters-layout/models/theater-layout.interface';
import { ICellType } from '../../core/theaters-layout/components/theater-layout-editor/models/cell-type.interface';

@Component({
  imports: [
    OptionsMenuComponent,
    OptionsMenuHeaderDirective,
    OptionsMenuContentDirective,
    TheaterLayoutEditorComponent,
  ],
  selector: 'app-theaters-layout-details',
  styleUrl: './theaters-layout-details.component.scss',
  templateUrl: './theaters-layout-details.component.html',
})
export class TheatersLayoutDetailsComponent implements OnInit {
  private readonly _theatersLayoutService = inject(TheatersLayoutService);

  readonly id = input.required<string>();

  gridLayout = signal<IEditorGridLayout>({
    tag: '',
    width: 0,
    height: 0,
    cells: []
  });

  ngOnInit(): void {
    this._theatersLayoutService.getTheaterLayoutById(this.id()).subscribe((layout) => {
      this.gridLayout.set(this.theaterLayoutToEditorGridLayout(layout));
    });
  }

  theaterLayoutToEditorGridLayout(layout: ITheaterLayout): IEditorGridLayout {
    const height = layout.layout.height;

    const cells: ICellType[][] = Array.from({ length: height }, () =>
      Array.from({ length: layout.layout.width }, () => ({ type: 'Empty' } as ICellType)));

    layout.layout.grid.forEach((cell) => {
      cells[height - 1 - cell.posY][cell.posX] = { type: cell.type };
    });

    return {
      tag: layout.tag,
      width: layout.layout.width,
      height: layout.layout.height,
      cells: cells
    };
  }
}
