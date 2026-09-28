import { IMovie } from "../../movies/models/movie.interface";
import { ISessionRoom } from "./session-room.interface";

export interface ISessionResponse {
  id: string;
  code: string;
  startsAt: Date;
  endsAt: Date;
  startTicketSalesAt: Date;
  pricing: number;
  sessionRoom: ISessionRoom;
  movie: IMovie;
}
