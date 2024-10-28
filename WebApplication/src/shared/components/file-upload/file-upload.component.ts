import { ChangeDetectorRef, Component, ElementRef, Input, ViewChild, forwardRef } from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";
import { FileUploadResource } from "./file-upload.resource";
import { AlertMessageService } from "../alert-message/services/alert-message.service";
import { ScAttachedFileDto } from "src/shared/dtos/sc-attached-file.dto";
import { AlertMessageDto } from "../alert-message/models/alert-message.dto";
import { catchError, throwError } from "rxjs";
import { HttpEvent, HttpEventType } from "@angular/common/http";

@Component({
    selector: 'file-upload',
    templateUrl: './file-upload.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => FileUploadComponent),
            multi: true
        }
    ]
})
export class FileUploadComponent implements ControlValueAccessor {

    @Input() model: ScAttachedFileDto = null;
    @Input() acceptedFileFormats: string[] = [];
    @Input() fileFormats: string[] = ['application/pdf', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document'];
    @Input() disabled = false;
    @Input() required = false;
    @Input() allowClear = true;
    @Input() fileStorageUploadUrl = 'api/FileStorage';
    @Input() fileStorageGetUrl = 'api/FileStorage';
    @Input() btnClass = 'btn-primary btn-sm';
    @Input() formControlClass = 'form-control form-control-sm';
    @Input() inputGroupClass = 'input-group input-group-sm';

    @ViewChild('fileUploadInput') fileUploadInput: ElementRef;

    fileUrl: string;
    fileUploadProgress: number = null;

    constructor(
        private resource: FileUploadResource,
        private alertMessageService: AlertMessageService,
        private changeDetectorRef: ChangeDetectorRef
    ) {
    }

    getFile() {
        return this.resource.getFile(this.fileUrl, this.model.mimeType)
            .subscribe((blob: Blob) => {
                var url = window.URL.createObjectURL(blob);
                window.open(url);
            });
    }

    uploadFileFromInput() {
        if (this.fileUploadInput) {
            this.fileUploadInput.nativeElement.click();
        }
    }

    uploadFile(event: any) {
        if (this.disabled) {
            return;
        }

        const target = event.target || event.srcElement;
        const files = target.files;
        if (files.length === 1) {

            this.fileUploadProgress = 1;
            let file = files[0];
            if ((this.fileFormats && !this.fileFormats.includes(file.type))
                || (this.acceptedFileFormats.length > 0 && !this.acceptedFileFormats.includes(file.type))) {
                const alertMessage = new AlertMessageDto('errorTexts.FileUpload_WrongFileFormat', 'fa-solid fa-triangle-exclamation', '.' + file.name.split(".").pop(), 'bg-danger text-light');
                this.alertMessageService.show(alertMessage);
                this.fileUploadProgress = null;
            } else {
                this.resource.uploadFile(file, this.fileStorageUploadUrl)
                    .pipe(
                        catchError((err) => {
                            this.fileUploadProgress = null;
                            return throwError(() => err);
                        })
                    )
                    .subscribe((resultEvent: HttpEvent<ScAttachedFileDto>) => {
                        if (resultEvent.type === HttpEventType.UploadProgress) {
                            this.fileUploadProgress = Math.round(100 * resultEvent.loaded / resultEvent.total);
                        } else if (resultEvent.type === HttpEventType.Response) {
                            this.fileUploadProgress = null;
                            this.markAsUploaded(files[0], resultEvent.body);
                        }
                    });
            }
        }
    }

    deleteFile() {
        if (this.disabled || this.fileUploadProgress > 0) {
            return;
        }

        this.model = null;

        this.setFileUrl();
        this.propagateChange(this.model);
    }

    private markAsUploaded(file: File, additionalInfo: ScAttachedFileDto) {
        if (!this.model) {
            this.model = new ScAttachedFileDto();
        }

        this.model.name = file.name;
        this.model.mimeType = file.type;
        this.model.size = file.size;
        this.model.key = additionalInfo.key || (additionalInfo as any).fileKey;
        this.model.hash = additionalInfo.hash;
        this.model.dbId = additionalInfo.dbId;

        this.setFileUrl();
        this.propagateChange(this.model);
    }

    private setFileUrl(): void {
        if (!this.model) {
            this.fileUrl = null;
        } else {
            this.fileUrl = `${this.fileStorageGetUrl}?key=${this.model.key}&fileName=${this.model.name}&dbId=${this.model.dbId}`;
        }
    }

    // ControlValueAccessor implementation start
    propagateChange = (_: any) => { };
    propagateTouched = () => { };
    registerOnChange(fn: (_: any) => void) {
        this.propagateChange = fn;
    }
    registerOnTouched(fn: () => void) {
        this.propagateTouched = fn;
    }
    writeValue(value: any) {
        this.model = value;
        this.setFileUrl();
        this.changeDetectorRef.detectChanges();
    }
}