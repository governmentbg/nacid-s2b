import { Component, Input } from "@angular/core";
import { ScAttachedFileDto } from "src/shared/dtos/sc-attached-file.dto";
import { FileUploadResource } from "../file-upload/file-upload.resource";
import { catchError, throwError } from "rxjs";

@Component({
    selector: 'file-read-custom-text',
    templateUrl: 'file-read-custom-text.component.html'
})
export class FileReadCustomTextComponent<T extends ScAttachedFileDto> {

    @Input() file: T = null;
    @Input() hasReadPermission = true;
    @Input() linkClass = 'link-primary';
    @Input() disabled = false;
    @Input() text: string = null;

    downloadingFile = false;

    constructor(
        private resource: FileUploadResource
    ) {
    }

    getFile() {
      if (!this.disabled && !this.downloadingFile) {
          this.downloadingFile = true;
          return this.resource.getFile(`api/FileStorage?key=${this.file.key}&fileName=${this.file.name}&dbId=${this.file.dbId}`, this.file.mimeType)
              .pipe(
                  catchError((err) => {
                      this.downloadingFile = false;
                      return throwError(() => err);
                  })
              )
              .subscribe((blob: Blob) => {
                  this.downloadingFile = false;
                  var url = window.URL.createObjectURL(blob);
                  window.open(url);
              });
      } else {
          return null;
      }
  }
}