using DocumentFormat.OpenXml.Spreadsheet;
using Infrastructure.FileManagementPackages.Excel.Models;
using OfficeOpenXml;
using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure.FileManagementPackages.Excel.Services
{
    public class ExcelProcessorService
    {
        readonly EnumUtilityService enumUtilityService;

        public ExcelProcessorService(EnumUtilityService enumUtilityService)
        {
            this.enumUtilityService = enumUtilityService;
        }

        public MemoryStream ExportMultiSheet<TResult>(IEnumerable<ExcelSheetDto<TResult>> exportSheets)
        {
            using ExcelPackage package = new();

            foreach (var exportSheet in exportSheets)
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(exportSheet.SheetName);

                ConstructSheet(worksheet, exportSheet);
            }

            var stream = new MemoryStream(package.GetAsByteArray());
            return stream;
        }

        private void ConstructSheet<TResult>(ExcelWorksheet worksheet, ExcelSheetDto<TResult> exportSheet)
        {
            var headers = new List<string>();
            var memberExpressions = new List<MemberExpression>();
            GetHeadersAndMembers(ref headers, ref memberExpressions, exportSheet.Expressions.ToArray());

            bool[] isFormatedMaxCols = new bool[headers.Count];
            int col = 1, row = 1;

            AddFilters(worksheet, ref row, exportSheet);
            AddTitles(worksheet, headers, ref row, ref col);
            FillData(worksheet, exportSheet, memberExpressions, ref row, ref col, ref isFormatedMaxCols);
            AutoFitFields(worksheet, headers, isFormatedMaxCols);
        } 

        private void AutoFitFields(ExcelWorksheet worksheet, List<string> headers, bool[] isFormatedMaxCols)
        {
            for (int i = 0; i <= headers.Count - 1; i++)
            {
                if (!isFormatedMaxCols[i])
                {
                    worksheet.Column(i + 1).AutoFit();
                }
            }
        }

        private void FillData<TResult>(ExcelWorksheet worksheet, ExcelSheetDto<TResult> exportSheet, List<MemberExpression> memberExpressions, ref int row, ref int col, ref bool[] isFormatedMaxCols)
        {
            foreach (var item in exportSheet.Items)
            {
                col = 1;
                row++;

                foreach (var memberExpression in memberExpressions)
                {
                    object value = null;
                    if (memberExpression.Expression.Type == item.GetType() && memberExpression.Expression.GetType() == typeof(UnaryExpression))
                    {
                        value = item.GetType().GetProperty(memberExpression.Member.Name).GetValue(item, null);
                    }
                    else
                    {
                        var resultValue = GetNestedProperties(item, memberExpression.Expression.ToString().Substring(memberExpression.Expression.ToString().IndexOf(".", StringComparison.Ordinal) + 1));
                        if (resultValue == null)
                        {
                            value = null;
                        }
                        else
                        {
                            value = resultValue.GetType().GetProperty(memberExpression.Member.Name).GetValue(resultValue, null);
                        }
                    }

                    if (value != null
                        && value.GetType().BaseType == typeof(Enum))
                    {
                        worksheet.Cells[row, col].Value = enumUtilityService.GetDescription(value);
                    }
                    else
                    {
                        worksheet.Cells[row, col].Value = value;
                    }

                    var fieldType = memberExpression.Type;
                    if (fieldType == typeof(bool)
                        || fieldType == typeof(bool?))
                    {
                        var obj = (bool)worksheet.Cells[row, col].Value;
                        string boolValue = "Не";
                        if (obj)
                        {
                            boolValue = "Да";
                        }
                        worksheet.Cells[row, col].Value = boolValue;
                    }

                    worksheet.Cells[row, col].Style.Numberformat.Format = GetCellFormatting(fieldType);

                    if (!isFormatedMaxCols[col - 1]
                        && worksheet.Cells[row, col].Value != null)
                    {
                        int cellSize = worksheet.Cells[row, col].Value.ToString().Length;
                        if (cellSize > 80)
                        {
                            worksheet.Column(col).Width = 80;
                            worksheet.Column(col).Style.WrapText = true;
                            isFormatedMaxCols[col - 1] = true;
                        }
                    }

                    col++;
                }
            }
        }

        private void AddTitles(ExcelWorksheet worksheet, List<string> headers, ref int row, ref int col)
        {
            for (int i = 0; i < headers.Count; i++)
            {
                worksheet.Cells[row, col].Value = headers[i];
                worksheet.Cells[row, col].Style.Font.Bold = true;

                col++;
            }
        }

        private void AddFilters<TResult>(ExcelWorksheet worksheet, ref int row, ExcelSheetDto<TResult> exportSheet)
        {
            if (!string.IsNullOrWhiteSpace(exportSheet.ReportName) || exportSheet.Filters.Any())
            {

                if (!string.IsNullOrWhiteSpace(exportSheet.ReportName))
                {
                    AddFilter(worksheet, ref row, "Вид справка", exportSheet.ReportName);
                }

                foreach (var filter in exportSheet.Filters)
                {
                    AddFilter(worksheet, ref row, filter.Title, filter.Value);
                }

                row++;
            }
        }

        private void AddFilter(ExcelWorksheet worksheet, ref int row, string title, string value)
        {
            worksheet.Cells[row, 1].Value = $"{title}: ";
            worksheet.Cells[row, 1].Style.Font.Bold = true;

            if (value.Length > 50)
            {
                worksheet.Cells[$"B{row}:G{row}"].Merge = true;
            }
            else
            {
                worksheet.Cells[$"B{row}:C{row}"].Merge = true;
            }

            worksheet.Cells[row, 2].Value = value;

            worksheet.Cells.AutoFitColumns();

            row++;
        }

        public static void GetHeadersAndMembers<T, TResult>(ref List<string> headers, ref List<MemberExpression> memberExpressions, params Expression<Func<T, TResult>>[] expressions)
        {
            foreach (var item in expressions)
            {
                if (item == null)
                {
                    continue;
                }

                var expression = item.Body as MemberInitExpression;
                var bindings = expression.Bindings;

                foreach (var binding in bindings)
                {
                    dynamic obj = binding;

                    var member = obj.Expression as MemberExpression;
                    var unary = obj.Expression as UnaryExpression;
                    var result = member ?? (unary != null ? unary.Operand as MemberExpression : null);

                    if (result == null)
                    {
                        headers.Add(obj.Expression.Value);
                    }
                    else
                    {
                        memberExpressions.Add(result);
                    }
                }
            }
        }

        private static object GetNestedProperties(object original, string properties)
        {
            string[] namesOfProperties = properties.Split('.');
            int size = namesOfProperties.Length - 1;

            PropertyInfo property = original.GetType().GetProperty(namesOfProperties[0]);
            object propValue = property.GetValue(original, null);

            if (propValue != null)
            {
                for (int i = 1; i <= size; i++)
                {
                    property = propValue.GetType().GetProperty(namesOfProperties[i]);
                    propValue = property.GetValue(propValue, null);
                }
            }

            return propValue;
        }

        private static string GetCellFormatting(Type fieldType)
        {
            if (fieldType == typeof(DateTime)
                || fieldType == typeof(DateTime?))
            {
                return "dd-mm-yyyy";
            }
            else if (fieldType == typeof(double)
                || fieldType == typeof(double?)
                || fieldType == typeof(decimal)
                || fieldType == typeof(decimal?))
            {
                return "0.00";
            }

            return null;
        }
    }
}
