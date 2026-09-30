import { ILayoutGrid } from "./layout-grid.interface";

export interface ITheaterLayout {
  id: string;
  tag: string;
  createdAt: Date;
  layout: ILayoutGrid;
}
