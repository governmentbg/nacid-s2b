import { ClearFilterType } from "../enums/clear-filter-type.enum";

export class ClearFilterDto {
  clearType: ClearFilterType;
  id: number;
  value: string | number;
}