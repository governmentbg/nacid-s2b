export class BgMapReportDto {
    id: number;
    parentId: number;
    title: string;

    suppliersCount: number;

    children: BgMapReportDto[] = [];
}