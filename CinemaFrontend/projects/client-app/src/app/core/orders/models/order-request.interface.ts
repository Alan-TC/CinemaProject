export interface IOrderRequest {
  sessionId: string,
  fullPriceQuantity: number,
  halfPriceQuantity: number,
  seats: string[];
}
