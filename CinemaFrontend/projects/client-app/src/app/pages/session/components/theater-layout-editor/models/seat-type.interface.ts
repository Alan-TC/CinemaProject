export interface ISeatType {
  id: string,
  code: string,
  type: 'Empty' | 'Available' | 'Unavailable' | 'Selected'
}
