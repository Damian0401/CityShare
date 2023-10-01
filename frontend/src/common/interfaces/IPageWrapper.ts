export interface IPageWrapper<T> {
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  content: T[];
}
