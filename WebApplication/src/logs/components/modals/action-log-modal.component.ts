import { Component } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { ActionLogDto } from "src/logs/dtos/action-log.dto";
import { ActionLogFilterDto } from "src/logs/dtos/filters/action-log-filter.dto";
import { LogsResource } from "src/logs/log.resource";
import { BaseLogModalComponent } from "./base/base-log-modal.componen";

@Component({
    selector: 'action-log-modal',
    templateUrl: './action-log-modal.component.html',
    providers: [LogsResource]
})
export class ActionLogModalComponent extends BaseLogModalComponent<ActionLogDto, ActionLogFilterDto> {

    constructor(
        protected override resource: LogsResource<ActionLogDto, ActionLogFilterDto>,
        protected override activeModal: NgbActiveModal
    ) {
        super(resource, activeModal, 'actions');
    }
}