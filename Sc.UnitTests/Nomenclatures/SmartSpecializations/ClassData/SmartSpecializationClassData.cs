using Sc.Models.FilterDtos.Nomenclatures.SmartSpecializations;
using System.Collections;

namespace Sc.UnitTests.Nomenclatures.SmartSpecializations.ClassDatas
{
    public class SmartSpecializationClassData
    {
        public class SmartSpecializationSearchNonExistingFilter : IEnumerable<object[]>
        {
            private readonly List<object[]> data = new()
            {
                new object[] { new SmartSpecializationFilterDto { Name = "Тестово име" } },
                new object[] { new SmartSpecializationFilterDto { Code = "Тестов код"} },
            };

            public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class SmartSpecializationSearchExactSmartSpecializationByFilter : IEnumerable<object[]>
        {
            private readonly List<object[]> data = new()
            {
                new object[] { new SmartSpecializationFilterDto { Name = "Информатика и ИКТ" } },
            };

            public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
