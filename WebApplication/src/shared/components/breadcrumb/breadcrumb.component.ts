import { Component } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs';
import { BreadcrumbItem } from './breadcrumb-item.model';
import { BreadcrumbType } from './breadcrumb-type.enum';

@Component({
  selector: 'app-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  styleUrls: ['./breadcrumb.styles.css']
})
export class BreadcrumbComponent {

  currentBreadcrumbItem: BreadcrumbItem = new BreadcrumbItem('/', 'root.header.sc');
  breadcrumbItems: BreadcrumbItem[] = [];

  breadcrumbTypeEnum = BreadcrumbType;
  breadcrumbType = BreadcrumbType.none;
  showBackButton = true;

  constructor(
    public router: Router,
    private activatedRoute: ActivatedRoute,
    private location: Location
  ) {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.breadcrumbItems = [];
        let currentRoute = this.activatedRoute;

        while (currentRoute.firstChild) {
          currentRoute = currentRoute.firstChild;
        }

        this.breadcrumbType = currentRoute.snapshot.data['breadcrumbType'] ?? this.breadcrumbTypeEnum.none;
        this.showBackButton = currentRoute.snapshot.data['showBackButton'] ?? true;
        this.currentBreadcrumbItem = new BreadcrumbItem(this.router.url, currentRoute.snapshot.data['title']);
        this.createBreadcrumbs(this.activatedRoute.root);
      });
  }

  goBack() {
    this.location.back();
  }

  private createBreadcrumbs(route: ActivatedRoute, url: string = '') {
    const children: ActivatedRoute[] = route.children;

    for (const child of children) {
      const routeURL: string = child.snapshot.url.map(segment => segment.path).join('/');
      if (routeURL !== '') {
        url += `/${routeURL}`;
      }

      const label = child.snapshot.data['title'];
      if (label && url != this.currentBreadcrumbItem.url) {
        this.breadcrumbItems.push(new BreadcrumbItem(url, label));
      }

      this.createBreadcrumbs(child, url);
    }
  }
}
