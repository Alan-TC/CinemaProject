export interface ISessionRequest {
  id: string;
  code: string;
  movieTitle: string;
  theaterLayoutTag: string;
  pricing: number;
  startsAt: Date;
  endsAt: Date;
  startTicketSalesAt: Date;
  createdAt: Date;
}
