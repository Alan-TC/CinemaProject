import { ISeat } from "./seat.interface";

export interface ISessionRoom {
  tag: string;
  width: number;
  height: number;
  seats: ISeat[];
}
