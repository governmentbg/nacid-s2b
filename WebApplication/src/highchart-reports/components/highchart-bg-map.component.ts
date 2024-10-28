import { Component, OnInit, ViewChild } from "@angular/core";
import * as Highcharts from "highcharts/highmaps";
import { Observable, Observer, catchError, throwError } from "rxjs";
import { BgMapDataDto, BgMapJson, bgMapId } from "src/app/bg-map-json/bg-map-json";
import { BgMapReportResource } from "../resources/bg-map-report.resource";
import { BgMapReportDto } from "../dtos/bg-map-report.dto";
import { DistrictFilterDto } from "src/nomenclatures/filter-dtos/district-filter.dto";
import { TranslateService } from "@ngx-translate/core";
import { Level } from "src/shared/enums/level.enum";
import { HighchartBgMapMenuComponent } from "./highchart-bg-map-menu.component";
import { PageHandlingService } from "src/shared/services/page-handling/page-handling.service";
import { Router } from "@angular/router";

@Component({
    selector: 'highchart-bg-map',
    templateUrl: './highchart-bg-map.component.html',
    styleUrls: ['./highchart-bg-map.component.styles.css']
})
export class HighchartBgMapComponent implements OnInit {

    @ViewChild(HighchartBgMapMenuComponent) bgMapMenuComponent: HighchartBgMapMenuComponent;

    filter = new DistrictFilterDto();

    loadingData = false;

    Highcharts: typeof Highcharts = Highcharts;
    chartConstructor = 'mapChart';
    chartOptions: Highcharts.Options = null;

    bgMapReport: BgMapReportDto[] = [];
    selectedBgReportItem: BgMapReportDto;
    selectedBgReportData: BgMapDataDto;

    level = Level

    bgMapId: number = bgMapId;

    constructor(
        public translateService: TranslateService,
        private bgMapJson: BgMapJson,
        private resource: BgMapReportResource,
        private pageHandlingService: PageHandlingService,
        private router: Router) {
    }

    goBackToBulgaria() {
        this.selectedBgReportData = null; 
        this.selectRegion(bgMapId);       
    }

    selectRegion(id: number) {
        if (id > 999999990) {
            let selectedRegion = this.bgMapJson.bgMapData.find(e => e.id === id);
            this.selectedBgReportItem = this.bgMapReport.find(e => e.id === id);
            this.setDataValues(selectedRegion);
            this.createChartOptions(selectedRegion);
            this.selectedBgReportData = selectedRegion;
            this.collapseAll(id);
        } else {
            let region = this.bgMapReport.find(e => e.children.some(s => s.id === id));
            let district = region.children.find(e => e.id === id);
            this.pageHandlingService.scrollToTop();
            this.router.navigate(['/suppliers'], { queryParams: { districtId: district.id, districtName: district.title } });
        }
    }

    smartSpecializationChanged(smartSpecializationId: number) {
        this.filter.smartSpecializationId = smartSpecializationId;
        this.loadData();
    }

    collapseAll(id: number) {
        if (this.bgMapMenuComponent && this.bgMapMenuComponent.bgMapMenuItemComponent?.length > 0) {
            this.bgMapMenuComponent.bgMapMenuItemComponent.forEach(element => {
                if (element.bgMapItem.id !== id) {
                    element.isCollapsed = true;
                } else {
                    element.isCollapsed = false;
                }
            });
        }
    }

    private getData() {
        this.loadingData = true;

        return new Observable((observer: Observer<BgMapReportDto[]>) => {
            return this.resource.getBgMap(this.filter)
                .pipe(
                    catchError((err) => {
                        this.loadingData = false;
                        observer.next([]);
                        observer.complete();
                        return throwError(() => err);
                    })
                )
                .subscribe(result => {
                    this.bgMapReport = result;
                    observer.next(result);
                    observer.complete();
                });
        });
    }

    private createChartOptions(bgMapData: BgMapDataDto) {
        let that = this;

        this.chartOptions = {
            title: {
                text: bgMapData.title + (this.selectedBgReportItem ? ` (${this.selectedBgReportItem.suppliersCount} доставчик${this.selectedBgReportItem.suppliersCount === 1 ? '' : 'а'})` : ''),
                margin: 70
            },

            legend: {
                layout: 'horizontal',
                borderWidth: 0,
                backgroundColor: 'rgba(255,255,255,0.85)',
                floating: true,
                verticalAlign: 'top',
                y: 25
            },

            colorAxis: {
                min: 1,
                type: 'logarithmic',
                minColor: '#00FFFF',
                maxColor: '#01014A',
                stops: [
                    [0, '#00FFFF'],
                    [0.67, '#4444FF'],
                    [1, '#01014A']
                ]
            },

            plotOptions: {
                series: {
                    cursor: 'pointer',
                    point: {
                        events: {
                            click: function () {
                                let id = +this.options.id;
                                if (this.value) {
                                    that.selectRegion(id);
                                }
                            }
                        }
                    }
                }
            },

            tooltip: {
                formatter: function () {
                    return (`<b>${this.point.name}</b><br><i>Брой доставчици на услуги: <b>${this.point.value ?? 0}</b></i>`);
                }
            },

            series: [
                {
                    type: 'map',
                    color: '#00FFFF',
                    dataLabels: {
                        enabled: true,
                        format: '{point.name}'
                    },
                    allAreas: false,
                    data: bgMapData.data,
                    keys: ['id', 'color']
                }
            ]
        };
    }

    private setDataValues(bgMapDto: BgMapDataDto) {
        bgMapDto.data.forEach(e => {
            let regionData = this.bgMapReport.find(s => s.id === +e.id);

            if (!regionData) {
                let parentRegion = this.bgMapReport.find(s => s.id === bgMapDto.id);

                if (parentRegion && parentRegion?.children?.length > 0) {
                    e.value = parentRegion.children.find(s => s.id === +e.id)?.suppliersCount;
                } else {
                    e.value = undefined;
                }
            } else {
                e.value = regionData?.suppliersCount;
            }
        });
    }

    private loadData() {
        this.getData().subscribe(() => {
            this.selectRegion(bgMapId);
            this.loadingData = false;
        });
    }

    ngOnInit() {
        this.loadData();
    }
}