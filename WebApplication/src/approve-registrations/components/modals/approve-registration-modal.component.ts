import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { catchError, throwError } from 'rxjs';
import { ApproveRegistrationsResource } from 'src/approve-registrations/approve-registrations.resource';
import { ApproveRegistrationDto } from 'src/approve-registrations/dtos/approve-registration.dto';
import { SignUpDto } from 'src/auth/dtos/sign-up/sign-up.dto';

@Component({
  selector: 'app-approve-registration-modal',
  templateUrl: './approve-registration-modal.component.html'
})
export class ApproveRegistrationModalComponent {

  @Input() registrationId!: number;
  @Input() signUpDto: SignUpDto;

  approvedDto: ApproveRegistrationDto = new ApproveRegistrationDto();
  
  approveDataPending = false;

  constructor(
    public activeModal: NgbActiveModal,
    private resource: ApproveRegistrationsResource
  ) { }

  onApprove(): void {
    this.approvedDto.registrationId = this.registrationId;
    this.approveDataPending = true;
    this.resource
      .approveRegistration(this.approvedDto)
      .pipe(
        catchError((err: HttpErrorResponse) => {
          this.approveDataPending = false;
          return throwError(() => err)
        })
      )
      .subscribe(e => {
        this.approveDataPending = false;
        this.activeModal.close(e);
      });
  }

  onCancel(): void {
    this.activeModal.close();
  }
}
