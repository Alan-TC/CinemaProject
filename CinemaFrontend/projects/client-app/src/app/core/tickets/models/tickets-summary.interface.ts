export interface ITicketSummary {
  id: string,
  code: string,
  theaterRoomTag: string,
  startsAt: Date,
  seatsCodes: string[],
  halfPriceTickets: number
}
