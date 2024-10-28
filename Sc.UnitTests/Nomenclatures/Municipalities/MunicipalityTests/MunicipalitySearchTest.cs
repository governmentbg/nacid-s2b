using Sc.Models.Dtos.Nomenclatures.Settlements;
using Sc.Models.Filters.Nomenclatures.Settlements;
using Sc.Models;
using Sc.UnitTests.Common;
using Xunit;
using Sc.UnitTests.Nomenclatures.Municipalities.ServiceInitialization;
using Sc.UnitTests.Nomenclatures.Municipalities.ClassDatas;

namespace Sc.UnitTests.Nomenclatures.Municipalities.MunicipalitiesTests
{
    public class MunicipalitySearchTest
    {
        [Fact]
        public async Task GetAll_Municipalities_ReturnsCorrectCount()
        {
            //Arrange
            using var testScContext = new ScDbContext(TestScDbContext.GetScDbContextOptions());
            testScContext.SeedMunicipalities();
            var service = MunicipalityServiceInitialization.CreateService(testScContext);

            //Act
            var municipalityList = await service.GetSearchResultDto<MunicipalityDto>(new MunicipalityFilterDto(), new CancellationTokenSource().Token);

            //Assert
            Assert.Equal(3, municipalityList.Result.Count);
            Assert.Equal(3, municipalityList.TotalCount);
        }

        [Theory]
        [ClassData(typeof(MunicipalitySearchExactMunicipalityByFilter))]
        public async Task GetAll_Filters_ExactMunicipality(MunicipalityFilterDto filter)
        {
            // Arrange
            using var testScContext = new ScDbContext(TestScDbContext.GetScDbContextOptions());
            testScContext.SeedMunicipalities();
            var service = MunicipalityServiceInitialization.CreateService(testScContext);

            // Act
            var municipalityList = await service.GetSearchResultDto<MunicipalityDto>(filter, new CancellationTokenSource().Token);

            // Assert
            Assert.Single(municipalityList.Result);
            Assert.Equal(1, municipalityList.TotalCount);
            Assert.Equal(1, municipalityList.Result.Single().Id);
        }

        [Theory]
        [ClassData(typeof(MunicipalitySearchNonExistingFilter))]
        public async Task GetAll_Filters_NonExistingMunicipality(MunicipalityFilterDto filter)
        {
            //Arrange
            using var testScContext = new ScDbContext(TestScDbContext.GetScDbContextOptions());
            testScContext.SeedMunicipalities();
            var service = MunicipalityServiceInitialization.CreateService(testScContext);

            //Act
            var municipalityList = await service.GetSearchResultDto<MunicipalityDto>(filter, new CancellationTokenSource().Token);

            //Assert
            Assert.Empty(municipalityList.Result);
            Assert.Equal(0, municipalityList.TotalCount);
        }
    }
}
