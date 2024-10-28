import { Component, Input } from "@angular/core";

@Component({
    selector: 'collapsable-section',
    templateUrl: './collapsable-section.component.html',
    styleUrls: ['./collapsable-section.styles.css']
})
export class CollapsableSectionComponent {

    @Input() isCollapsed = true;
    @Input() heading: string;
    @Input() class: string;
}