using Sc.Models.Filters.Nomenclatures.Settlements;
using System.Collections;

namespace Sc.UnitTests.Nomenclatures.Settlements.ClassDatas
{
    public class SettlementSearchNonExistingFilter : IEnumerable<object[]>
    {
        private readonly List<object[]> data = new()
        {
            new object[] { new SettlementFilterDto { Name = "Тестово име" } },
            new object[] { new SettlementFilterDto { Code = "Тестов код"} },
            new object[] { new SettlementFilterDto { Description = "тестово описание" } },
            new object[] { new SettlementFilterDto { TextFilter = "Тест филтер от номенклатурата" } },
        };

        public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class SettlementSearchExactSettlementByFilter : IEnumerable<object[]>
    {
        private readonly List<object[]> data = new()
        {
            new object[] { new SettlementFilterDto { Code = "00014" } },
        };

        public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
