using Sc.Models.Dtos.Nomenclatures.Settlements;
using Sc.Models;
using Sc.Models.Filters.Nomenclatures.Settlements;
using Sc.UnitTests.Common;
using Xunit;
using Sc.UnitTests.Nomenclatures.Settlements.ServiceInitialization;
using Sc.UnitTests.Nomenclatures.Settlements.ClassDatas;

namespace Sc.UnitTests.Nomenclatures.Settlements.SettlementTests
{
    public class SettlementSearchTests
    {
        [Fact]
        public async Task GetAll_Settlements_ReturnsCorrectCount()
        {
            //Arrange
            using var testScContext = new ScDbContext(TestScDbContext.GetScDbContextOptions());
            testScContext.SeedSettlements();
            var service = SettlementServiceInitialization.CreateService(testScContext);

            //Act
            var settlementList = await service.GetSearchResultDto<SettlementDto>(new SettlementFilterDto(), new CancellationTokenSource().Token);

            //Assert
            Assert.Equal(3, settlementList.Result.Count);
            Assert.Equal(3, settlementList.TotalCount);
        }

        [Theory]
        [ClassData(typeof(SettlementSearchExactSettlementByFilter))]
        public async Task GetAll_Filters_ExactSettlement(SettlementFilterDto filter)
        {
            // Arrange
            using var testScContext = new ScDbContext(TestScDbContext.GetScDbContextOptions());
            testScContext.SeedSettlements();
            var service = SettlementServiceInitialization.CreateService(testScContext);

            // Act
            var settlementList = await service.GetSearchResultDto<SettlementDto>(filter, new CancellationTokenSource().Token);

            // Assert
            Assert.Single(settlementList.Result);
            Assert.Equal(1, settlementList.TotalCount);
            Assert.Equal(1, settlementList.Result.Single().Id);
        }

        [Theory]
        [ClassData(typeof(SettlementSearchNonExistingFilter))]
        public async Task GetAll_Filters_NonExistingSettlement(SettlementFilterDto filter)
        {
            //Arrange
            using var testScContext = new ScDbContext(TestScDbContext.GetScDbContextOptions());
            testScContext.SeedSettlements();
            var service = SettlementServiceInitialization.CreateService(testScContext);

            //Act
            var settlementList = await service.GetSearchResultDto<SettlementDto>(filter, new CancellationTokenSource().Token);

            //Assert
            Assert.Empty(settlementList.Result);
            Assert.Equal(0, settlementList.TotalCount);
        }
    }
}
