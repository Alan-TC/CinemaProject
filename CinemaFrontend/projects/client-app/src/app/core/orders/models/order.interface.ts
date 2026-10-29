export interface IOrder {
  id: string,
  sessionCode: string,
  fullPriceSeatsQuantity: number,
  halfPriceSeatsQuantity: number,
  total: number,
  status: 'Pending' | 'Paid',
  seatsCodes: string[]
}
