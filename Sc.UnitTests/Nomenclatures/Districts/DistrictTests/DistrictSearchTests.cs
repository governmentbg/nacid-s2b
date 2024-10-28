using Sc.Models;
using Sc.Models.Dtos.Nomenclatures.Settlements;
using Sc.Models.Filters.Nomenclatures.Settlements;
using Sc.UnitTests.Common;
using Sc.UnitTests.Nomenclatures.Districts.ClassDatas;
using Sc.UnitTests.Nomenclatures.Districts.ServiceInitialization;
using Xunit;

namespace Sc.UnitTests.Nomenclatures.Districts.DistrictTests
{
    public class DistrictSearchTests
    {
        [Fact]
        public async Task GetAll_Districts_ReturnsCorrectCount()
        {
            //Arrange
            using var testScContext = new ScDbContext(TestScDbContext.GetScDbContextOptions());
            testScContext.SeedDistricts();
            var service = DistrictServiceInitialization.CreateService(testScContext);

            //Act
            var districtList = await service.GetSearchResultDto<DistrictDto>(new DistrictFilterDto(), new CancellationTokenSource().Token);

            //Assert
            Assert.Equal(3, districtList.Result.Count);
            Assert.Equal(3, districtList.TotalCount);
        }

        [Theory]
        [ClassData(typeof(DistrictSearchExactDistrictByFilter))]
        public async Task GetAll_Filters_ExactDistrict(DistrictFilterDto filter)
        {
            // Arrange
            using var testScContext = new ScDbContext(TestScDbContext.GetScDbContextOptions());
            testScContext.SeedDistricts();
            var service = DistrictServiceInitialization.CreateService(testScContext);

            // Act
            var districtList = await service.GetSearchResultDto<DistrictDto>(filter, new CancellationTokenSource().Token);

            // Assert
            Assert.Single(districtList.Result);
            Assert.Equal(1, districtList.TotalCount);
            Assert.Equal(1, districtList.Result.Single().Id);
        }

        [Theory]
        [ClassData(typeof(DistrictSearchNonExistingFilter))]
        public async Task GetAll_Filters_NonExistingDistrict(DistrictFilterDto filter)
        {
            //Arrange
            using var testScContext = new ScDbContext(TestScDbContext.GetScDbContextOptions());
            testScContext.SeedDistricts();
            var service = DistrictServiceInitialization.CreateService(testScContext);

            //Act
            var districtList = await service.GetSearchResultDto<DistrictDto>(filter, new CancellationTokenSource().Token);

            //Assert
            Assert.Empty(districtList.Result);
            Assert.Equal(0, districtList.TotalCount);
        }
    }
}
