import { Component, HostListener, OnInit } from '@angular/core';
import { ApproveRegistrationsResource } from '../approve-registrations.resource';
import { SearchResultDto } from 'src/shared/dtos/search/search-result.dto';
import { ApproveRegistrationFilterDto } from '../filter-dtos/approve-registration-filter.dto';
import { catchError, throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { ApproveRegistrationSearchDto } from '../dtos/search/approve-registration-search.dto';
import { SearchUnsubscriberService } from 'src/shared/services/search-unsubscriber/search-unsubscriber.service';
import { nacidAlias } from 'src/auth/constants/organizational-unit.constants';
import { PermissionService } from 'src/auth/services/permission.service';
import { approvalRegistrationCreatePermission, approvalRegistrationReadPermission, approvalRegistrationWritePermission } from 'src/auth/constants/permission.constants';
import { ApproveRegistrationState } from '../enums/approve-registration-state.enum';
import { DeclineRegistrationModalComponent } from './modals/decline-registration-modal.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ApproveRegistrationModalComponent } from './modals/approve-registration-modal.component';
import { Router } from '@angular/router';
import { PageHandlingService } from 'src/shared/services/page-handling/page-handling.service';
import { SupplierOfferingCountDto } from '../dtos/supplier-offering-count.dto';
import { SupplierType } from 'src/suppliers/enums/supplier-type.enum';
import { TranslateService } from '@ngx-translate/core';
import { ApproveRegistrationHistorySearchDto } from '../dtos/search/approve-registration-history-search.dto';
import { HistoryRegistrationDetailsComponent } from './history-registration-details.component';

@Component({
  selector: 'app-approve-registration-search',
  templateUrl: './approve-registration-search.component.html',
  providers: [SearchUnsubscriberService]
})
export class ApproveRegistrationSearchComponent implements OnInit {
  loadingData = false;
  searchDataPending = false;
  dataCountPending = false;
  clearDataPending = false;

  hasApprovalRegistrationReadPermission = false;
  hasApprovalRegistrationCreatePermission = false;
  hasApprovalRegistrationWritePermission = false;

  searchResult: SearchResultDto<ApproveRegistrationSearchDto> = new SearchResultDto<ApproveRegistrationSearchDto>();
  filter = new ApproveRegistrationFilterDto();
  supplierOfferingCountDto: SupplierOfferingCountDto = new SupplierOfferingCountDto();

  supplierType = SupplierType;
  state = ApproveRegistrationState;

  @HostListener('document:keydown.enter', ['$event']) onKeydownEnterHandler() {
    this.getData(false, false, true);
  }

  constructor(
    protected searchUnsubscriberService: SearchUnsubscriberService,
    private resource: ApproveRegistrationsResource,
    private permissionService: PermissionService,
    private modalService: NgbModal,
    private router: Router,
    public pageHandlingService: PageHandlingService,
    public translateService: TranslateService
  ) {
  }


  clearFilters() {
    this.filter = new ApproveRegistrationFilterDto();
    this.loadingData = false;
    this.searchDataPending = false;
    this.clearDataPending = true;
    this.getData(false, false, true);
  }

  search() {
    this.loadingData = false;
    this.clearDataPending = false;
    this.searchDataPending = true;
    this.getData(false, true, true);
  }

  getData(triggerLoadingDataIndicator = true, isClearOperation = false, reloadCurrentPage: boolean) {
    this.unsubscribe(1);
    if (triggerLoadingDataIndicator) {
      this.loadingData = true;
    }

    if (reloadCurrentPage) {
      this.filter.currentPage = 1;
    }

    if (isClearOperation) {
      this.searchDataPending = true;
    } else {
      this.searchDataPending = false;
    }

    this.filter.offset = (this.filter.currentPage - 1) * this.filter.limit;

    this.getDataCount();
    var subscriber = this.resource
      .getSearchResultDto(this.filter)
      .pipe(
        catchError((err: HttpErrorResponse) => {
          this.clearDataPending = false;
          this.loadingData = false;
          this.searchDataPending = false;
          return throwError(() => err);
        })
      )
      .subscribe(e => {
        this.searchResult = e;
        this.loadingData = false;
        this.searchDataPending = false;
        this.clearDataPending = false;
      });

    this.searchUnsubscriberService.addSubscription(1, subscriber);
    return subscriber;
  }

  getDataCount() {
    this.dataCountPending = true;

    this.resource
      .supplierOfferingCount(this.filter)
      .pipe(
        catchError((err: HttpErrorResponse) => {
          this.dataCountPending = false;
          return throwError(() => err);
        })
      )
      .subscribe((e) => {
        this.supplierOfferingCountDto = e;
        this.clearDataPending = false;
        this.dataCountPending = false;
      });
  }

  getSupplier(id: number) {
    this.pageHandlingService.scrollToTop();
    this.router.navigate(['/suppliers', id]);
  }

  private unsubscribe(searchType: number) {
    this.loadingData = false;
    this.searchDataPending = true;
    this.searchUnsubscriberService.unsubscribeByType(searchType);
  }

  openApproveModal(registration: ApproveRegistrationSearchDto, index: number) {
    const modal = this.modalService.open(ApproveRegistrationModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
    modal.componentInstance.registrationId = registration.id;
    modal.componentInstance.signUpDto = this.searchResult.result[index].signUpDto;

    return modal.result.then((updatedRegistration: ApproveRegistrationSearchDto) => {
      if (updatedRegistration) {
        this.searchResult.result[index] = updatedRegistration;
        this.supplierOfferingCountDto.supplierCount++;
      }
    });
  }


  openDeclineModal(registration: ApproveRegistrationSearchDto, index: number) {
    const modal = this.modalService.open(DeclineRegistrationModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
    modal.componentInstance.registrationId = registration.id;

    return modal.result.then((updatedRegistration: ApproveRegistrationSearchDto) => {
      if (updatedRegistration) {
        this.searchResult.result[index] = updatedRegistration;
      }
    });
  }

  openEdit(id: number) {
    this.pageHandlingService.scrollToTop();
    this.router.navigate(['/signUpSupplier', id]);
  }

  openHistory(approveRegistrationHistories: ApproveRegistrationHistorySearchDto[]) {
    const modal = this.modalService.open(HistoryRegistrationDetailsComponent, { backdrop: 'static', size: 'xl', keyboard: false });
    modal.componentInstance.history = approveRegistrationHistories;
  }

  ngOnInit() {
    this.hasApprovalRegistrationReadPermission = this.permissionService.verifyUnitPermission(approvalRegistrationReadPermission, [[nacidAlias, null]]);
    this.hasApprovalRegistrationCreatePermission = this.permissionService.verifyUnitPermission(approvalRegistrationCreatePermission, [[nacidAlias, null]]);
    this.hasApprovalRegistrationWritePermission = this.permissionService.verifyUnitPermission(approvalRegistrationWritePermission, [[nacidAlias, null]]);

    return this.getData(false, false, true);
  }
}
