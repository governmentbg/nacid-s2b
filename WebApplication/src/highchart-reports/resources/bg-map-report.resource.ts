import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BgMapReportDto } from "../dtos/bg-map-report.dto";
import { DistrictFilterDto } from "src/nomenclatures/filter-dtos/district-filter.dto";

@Injectable()
export class BgMapReportResource {

    url = 'api/report/bgMap';

    constructor(
        private http: HttpClient
    ) {
    }

    getBgMap(filter: DistrictFilterDto): Observable<BgMapReportDto[]> {
        return this.http.post<BgMapReportDto[]>(`${this.url}`, filter);
    }
}