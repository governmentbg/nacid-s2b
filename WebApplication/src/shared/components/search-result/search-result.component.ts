import { Component, Input } from "@angular/core";

@Component({
    selector: 'search-result',
    templateUrl: 'search-result.component.html'
})
export class SearchResultComponent {

    @Input() offset: number;
    @Input() limit: number;
    @Input() totalCount: number;

}