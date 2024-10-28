import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ReceivedVoucherCountResource {
  private receivedVoucherCountSubject: BehaviorSubject<number> = new BehaviorSubject<number>(0);
  public receivedVoucherCount$: Observable<number> = this.receivedVoucherCountSubject.asObservable();

  constructor(private http: HttpClient) {}


  fetchVoucherCountForCompany(): void {
    this.http.get<number>(`/api/receivedVouchers/count`,)
      .pipe(
        tap((count: number) => this.receivedVoucherCountSubject.next(count))
      )
      .subscribe();
  }
 
incrementVoucherCount(): void {
    const currentCount = this.receivedVoucherCountSubject.value;
    this.receivedVoucherCountSubject.next(currentCount + 1);
  }
}
