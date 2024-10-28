import { Component } from "@angular/core";
import { BaseLogSearchComponent } from "./base/base-log-search.component";
import { LogsResource } from "../log.resource";
import { SearchUnsubscriberService } from "src/shared/services/search-unsubscriber/search-unsubscriber.service";
import { ErrorLogDto } from "../dtos/error-log.dto";
import { ErrorLogFilterDto } from "../dtos/filters/error-log-filter.dto";
import { ErrorLogType } from "../enums/error-log-type.enum";
import { Verb } from "../enums/verb.enum";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { ErrorLogModalComponent } from "./modals/error-log-modal.component";
import { Configuration } from "src/app/configuration/configuration";

@Component({
    selector: 'error-log-search',
    templateUrl: './error-log-search.component.html',
    providers: [
        LogsResource,
        SearchUnsubscriberService
    ]
})
export class ErrorLogSearchComponent extends BaseLogSearchComponent<ErrorLogDto, ErrorLogFilterDto> {

    errorLogType = ErrorLogType;
    verb = Verb;

    constructor(
        protected override resource: LogsResource<ErrorLogDto, ErrorLogFilterDto>,
        protected override searchUnsubscriberService: SearchUnsubscriberService,
        protected override modalService: NgbModal,
        public override configuration: Configuration
    ) {
        super(resource, ErrorLogFilterDto, 'errors', searchUnsubscriberService, modalService, configuration);
    }

    openDetails(id: number) {
        const modal = this.modalService.open(ErrorLogModalComponent, { backdrop: 'static', size: 'xl', keyboard: false });
        modal.componentInstance.logId = id;
    }
}