export interface IEventSearchQuery {
  query?: string;
  cityId?: number;
  skipCategoryIds?: number[];
  startDate?: Date | null;
  endDate?: Date | null;
  sortBy?: string;
  isNow?: boolean;
  pageNumber?: number;
  pageSize?: number;
}
