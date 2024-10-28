import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './components/home/home.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { BreadcrumbType } from 'src/shared/components/breadcrumb/breadcrumb-type.enum';
import { CookiesPolicyComponent } from './components/cookies-policy/cookies-policy.component';
import { VoucherTabsComponent } from './components/vouchers/voucher-tabs.component';
import { TermsOfUseComponent } from './components/terms-of-use/terms-of-use.component';
import { ConfidentialityComponent } from './components/confidentiality/confidentiality.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'home'
  },
  {
    path: 'home',
    component: HomeComponent,
    data: {
      title: 'routes.home'
    }
  },
  {
    path: 'vouchers',
    component: VoucherTabsComponent,
    data: {
      title: 'routes.vouchers',
      breadcrumbType: BreadcrumbType.simple
    }
  },
  {
    path: 'cookies-policy',
    component: CookiesPolicyComponent,
    data: {
      breadcrumbType: BreadcrumbType.simple,
      title: 'routes.cookies-policy'
    }
  },
  {
    path: 'terms-of-use',
    component: TermsOfUseComponent,
    data: {
      breadcrumbType: BreadcrumbType.simple,
      title: 'routes.terms-of-use'
    }
  },
  {
    path: 'confidentiality',
    component: ConfidentialityComponent,
    data: {
      breadcrumbType: BreadcrumbType.simple,
      title: 'routes.confidentiality'
    }
  },
  {
    path: '**',
    redirectTo: '/notFound'
  },
  {
    path: 'notFound',
    component: PageNotFoundComponent,
    data: {
      title: 'routes.notFound',
      breadcrumbType: BreadcrumbType.simple
    }
  }
];

@NgModule({
  imports: [CommonModule, RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StaticPageRoutingModule { }

