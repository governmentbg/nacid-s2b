import { Component, Input } from "@angular/core";

@Component({
    selector: 'collapsable-label',
    templateUrl: './collapsable-label.component.html',
    styleUrls: ['./collapsable-label.styles.css']
})
export class CollapsableLabelComponent {

    @Input() isCollapsed = true;
    @Input() heading: string;
    @Input() class = 'fs-16'
}