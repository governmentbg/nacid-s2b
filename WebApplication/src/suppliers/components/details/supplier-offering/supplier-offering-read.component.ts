import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable, Observer, catchError, forkJoin, throwError } from 'rxjs';
import { voucherRequestCreatePermission } from 'src/auth/constants/permission.constants';
import { PermissionService } from 'src/auth/services/permission.service';
import { UserContextService } from 'src/auth/services/user-context.service';
import { SupplierOfferingDto } from 'src/suppliers/dtos/supplier-offering.dto';
import { SupplierDto } from 'src/suppliers/dtos/supplier.dto';
import { OfferingType } from 'src/suppliers/enums/offering-type.enum';
import { SupplierType } from 'src/suppliers/enums/supplier-type.enum';
import { SupplierOfferingResource } from 'src/suppliers/resources/supplier-offering.resource';
import { SupplierResource } from 'src/suppliers/resources/supplier.resource';

@Component({
    selector: 'supplier-offering-read',
    templateUrl: './supplier-offering-read.component.html',
    styleUrls: ['./supplier-offering-read.styles.css']
})
export class SupplierOfferingReadComponent implements OnInit {

    supplierOffering = new SupplierOfferingDto();
    supplier = new SupplierDto();
    supplierType = SupplierType;
    offeringType = OfferingType;
    loadingData = false;
    communicationOpened = false;
    isCompanyUser = false;

    constructor(
        private route: ActivatedRoute,
        private resource: SupplierOfferingResource,
        private supplierResource: SupplierResource,
        private permissionService: PermissionService,
        public userContextService: UserContextService
    ) {
    }

    private getSupplierOffering(supplierId: number, supplierOfferingId: number) {
        return new Observable((observer: Observer<any>) => {
            return this.resource.getById(supplierId, supplierOfferingId)
                .pipe(
                    catchError((err) => {
                        this.loadingData = false;
                        observer.complete();
                        return throwError(() => err);
                    })
                )
                .subscribe(supplierOffering => {
                    observer.next(supplierOffering);
                    observer.complete();
                });
        });
    }

    private getSupplier(supplierId: number) {
        return new Observable((observer: Observer<any>) => {
            return this.supplierResource.getById(supplierId)
                .pipe(
                    catchError((err) => {
                        this.loadingData = false;
                        observer.complete();
                        return throwError(() => err);
                    })
                )
                .subscribe(supplier => {
                    observer.next(supplier);
                    observer.complete();
                });
        });
    }

    ngOnInit() {
        this.loadingData = true;
        this.isCompanyUser = this.permissionService.verifyIsCompanyUserWithPermission(voucherRequestCreatePermission);
        this.route.params.subscribe(p => {
            const supplierId = p['supplierId'];
            const supplierOfferingId = p['id'];

            return forkJoin([
                this.getSupplierOffering(supplierId, supplierOfferingId),
                this.getSupplier(supplierId)])
                .subscribe(([supplierOffering, supplier]) => {
                    this.supplier = supplier;
                    this.supplierOffering = supplierOffering;
                    this.loadingData = false;
                });
        });
    }
}
