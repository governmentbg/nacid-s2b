import { ErrorLogType } from "src/logs/enums/error-log-type.enum";
import { Verb } from "src/logs/enums/verb.enum";
import { FilterDto } from "src/shared/filter-dtos/filter.dto";

export class ErrorLogFilterDto extends FilterDto {
    ip: string;
    url: string;
    verb: Verb;
    userId: number;
    errorLogType: ErrorLogType;
    logDate: Date;

    // Client only
    user: any;
}