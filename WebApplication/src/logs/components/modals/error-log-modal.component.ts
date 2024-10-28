import { Component } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { LogsResource } from "src/logs/log.resource";
import { BaseLogModalComponent } from "./base/base-log-modal.componen";
import { ErrorLogDto } from "src/logs/dtos/error-log.dto";
import { ErrorLogFilterDto } from "src/logs/dtos/filters/error-log-filter.dto";
import { ErrorLogType } from "src/logs/enums/error-log-type.enum";

@Component({
    selector: 'error-log-modal',
    templateUrl: './error-log-modal.component.html',
    providers: [LogsResource]
})
export class ErrorLogModalComponent extends BaseLogModalComponent<ErrorLogDto, ErrorLogFilterDto> {

    errorLogType = ErrorLogType;

    constructor(
        protected override resource: LogsResource<ErrorLogDto, ErrorLogFilterDto>,
        protected override activeModal: NgbActiveModal
    ) {
        super(resource, activeModal, 'errors');
    }
}