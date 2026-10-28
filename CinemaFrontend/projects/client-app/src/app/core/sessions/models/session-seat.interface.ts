export interface ISessionSeat {
  id: string,
  code: string,
  posX: number,
  posY: number,
  Status: 'Available' | 'Unavailable' | 'Selected'
}
