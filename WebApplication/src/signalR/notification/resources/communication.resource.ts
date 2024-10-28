import { Injectable } from "@angular/core";
import { BaseCommunicationDto } from "../dtos/base-communication.dto";
import { FilterDto } from "src/shared/filter-dtos/filter.dto";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable()
export class CommunicationResource<T extends BaseCommunicationDto, TFilter extends FilterDto> {

    url = 'api/';

    constructor(
        protected http: HttpClient
    ) { }

    init(childUrl: string) {
        this.url = `${this.url}${childUrl}`;
    }

    getCommunications(filter: TFilter): Observable<T[]> {
        return this.http.post<T[]>(`${this.url}`, filter);
    }

    sendMessage(currentCommunicationDto: T): Observable<T> {
        return this.http.post<T>(`${this.url}/sendMessage`, currentCommunicationDto);
    }
}