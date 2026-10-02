import { ICellType } from "./cell-type.interface";

export interface IEditorGridLayout {
  tag: string;
  width: number;
  height: number;
  cells: ICellType[][];
}
