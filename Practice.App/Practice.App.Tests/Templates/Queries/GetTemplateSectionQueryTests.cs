using Common.Basic.Blocks;
using Common.Basic.CQRS.Query;
using Common.Basic.Repository;
using NSubstitute;
using NUnit.Framework;
using Practicer.App.Queries.Templates.GetTemplateSection;
using Practicer.Domain.Templates;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Practicer.App.Tests
{
    internal class GetTemplateSectionQueryTests
    {
        private GetTemplateSectionQuery Query = new GetTemplateSectionQuery();

        private static readonly List<TemplateSignature> Signatures = 
            new List<TemplateSignature>() 
            {
                new TemplateSignature("1", "1"),
                new TemplateSignature("2", "2"),
                new TemplateSignature("3", "3"),
            };

        private static readonly TemplateSignatureList SignatureList = 
            new TemplateSignatureList(
                AppConfiguration.TemplateSignatureListID,
                new List<string>() { "2", "3", "1" }
            );

        [Test]
        public async Task GivenQuery_WhenHandle_ThenVMCorrect()
        {
            // Arrange
            var signatureRepository = Substitute.For<IRepository<TemplateSignature>>();
            signatureRepository
                .GetAll()
                .Returns(Result.SuccessTask(Signatures));

            var signatureListRepository = Substitute.For<IRepository<TemplateSignatureList>>();
            signatureListRepository
                .GetBy(AppConfiguration.TemplateSignatureListID)
                .Returns(Result.SuccessTask(SignatureList));

            // Act
            var result = await CreateHandler(signatureRepository, signatureListRepository).Handle(Query);
            var vm = result.Get();

            // Assert
            Assert.IsTrue(result.IsSuccess);

            Assert.AreEqual(vm.Signatures[0].ID, "2");
            Assert.AreEqual(vm.Signatures[1].ID, "3");
            Assert.AreEqual(vm.Signatures[2].ID, "1");
        }

        [Test]
        public async Task GivenQueryAndEmptySignatureRepo_WhenHandle_ThenVMCorrect()
        {
            // Arrange
            var signatureRepository = Substitute.For<IRepository<TemplateSignature>>();
            signatureRepository
                .GetAll()
                .Returns(Result.SuccessTask(new List<TemplateSignature>()));

            var signatureListRepository = Substitute.For<IRepository<TemplateSignatureList>>();
            signatureListRepository
                .GetBy(AppConfiguration.TemplateSignatureListID)
                .Returns(Result.SuccessTask());

            // Act
            var result = await CreateHandler(signatureRepository, signatureListRepository).Handle(Query);
            var vm = result.Get();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(vm.Signatures.Length, 0);
        }

        private IQueryHandler<GetTemplateSectionQuery, GetTemplateSectionQueryVM> CreateHandler(params object[] repositories) =>
            Extensions.CreateHandler<GetTemplateSectionQueryHandler>(repositories);
    }
}
