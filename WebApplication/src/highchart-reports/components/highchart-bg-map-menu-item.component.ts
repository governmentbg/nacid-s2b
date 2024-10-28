import { Component, EventEmitter, Input, Output } from "@angular/core";
import { BgMapReportDto } from "../dtos/bg-map-report.dto";
import { BgMapDataDto } from "src/app/bg-map-json/bg-map-json";

@Component({
    selector: 'highchart-bg-map-menu-item',
    templateUrl: './highchart-bg-map-menu-item.component.html',
    styleUrls: ['./highchart-bg-map-menu-item.styles.css']
})
export class HighchartBgMapMenuItemComponent {

    @Input() bgMapItem: BgMapReportDto = new BgMapReportDto();
    @Input() selectedBgReportData: BgMapDataDto;
    @Input() icon = '';
    @Input() class = 'fs-18';
    @Input() isCollapsed = true;

    @Output() selectRegionEvent: EventEmitter<number> = new EventEmitter<number>();

    selectRegion(id: number) {
        this.isCollapsed = false;
        this.selectRegionEvent.emit(id);
    }
}