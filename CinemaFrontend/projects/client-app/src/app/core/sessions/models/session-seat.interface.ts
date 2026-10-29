export interface ISessionSeat {
  id: string,
  code: string,
  posX: number,
  posY: number,
  status: 'Available' | 'Unavailable'
}
