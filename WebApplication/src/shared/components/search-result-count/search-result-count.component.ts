import { Component, EventEmitter, Input, Output } from "@angular/core";

@Component({
    selector: 'search-result-count',
    templateUrl: 'search-result-count.component.html'
})
export class SearchResultCountComponent {

    @Input() resultLength = 0;
    @Input() dataCountPending = false;
    @Input() limit: number;
    @Input() offset: number;
    @Input() dataCount: number = null;

    @Output() getCountEvent: EventEmitter<void> = new EventEmitter<void>();
}