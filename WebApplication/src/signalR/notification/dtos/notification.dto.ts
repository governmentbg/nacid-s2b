import { ReceivedVoucherState } from "src/received-vouchers/enums/received-voucher-state.enum";
import { VoucherRequestState } from "../../../voucher-requests/enums/voucher-request-state.enum";
import { NotificationEntityType } from "../enums/notification-entity-type.enum";
import { NotificationType } from "../enums/notification-type.enum";

export class NotificationDto {
    entityId: number;

    type: NotificationType;
    entityType: NotificationEntityType;

    companyId: number;
    offeringId: number;
    supplierId: number;

    createDate: Date;

    fromUserid: number;
    fromUsername: string;
    fromFullname: string;
    fromUserOrganization: string;

    toUserId: number;

    text: string;

    // Only if EntityType == NotificationEntityType.VoucherRequest && Type == NotificationType.ChangedState
    vrState: VoucherRequestState;
    // Only if EntityType == NotificationEntityType.VoucherRequest && Type == NotificationType.GeneratedCode
    code: string;

    // Only if EntityType == NotificationEntityType.ReceivedVoucher && Type == NotificationType.ChangedState
    rvState: ReceivedVoucherState;
}