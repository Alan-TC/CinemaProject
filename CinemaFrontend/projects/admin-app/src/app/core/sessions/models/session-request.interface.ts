export interface ISessionRequest {
  startsAt: Date;
  endsAt: Date;
  startTicketSalesAt: Date;
  pricing: number;
  theaterLayoutId: string;
  movieId: string;
}
