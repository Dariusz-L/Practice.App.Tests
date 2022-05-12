using Common.Basic.Blocks;
using Common.Basic.CQRS.Query;
using Common.Basic.Repository;
using NSubstitute;
using NUnit.Framework;
using Practicer.App.Queries.Templates.GetTemplateSection;
using Practicer.Domain.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Practicer.App.Tests
{
    internal class GetTemplateSectionQueryTests
    {
        private GetTemplateSectionQuery Query = new GetTemplateSectionQuery();

        private static readonly TemplateSignature[] Signatures = 
            new TemplateSignature[] 
            {
                new TemplateSignature("1", "1"),
                new TemplateSignature("2", "2"),
                new TemplateSignature("3", "3"),
                new TemplateSignature("4", "4"),
                new TemplateSignature("5", "5"),
            };

        private static readonly TemplateSignatureList SignatureList = 
            new TemplateSignatureList(
                AppConfiguration.TemplateSignatureListID,
                new List<string>() { "2", "3", "1", "5", "4" }
            );

        private static readonly TemplatePage Page =
            new TemplatePage(
                "ID",
                "PageName",
                new List<TemplatePage>()
                {
                    new TemplatePage("ID_Child1", "PageName_Child1", "2"),
                    new TemplatePage(
                        "ID_Child2",
                        "PageName_Child2",
                        new List<TemplatePage>()
                        {
                            new TemplatePage("ID_Child21", "PageName_Child21", "5")
                        },
                        "3",
                        isTemplateExpanded: true,
                        isPageExpanded: false
                    ),
                    new TemplatePage("ID_Child3", "PageName_Child3", "4"),
                },
                "1",
                isTemplateExpanded: true,
                isPageExpanded: false
            );

        [Test]
        public async Task Handle()
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

            var templatePageRepository = Substitute.For<IRepository<TemplatePage>>();
            templatePageRepository
                .GetBy(AppConfiguration.TemplatePageRootID)
                .Returns(Result.SuccessTask(Page));

            // Act
            var result = await CreateHandler(
                signatureRepository, signatureListRepository, templatePageRepository).Handle(Query);
            var vm = result.Get();

            // Assert
            Assert.IsTrue(result.IsSuccess);

            Assert.AreEqual(vm.Signatures[0].ID, "2");
            Assert.AreEqual(vm.Signatures[1].ID, "3");
            Assert.AreEqual(vm.Signatures[2].ID, "1");
            Assert.AreEqual(vm.Signatures[3].ID, "5");
            Assert.AreEqual(vm.Signatures[4].ID, "4");
            Assert.AreEqual(vm.Pages[0].ID, "ID_Child1");
            Assert.AreEqual(vm.Pages[1].ID, "ID_Child2");
            Assert.AreEqual(vm.Pages[1].SubPages[0].ID, "ID_Child21");
            Assert.AreEqual(vm.Pages[2].ID, "ID_Child3");
        }

        [Test]
        public async Task Handle_WhenSignatureRepoEmpty()
        {
            // Arrange
            var signatureRepository = Substitute.For<IRepository<TemplateSignature>>();
            signatureRepository
                .GetAll()
                .Returns(Result.SuccessTask(Array.Empty<TemplateSignature>()));

            var signatureListRepository = Substitute.For<IRepository<TemplateSignatureList>>();
            signatureListRepository
                .GetBy(AppConfiguration.TemplateSignatureListID)
                .Returns(Result.SuccessTask());

            var templatePageRepository = Substitute.For<IRepository<TemplatePage>>();
            templatePageRepository
                .GetBy(AppConfiguration.TemplatePageRootID)
                .Returns(Result.SuccessTask(Page));

            // Act
            var result = await CreateHandler(
                signatureRepository, signatureListRepository, templatePageRepository).Handle(Query);
            var vm = result.Get();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(vm.Signatures.Length, 0);
        }

        private IQueryHandler<GetTemplateSectionQuery, GetTemplateSectionQueryDTO> CreateHandler(params object[] repositories) =>
            Extensions.CreateHandler<GetTemplateSectionQueryHandler>(repositories);
    }
}
