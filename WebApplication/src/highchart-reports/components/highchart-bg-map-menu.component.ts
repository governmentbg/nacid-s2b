import { Component, EventEmitter, Input, Output, ViewChildren } from "@angular/core";
import { BgMapReportDto } from "../dtos/bg-map-report.dto";
import { BgMapDataDto } from "src/app/bg-map-json/bg-map-json";
import { HighchartBgMapMenuItemComponent } from "./highchart-bg-map-menu-item.component";

@Component({
    selector: 'highchart-bg-map-menu',
    templateUrl: './highchart-bg-map-menu.component.html'
})
export class HighchartBgMapMenuComponent {

    @Input() bgMapReport: BgMapReportDto[] = [];
    @Input() selectedBgReportData: BgMapDataDto;

    @Output() selectRegionEvent: EventEmitter<number> = new EventEmitter<number>();

    @ViewChildren(HighchartBgMapMenuItemComponent) bgMapMenuItemComponent: HighchartBgMapMenuItemComponent[];
}