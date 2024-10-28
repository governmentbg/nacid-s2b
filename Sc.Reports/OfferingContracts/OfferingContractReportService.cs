using Dapper;
using Infrastructure;
using Infrastructure.AppSettings;
using Infrastructure.FileManagementPackages.Csv;
using Infrastructure.FileManagementPackages.Excel.Models;
using Infrastructure.FileManagementPackages.Excel.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Npgsql;
using Sc.Models.Dtos.Base.Search;
using Sc.Models.Dtos.Reports.ReceivedVouchers;
using Sc.Models.Entities.ReceivedVouchers;
using Sc.Models.FilterDtos.Reports.ReceivedVouchers;
using Sc.Models.Interfaces;
using System.Data;
using System.Linq.Expressions;
using System.Text;

namespace Sc.Reports.ReceivedVouchers
{
    public class OfferingContractReportService : IReport<OfferingContractReportDto, ReceivedVoucher, OfferingContractReportFilterDto>, IExport<ReceivedVoucher, OfferingContractReportFilterDto>
    {
        private readonly UserContext userContext;
        private readonly ExcelProcessorService excelProcessorService;
        private readonly CsvProcessorService csvProcessorService;
        private readonly EnumUtilityService enumUtilityService;

        public OfferingContractReportService(
            UserContext userContext,
            ExcelProcessorService excelProcessorService,
            CsvProcessorService csvProcessorService,
            EnumUtilityService enumUtilityService
            )
        {
            this.userContext = userContext;
            this.excelProcessorService = excelProcessorService;
            this.csvProcessorService = csvProcessorService;
            this.enumUtilityService = enumUtilityService;
        }

