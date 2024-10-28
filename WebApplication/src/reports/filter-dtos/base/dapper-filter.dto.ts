export class DapperFilterDto {
    limit = 30;
    offset = 0;
    getAllData = true;

    isActive: boolean;

    // For paginator
    currentPage = 1;
}