using Sc.Models.Filters.Nomenclatures.Settlements;
using System.Collections;

namespace Sc.UnitTests.Nomenclatures.Districts.ClassDatas
{
    public class DistrictSearchNonExistingFilter : IEnumerable<object[]>
    {
        private readonly List<object[]> data = new()
        {
            new object[] { new DistrictFilterDto { Name = "Тестово име" } },
            new object[] { new DistrictFilterDto { Code = "Тестов код"} },
            new object[] { new DistrictFilterDto { Description = "тестово описание" } },
            new object[] { new DistrictFilterDto { TextFilter = "Тест филтер от номенклатурата" } },
        };

        public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class DistrictSearchExactDistrictByFilter : IEnumerable<object[]>
    {
        private readonly List<object[]> data = new()
        {
            new object[] { new DistrictFilterDto { Code = "BLG" } },
        };

        public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