        public async Task<SearchResultDto<OfferingContractReportDto>> GetReport(OfferingContractReportFilterDto filter, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (filter == null)
            {
                filter = new OfferingContractReportFilterDto();
            }

            if (userContext.OrganizationalUnits.Any(e => e.SupplierId.HasValue && e.SupplierId > 0))
            {
                filter.SupplierIds = userContext.OrganizationalUnits
                    .Where(e => e.SupplierId.HasValue && e.SupplierId > 0)
                    .Select(e => e.SupplierId.Value)
                    .ToList();
            }

            using IDbConnection dbConnection = new NpgsqlConnection(AppSettingsProvider.MainDbConnectionString);

            var sqlBuilder = new SqlBuilder();

            var builderTemplate = sqlBuilder.AddTemplate($@"
                select combinedQuery.supplierid, combinedQuery.suppliertype, combinedQuery.suppliername, combinedQuery.institutionrootname, combinedQuery.institutionrootid,
                combinedQuery.institutionid, combinedQuery.complexid, combinedQuery.offeringid, combinedQuery.offeringcode, combinedQuery.offeringname, sum(combinedQuery.offeringscount) as offeringscount
                from(
                    select /**select**/
                    from receivedvoucher as rv
                    join supplier as supp on supp.id = rv.supplierid
                    left join institution as inst on inst.id = supp.institutionid
                    left join institution as rootinst on rootinst.id = inst.rootid
                    left join complex as comp on comp.id = supp.complexid
                    join supplieroffering as so on so.id = rv.offeringid
                    /**where**/ 
                    /**groupby**/
                    union all
                    select /**select**/
                    from receivedvoucher as rv
                    join supplier as supp on supp.id = rv.secondsupplierid
                    left join institution as inst on inst.id = supp.institutionid
                    left join institution as rootinst on rootinst.id = inst.rootid
                    left join complex as comp on comp.id = supp.complexid
                    join supplieroffering as so on so.id = rv.secondofferingid
                    /**where**/ 
                    /**groupby**/
                ) as combinedQuery
                group by combinedQuery.supplierid, combinedQuery.suppliertype, combinedQuery.suppliername, combinedQuery.institutionrootname, combinedQuery.institutionrootid,
                combinedQuery.institutionid, combinedQuery.complexid, combinedQuery.offeringid, combinedQuery.offeringcode, combinedQuery.offeringname
                /**orderby**/");

            SelectBuilder(sqlBuilder);
            WhereBuilder(sqlBuilder, filter);
            GroupByBuilder(sqlBuilder);
            OrderByBuilder(sqlBuilder);

            var result = await dbConnection.QueryAsync<OfferingContractReportDto>(builderTemplate.RawSql, builderTemplate.Parameters);
            var totalCount = result.Count();

            var searchResult = new SearchResultDto<OfferingContractReportDto>
            {
                Result = result.ToList(),
                TotalCount = totalCount
            };

            return searchResult;
        }

        public void GroupByBuilder(SqlBuilder sqlBuilder)
        {
            sqlBuilder.GroupBy("supp.id");
            sqlBuilder.GroupBy("supp.type");
            sqlBuilder.GroupBy("suppliername");
            sqlBuilder.GroupBy("institutionrootname");
            sqlBuilder.GroupBy("institutionrootid");
            sqlBuilder.GroupBy("institutionid");
            sqlBuilder.GroupBy("complexid");
            sqlBuilder.GroupBy("so.id");
            sqlBuilder.GroupBy("offeringcode");
            sqlBuilder.GroupBy("offeringname");
        }

        public void OrderByBuilder(SqlBuilder sqlBuilder)
        {
            sqlBuilder.OrderBy("combinedQuery.suppliertype");
            sqlBuilder.OrderBy("combinedQuery.institutionrootname");
            sqlBuilder.OrderBy("combinedQuery.institutionrootid = combinedQuery.institutionid");
            sqlBuilder.OrderBy("combinedQuery.suppliername");
            sqlBuilder.OrderBy("combinedQuery.institutionrootname");
        }

        public void SelectBuilder(SqlBuilder sqlBuilder)
        {
            sqlBuilder.Select("supp.id as supplierid");
            sqlBuilder.Select("supp.type as suppliertype");
            sqlBuilder.Select("(case when supp.type = 1 then inst.name when supp.type = 2 then comp.name end) as suppliername");
            sqlBuilder.Select("(case when supp.type = 1 then rootinst.shortname else null end) as institutionrootname");
            sqlBuilder.Select("rootinst.id as institutionrootid");
            sqlBuilder.Select("supp.institutionid as institutionid");
            sqlBuilder.Select("supp.complexid as complexid");
            sqlBuilder.Select("so.id as offeringid");
            sqlBuilder.Select("so.code as offeringcode");
            sqlBuilder.Select("so.name as offeringname");
            sqlBuilder.Select("count(*) as offeringscount");
        }

        public void WhereBuilder(SqlBuilder sqlBuilder, OfferingContractReportFilterDto filter)
        {
            filter.DefaultWhereBuilder(sqlBuilder);
            filter.WhereBuilder(sqlBuilder);
        }

        public List<ExcelSheetFilterDto> ConstructSheetFilter(OfferingContractReportFilterDto filter)
        {
            var excelSheetFilterDtos = new List<ExcelSheetFilterDto>();

            if (filter.State.HasValue)
            {
                excelSheetFilterDtos.Add(new ExcelSheetFilterDto { 
                    Title = "Статус", 
                    Value = enumUtilityService.GetDescription(filter.State) });
            }

            if (filter.FromContractDate.HasValue || filter.ToContractDate.HasValue)
            {
                excelSheetFilterDtos.Add(new ExcelSheetFilterDto { 
                    Title = "От-до дата", 
                    Value = (filter.FromContractDate.HasValue && filter.ToContractDate.HasValue && filter.FromContractDate.Value.Date == filter.ToContractDate.Value.Date) 
                        ? $"{filter.FromContractDate.Value:dd.MM.yyyy}" 
                        : $"{(filter.FromContractDate.HasValue ? filter.FromContractDate.Value.ToString("dd.MM.yyyy") : string.Empty)} - {(filter.ToContractDate.HasValue ? filter.ToContractDate.Value.ToString("dd.MM.yyyy") : string.Empty)}"
                });
            }

            if (filter.SupplierType.HasValue)
            {
                excelSheetFilterDtos.Add(new ExcelSheetFilterDto
                {
                    Title = "Тип на доставчика на услугата",
                    Value = enumUtilityService.GetDescription(filter.SupplierType)
                });
            }

            if (filter.RootInstitution != null)
            {
                excelSheetFilterDtos.Add(new ExcelSheetFilterDto
                {
                    Title = "Организация",
                    Value = filter.RootInstitution.Name
                });
            }

            if (filter.Institution != null)
            {
                excelSheetFilterDtos.Add(new ExcelSheetFilterDto
                {
                    Title = "Звено, предоставящо услугите",
                    Value = filter.Institution.Name
                });
            }

            if (filter.Complex != null)
            {
                excelSheetFilterDtos.Add(new ExcelSheetFilterDto
                {
                    Title = "Инфраструктура, предоставяща услугата",
                    Value = filter.Complex.Name
                });
            }

            if (filter.Company != null)
            {
                excelSheetFilterDtos.Add(new ExcelSheetFilterDto
                {
                    Title = "Предприятие",
                    Value = filter.Company.Name
                });
            }

            return excelSheetFilterDtos;
        }

        public async Task<MemoryStream> ExportCsv(OfferingContractReportFilterDto filter, CancellationToken cancellationToken)
        {
            var searchResult = await GetReport(filter, cancellationToken);

            return csvProcessorService.ExportCsv(searchResult.Result);
        }

        public async Task<MemoryStream> ExportExcel(OfferingContractReportFilterDto filter, CancellationToken cancellationToken)
        {
            var searchResult = await GetReport(filter, cancellationToken);
            var sheetInfo = ConstructSheetFilter(filter);

            var sheets = new List<ExcelSheetDto<ExcelTableTupleDto>> {
                new ExcelSheetDto<ExcelTableTupleDto>
                {
                    SheetName = $"Брой сключени договора {DateTime.Now:dd.MM.yyyy HH:mmч.}",
                    ReportName = "Брой сключени договора",
                    Filters = sheetInfo,
                    Items = searchResult.Result.Cast<object>().ToList(),
                    Expressions = new List<Expression<Func<object, ExcelTableTupleDto>>> {
                        e => new ExcelTableTupleDto { CellItem = ((OfferingContractReportDto)e).SupplierName, ColumnName = "Звено, предоставящо услугите" },
                        e => new ExcelTableTupleDto { CellItem = ((OfferingContractReportDto)e).InstitutionRootName, ColumnName = "Организация" },
                        e => new ExcelTableTupleDto { CellItem = ((OfferingContractReportDto)e).OfferingCode, ColumnName = "Код на предлаганата услуга" },
                        e => new ExcelTableTupleDto { CellItem = ((OfferingContractReportDto)e).OfferingName, ColumnName = "Предлагана услуга" },
                        e => new ExcelTableTupleDto { CellItem = ((OfferingContractReportDto)e).OfferingsCount, ColumnName = "Брой" }
                    }
                }
            };

            var excelStream = excelProcessorService.ExportMultiSheet(sheets);

            return excelStream;
        }

        public async Task<MemoryStream> ExportJson(OfferingContractReportFilterDto filter, CancellationToken cancellationToken)
        {
            var searchResult = await GetReport(filter, cancellationToken);

            var jsonString = JsonConvert.SerializeObject(searchResult.Result, Formatting.Indented, new JsonSerializerSettings {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter>() { new Newtonsoft.Json.Converters.StringEnumConverter() }
            });

            return new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
        }
    }
}
