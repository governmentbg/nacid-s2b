import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
    selector: 'voucher-info',
    templateUrl: './voucher-info.component.html',
    styleUrls: ['./voucher-info.styles.css']
})
export class VoucherInfoComponent {
    prices: string[] = ['vouchers.spyMenu.description.firstPrice', 'vouchers.spyMenu.description.secondPrice'];
    consultings: string[] = ['vouchers.spyMenu.purpose.firstConsulting', 'vouchers.spyMenu.purpose.secondConsulting', 'vouchers.spyMenu.purpose.thirdConsulting']
    services: string[] = ['vouchers.spyMenu.purpose.firstService', 'vouchers.spyMenu.purpose.secondService']
    ineligibleActivities: string[] = ['vouchers.spyMenu.ineligibleActivities.firstActivity', 'vouchers.spyMenu.ineligibleActivities.secondActivity',
        'vouchers.spyMenu.ineligibleActivities.thirdActivity', 'vouchers.spyMenu.ineligibleActivities.forthActivity',
        'vouchers.spyMenu.ineligibleActivities.fifthActivity', 'vouchers.spyMenu.ineligibleActivities.sixthActivity'];
    additionalIneligibleActivities: string[] = ['vouchers.spyMenu.ineligibleActivities.firstAdditional', 'vouchers.spyMenu.ineligibleActivities.secondAdditional', 
        'vouchers.spyMenu.ineligibleActivities.thirdAdditional', 'vouchers.spyMenu.ineligibleActivities.fourthAdditional']

    fifthStepDocuments: string[] = ['vouchers.spyMenu.grantApplication.firstDocument', 'vouchers.spyMenu.grantApplication.secondDocument', 
        'vouchers.spyMenu.grantApplication.thirdDocument']

    constructor(public translateService: TranslateService) {
    }
}
