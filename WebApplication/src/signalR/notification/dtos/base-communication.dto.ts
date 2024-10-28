export class BaseCommunicationDto {
    id: number;

    entityId: number;

    createDate: Date;

    fromUserId: number;
    fromUsername: string;
    fromFullname: string;

    text: string;
}