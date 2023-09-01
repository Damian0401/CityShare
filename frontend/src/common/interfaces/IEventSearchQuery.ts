export interface IEventSearchQuery {
  query?: string;
  cityId?: number;
  skipCategoryIds: number[];
  startDate: Date;
  endDate: Date | null;
  sortBy?: string;
}
