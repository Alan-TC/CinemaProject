import { IMovie } from "../../movies/models/movie.interface";

export interface ITicket {
  id: string,
  code: string,
  theaterRoomTag: string,
  startsAt: Date,
  seatsCodes: string[],
  halfPriceTickets: number,
  movie: IMovie
}
