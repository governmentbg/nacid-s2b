import { Component } from "@angular/core";
import { LogsResource } from "../log.resource";
import { BaseLogSearchComponent } from "./base/base-log-search.component";
import { ActionLogDto } from "../dtos/action-log.dto";
import { ActionLogFilterDto } from "../dtos/filters/action-log-filter.dto";
import { Verb } from "../enums/verb.enum";
import { SearchUnsubscriberService } from "src/shared/services/search-unsubscriber/search-unsubscriber.service";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { ActionLogModalComponent } from "./modals/action-log-modal.component";
import { Configuration } from "src/app/configuration/configuration";

@Component({
    selector: 'action-log-search',
    templateUrl: './action-log-search.component.html',
    providers: [
        LogsResource,
        SearchUnsubscriberService
    ]
})
export class ActionLogSearchComponent extends BaseLogSearchComponent<ActionLogDto, ActionLogFilterDto> {

    verb = Verb;

    constructor(
        protected override resource: LogsResource<ActionLogDto, ActionLogFilterDto>,
        protected override searchUnsubscriberService: SearchUnsubscriberService,
        protected override modalService: NgbModal,
        public override configuration: Configuration
    ) {
        super(resource, ActionLogFilterDto, 'actions', searchUnsubscriberService, modalService, configuration);
    }

    openDetails(id: number) {
        const modal = this.modalService.open(ActionLogModalComponent, { backdrop: 'static', size: 'xl', keyboard: false });
        modal.componentInstance.logId = id;
    }
}