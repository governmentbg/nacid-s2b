import { HttpClient, HttpEvent } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, map } from "rxjs";
import { ScAttachedFileDto } from "src/shared/dtos/sc-attached-file.dto";

@Injectable()
export class FileUploadResource {

    constructor(
        private http: HttpClient
    ) {
    }

    uploadFile(file: File, fileStorageUrl: string): Observable<HttpEvent<ScAttachedFileDto>> {
        const formData = new FormData();
        formData.append('file', file, file.name);

        return this.http.post<ScAttachedFileDto>(fileStorageUrl, formData,
            {
                reportProgress: true, observe: 'events'
            });
    }

    getFile(fileUrl: string, mimeType: string): Observable<Blob> {
        return this.http.get(fileUrl,
            { responseType: 'blob' })
            .pipe(
                map(response => new Blob([response], { type: mimeType }))
            );
    }
}