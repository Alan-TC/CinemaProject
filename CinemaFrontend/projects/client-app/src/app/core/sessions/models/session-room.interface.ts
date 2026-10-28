import { ISessionSeat } from "./session-seat.interface";

export interface ISessionRoom {
  tag: string,
  width: number,
  height: number,
  seats: ISessionSeat[]
}
