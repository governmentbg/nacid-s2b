import { Component, Input, ViewChild } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NgForm } from '@angular/forms';
import { ApproveRegistrationsResource } from 'src/approve-registrations/approve-registrations.resource';
import { catchError, throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { DeclineRegistrationDto } from 'src/approve-registrations/dtos/decline-registration.dto';


@Component({
  selector: 'app-decline-registration-modal',
  templateUrl: './decline-registration-modal.component.html',
})
export class DeclineRegistrationModalComponent {
  @Input() registrationId!: number;

  declinedDto: DeclineRegistrationDto = new DeclineRegistrationDto();

  @ViewChild(NgForm) form: NgForm;

  declineDataPending = false;
  
  constructor(
    public activeModal: NgbActiveModal,
    private registrationResource: ApproveRegistrationsResource,
  ) { }

  onSubmit(): void {
    if (this.form.valid) {
      this.declinedDto.registrationId = this.registrationId;
      this.declineDataPending = true;
      this.registrationResource
        .declineRegistration(this.declinedDto)
        .pipe(
          catchError((err: HttpErrorResponse) => {
            this.declineDataPending = false;
            return throwError(() => err)
          })
        )
        .subscribe(e => {
          this.declineDataPending = false;
          this.activeModal.close(e);
        });
    }
  }

  onCancel(): void {
    this.activeModal.close();
  }
}

