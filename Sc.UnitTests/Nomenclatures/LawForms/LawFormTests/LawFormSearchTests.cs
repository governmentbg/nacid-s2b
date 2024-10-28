using Sc.Models;
using Sc.UnitTests.Common;
using Xunit;
using Sc.UnitTests.Nomenclatures.LawForms.ServiceInitialization;
using Sc.Models.Dtos.Nomenclatures;
using Sc.Models.FilterDtos.Nomenclatures.LawForms;
using Sc.UnitTests.Nomenclatures.LawForms.ClassDatas;

namespace Sc.UnitTests.Nomenclatures.LawForms.LawFormTests
{
    public class LawFormSearchTests
    {
        [Fact]
        public async Task GetAll_LawForms_ReturnsCorrectCount()
        {
            //Arrange
            using var testScContext = new ScDbContext(TestScDbContext.GetScDbContextOptions());
            testScContext.SeedLawForms();
            var service = LawFormServiceInitialization.CreateService(testScContext);

            //Act
            var lawFormList = await service.GetSearchResultDto<LawFormDto>(new LawFormFilterDto(), new CancellationTokenSource().Token);

            //Assert
            Assert.Equal(3, lawFormList.Result.Count);
            Assert.Equal(3, lawFormList.TotalCount);
        }

        [Theory]
        [ClassData(typeof(LawFormSearchExactLawFormByFilter))]
        public async Task GetAll_Filters_ExactLawForm(LawFormFilterDto filter)
        {
            // Arrange
            using var testScContext = new ScDbContext(TestScDbContext.GetScDbContextOptions());
            testScContext.SeedLawForms();
            var service = LawFormServiceInitialization.CreateService(testScContext);

            // Act
            var lawFormList = await service.GetSearchResultDto<LawFormDto>(filter, new CancellationTokenSource().Token);

            // Assert
            Assert.Single(lawFormList.Result);
            Assert.Equal(1, lawFormList.TotalCount);
            Assert.Equal(1, lawFormList.Result.Single().Id);
        }

        [Theory]
        [ClassData(typeof(LawFormSearchNonExistingFilter))]
        public async Task GetAll_Filters_NonExistingLawForm(LawFormFilterDto filter)
        {
            //Arrange
            using var testScContext = new ScDbContext(TestScDbContext.GetScDbContextOptions());
            testScContext.SeedLawForms();
            var service = LawFormServiceInitialization.CreateService(testScContext);

            //Act
            var lawFormList = await service.GetSearchResultDto<LawFormDto>(filter, new CancellationTokenSource().Token);

            //Assert
            Assert.Empty(lawFormList.Result);
            Assert.Equal(0, lawFormList.TotalCount);
        }
    }
}
