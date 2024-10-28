import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { BreadcrumbType } from "src/shared/components/breadcrumb/breadcrumb-type.enum";
import { SupplierSearchGroupComponent } from "./components/search/supplier-search-group/supplier-search-group.component";
import { SupplierDetailsComponent } from "./components/supplier-details.component";
import { SupplierOfferingReadComponent } from "./components/details/supplier-offering/supplier-offering-read.component";

const routes: Routes = [
    {
        path: 'suppliers',
        data: {
            title: 'routes.suppliers'
        },
        children: [
            {
                path: '',
                component: SupplierSearchGroupComponent
            },
            {
                path: ':id',
                component: SupplierDetailsComponent,
                data: {
                    title: 'routes.supplierDetails',
                    breadcrumbType: BreadcrumbType.simple
                }
            },
            {
                path: ':id/:tab',
                component: SupplierDetailsComponent,
                data: {
                    title: 'routes.supplierDetails',
                    breadcrumbType: BreadcrumbType.simple
                }
            }
        ]
    },
    {
        path: 'suppliers/:supplierId/offerings/:id',
        component: SupplierOfferingReadComponent,
        data: {
            title: 'routes.supplierOfferings',
            breadcrumbType: BreadcrumbType.simple
        }
    }
];

@NgModule({
    imports: [CommonModule, RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class SupplierRoutingModule { }
