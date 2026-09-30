import { IGridCell } from "./grid-cell.interface";

export interface ILayoutGrid {
  width: number;
  height: number;
  grid: IGridCell[];
}
