import { Component, HostListener, Input } from "@angular/core";
import { ApproveRegistrationHistorySearchDto } from "../dtos/search/approve-registration-history-search.dto";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";

@Component({
    selector: 'history-registration-details',
    templateUrl: './history-registration-details.component.html',
})
export class HistoryRegistrationDetailsComponent {

    @Input() history: ApproveRegistrationHistorySearchDto[] = [];

    @HostListener('document:keydown.escape', ['$event']) onKeydownHandler() {
        this.close();
    }

    constructor(
        private activeModal: NgbActiveModal
    ) {
    }

    close() {
        this.activeModal.close();
    }
}