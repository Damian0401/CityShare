export interface IPostSearchQuery {
  query?: string;
  cityId?: number;
  skipCategoryIds: number[];
  startDate?: Date;
  endDate?: Date;
}
