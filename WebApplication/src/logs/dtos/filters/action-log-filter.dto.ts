import { Verb } from "src/logs/enums/verb.enum";
import { FilterDto } from "src/shared/filter-dtos/filter.dto";

export class ActionLogFilterDto extends FilterDto {
    ip: string;
    url: string;
    verb: Verb;
    userId: number;
    logDate: Date;

    // Client only
    user: any;
}