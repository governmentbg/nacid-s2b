import { Component, Input } from '@angular/core';

@Component({
    selector: 'static-file-download',
    templateUrl: './static-file-download.component.html'
})
export class StaticFileDownloadComponent {

    @Input() href: string;
    @Input() class: string;
    @Input() text: string;
    @Input() icon: string;
}
