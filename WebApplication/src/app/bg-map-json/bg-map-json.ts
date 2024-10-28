import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

export const bgMapId = 999999999;

@Injectable()
export class BgMapJson {

    bgMapData: BgMapDataDto[] = [];

    constructor(
        private httpClient: HttpClient
    ) { }

    load(): Promise<{}> {
        return new Promise(resolve => {
            this.httpClient.get('../../assets/bg-map-json/bg-map.json')
                .subscribe(config => {
                    this.importSettings(config);
                    resolve(true);
                });
        });
    }

    private importSettings(bgMapData: any) {
        this.bgMapData = bgMapData;
    }
}

export class BgMapDataDto {
    id: number;
    parentId: number;
    title: string;
    data: BgMapLocationDto[] = [];
}

export class BgMapLocationDto {
    id: string;
    name: string;
    value: number;
    path: string;
}