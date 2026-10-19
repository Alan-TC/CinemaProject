export interface IPaginationResponse<T> {
  items: T[];
  totalCount: number;
  currentPage: number;
  totalPages: number;
  pageSize: number;
  hasPrevious: boolean;
  hasNext: boolean;
}
