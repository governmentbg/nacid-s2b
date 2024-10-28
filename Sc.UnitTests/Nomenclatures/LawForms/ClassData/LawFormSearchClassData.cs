using Sc.Models.FilterDtos.Nomenclatures.LawForms;
using System.Collections;

namespace Sc.UnitTests.Nomenclatures.LawForms.ClassDatas
{
    public class LawFormSearchNonExistingFilter : IEnumerable<object[]>
    {
        private readonly List<object[]> data = new()
        {
            new object[] { new LawFormFilterDto { Name = "Тестово име" } },
            new object[] { new LawFormFilterDto { Code = "Тестов код"} },
            new object[] { new LawFormFilterDto { Description = "тестово описание" } },
            new object[] { new LawFormFilterDto { TextFilter = "Тест филтер от номенклатурата" } },
        };

        public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class LawFormSearchExactLawFormByFilter : IEnumerable<object[]>
    {
        private readonly List<object[]> data = new()
        {
            new object[] { new LawFormFilterDto { Code = "АД" } },
        };

        public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
