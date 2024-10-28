import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { SearchResultDto } from "src/shared/dtos/search/search-result.dto";
import { NomenclatureDto } from "./dtos/nomenclature.dto";
import { NomenclatureFilterDto } from "./filter-dtos/nomenclature-filter.dto";

@Injectable()
export class NomenclatureResource<TNomenclature extends NomenclatureDto, TFilter extends NomenclatureFilterDto> {

    url = 'api/nomenclatures/';

    constructor(
        protected http: HttpClient
    ) { }

    init(childUrl: string) {
        this.url = `${this.url}${childUrl}`;
    }

    getById(id: number) {
        return this.http.get<TNomenclature>(`${this.url}/${id}`);
    }

    getAll(filter: TFilter) {
        return this.http.post<SearchResultDto<TNomenclature>>(`${this.url}/search`, filter);
    }

    getByAlias(alias: string) {
        return this.http.get<TNomenclature>(`${this.url}/Alias?alias=${alias}`);
    }
}