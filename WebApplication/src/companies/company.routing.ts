import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { CompanySearchComponent } from './components/search/company-search.component';
import { CompanyDetailsComponent } from './components/company-details.component';
import { PermissionAuthGuard } from 'src/app/auth-guard/permission.auth-guard';
import { nacidAlias, pniiditAlias } from 'src/auth/constants/organizational-unit.constants';
import { BreadcrumbType } from 'src/shared/components/breadcrumb/breadcrumb-type.enum';

const routes: Routes = [
    {
        path: 'companies',
        data: {
            title: 'routes.companies'
        },
        children: [
            {
                path: '',
                component: CompanySearchComponent,
                canActivate: [PermissionAuthGuard],
                data: {
                    unitExternals: [
                        [nacidAlias, null],
                        [pniiditAlias, null]
                    ]
                }
            },
            {
                path: ':id',
                component: CompanyDetailsComponent,
                data: {
                    title: 'routes.companyDetails',
                    breadcrumbType: BreadcrumbType.simple
                }
            }
        ]
    }
];

@NgModule({
    imports: [CommonModule, RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class CompanyRoutingModule { }

