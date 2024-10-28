import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ReceivedVoucherCertificateDto } from "../dtos/received-voucher-certificate.dto";
import { Observable } from "rxjs";

@Injectable()
export class ReceivedVoucherCertificateResource {

    url = 'api/receivedVouchers';

    constructor(
        private http: HttpClient
    ) {
    }

    generateCertificate(certificateDto: ReceivedVoucherCertificateDto): Observable<ReceivedVoucherCertificateDto> {
        return this.http.post<ReceivedVoucherCertificateDto>(`${this.url}/${certificateDto.receivedVoucherId}/certificates`, certificateDto);
    }
}