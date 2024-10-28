using Sc.Models;
using Sc.UnitTests.Common;
using Xunit;
using Sc.UnitTests.Nomenclatures.SmartSpecializations.ServiceInitialization;
using Sc.Models.Dtos.Nomenclatures.SmartSpecializations;
using Sc.Models.FilterDtos.Nomenclatures.SmartSpecializations;
using static Sc.UnitTests.Nomenclatures.SmartSpecializations.ClassDatas.SmartSpecializationClassData;

namespace Sc.UnitTests.Nomenclatures.SmartSpecializations.SmartSpecializationTests
{
    public class SmartSpecializationSearchTests
    {
        [Fact]
        public async Task GetAll_SmartSpecializations_ReturnsCorrectCount()
        {
            //Arrange
            using var testScContext = new ScDbContext(TestScDbContext.GetScDbContextOptions());
            testScContext.SeedSmartSpecialization();
            var service = SmartSpecializationServiceInitialization.CreateService(testScContext);

            //Act
            var smartSpecializationList = await service.GetSearchResultDto<SmartSpecializationDto>(new SmartSpecializationFilterDto(), new CancellationTokenSource().Token);

            //Assert
            Assert.Equal(3, smartSpecializationList.Result.Count);
            Assert.Equal(3, smartSpecializationList.TotalCount);
        }

        [Theory]
        [ClassData(typeof(SmartSpecializationSearchExactSmartSpecializationByFilter))]
        public async Task GetAll_Filters_ExactSmartSpecialization(SmartSpecializationFilterDto filter)
        {
            // Arrange
            using var testScContext = new ScDbContext(TestScDbContext.GetScDbContextOptions());
            testScContext.SeedSmartSpecialization();
            var service = SmartSpecializationServiceInitialization.CreateService(testScContext);

            // Act
            var smartSpecializationList = await service.GetSearchResultDto<SmartSpecializationDto>(filter, new CancellationTokenSource().Token);

            // Assert
            Assert.Single(smartSpecializationList.Result);
            Assert.Equal(1, smartSpecializationList.TotalCount);
            Assert.Equal(1, smartSpecializationList.Result.Single().Id);
        }

        [Theory]
        [ClassData(typeof(SmartSpecializationSearchNonExistingFilter))]
        public async Task GetAll_Filters_NonExistingSmartSpecialization(SmartSpecializationFilterDto filter)
        {
            //Arrange
            using var testScContext = new ScDbContext(TestScDbContext.GetScDbContextOptions());
            testScContext.SeedSmartSpecialization();
            var service = SmartSpecializationServiceInitialization.CreateService(testScContext);

            //Act
            var smartSpecializationList = await service.GetSearchResultDto<SmartSpecializationDto>(filter, new CancellationTokenSource().Token);

            //Assert
            Assert.Empty(smartSpecializationList.Result);
            Assert.Equal(0, smartSpecializationList.TotalCount);
        }
    }
}
