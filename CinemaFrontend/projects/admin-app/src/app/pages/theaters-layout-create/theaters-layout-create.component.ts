import { Component } from '@angular/core';
import { OptionsMenuComponent, OptionsMenuContentDirective, OptionsMenuHeaderDirective } from '../../../../../shared-lib/src/lib/pages/options/options-menu/options-menu.component';
import { TheaterLayoutEditorComponent } from '../../core/theaters-layout/components/theater-layout-editor/theater-layout-editor.component';
import { IEditorGridLayout } from '../../core/theaters-layout/components/theater-layout-editor/models/editor-grid-layout.interface';

@Component({
  imports: [
    OptionsMenuComponent,
    OptionsMenuHeaderDirective,
    OptionsMenuContentDirective,
    TheaterLayoutEditorComponent
  ],
  selector: 'app-theaters-layout-create',
  styleUrl: './theaters-layout-create.component.scss',
  templateUrl: './theaters-layout-create.component.html',
})
export class TheatersLayoutCreateComponent {
  gridLayout: IEditorGridLayout = {
    tag: '',
    width: 3,
    height: 3,
    cells: [
      [{ type: 'Empty' }, { type: 'Empty' }, { type: 'Empty' }],
      [{ type: 'Empty' }, { type: 'Empty' }, { type: 'Empty' }],
      [{ type: 'Seat' }, { type: 'Seat' }, { type: 'Seat' }],
    ]
  };
}
