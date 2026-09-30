import { Component, input, OnInit, signal } from '@angular/core';
import { ILayoutGrid } from '../../../../core/theaters-layout/models/layout-grid.interface';

@Component({
  imports: [],
  selector: 'app-layout-preview',
  styleUrl: './layout-preview.component.scss',
  templateUrl: './layout-preview.component.html'
})
export class LayoutPreviewComponent implements OnInit {
  theaterLayout = input.required<ILayoutGrid>();

  theaterGridFormated = signal<string[][]>([]);

  ngOnInit(): void {
    const gridFormated: string[][] = [];
    const gridHeight = this.theaterLayout().height;

    for (let x = 0; x < gridHeight; x++) {
      const row: string[] = [];
      for (let y = 0; y < this.theaterLayout().width; y++) {
        row.push('Empty');
      }
      gridFormated.push(row);
    }

    this.theaterLayout().grid.forEach((cell) => {
      gridFormated[gridHeight - 1 - cell.posX][cell.posY] = cell.type;
    });

    this.theaterGridFormated.set(gridFormated);
  }
}
