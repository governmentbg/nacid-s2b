using Sc.Models.Filters.Nomenclatures.Settlements;
using Sc.Models.Filters.Nomenclatures.Settlements;
using System.Collections;

namespace Sc.UnitTests.Nomenclatures.Municipalities.ClassDatas
{
    public class MunicipalitySearchNonExistingFilter : IEnumerable<object[]>
    {
        private readonly List<object[]> data = new()
        {
            new object[] { new MunicipalityFilterDto { Name = "Тестово име" } },
            new object[] { new MunicipalityFilterDto { Code = "Тестов код"} },
            new object[] { new MunicipalityFilterDto { Description = "тестово описание" } },
            new object[] { new MunicipalityFilterDto { TextFilter = "Тест филтер от номенклатурата" } },
        };

        public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class MunicipalitySearchExactMunicipalityByFilter : IEnumerable<object[]>
    {
        private readonly List<object[]> data = new()
        {
            new object[] { new MunicipalityFilterDto { Code = "BGS03" } },
        };

        public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
