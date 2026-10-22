import { IMovieImagesUrl } from "../../movies/models/movies-images-url.interface";

export interface ISessionSummary {
  id: string,
  movieTitle: string,
  classInd: string,
  moviePosterUrl: string | null,
  startsAt: Date,
  endsAt: Date
}
