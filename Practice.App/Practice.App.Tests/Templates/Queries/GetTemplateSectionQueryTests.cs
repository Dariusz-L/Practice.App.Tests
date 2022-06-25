//using Common.Basic.Blocks;
//using Common.Basic.CQRS.Query;
//using Common.Basic.Repository;
//using NSubstitute;
//using NUnit.Framework;
//using Practicer.App.Queries.Templates.GetTemplateSection;
//using Practicer.Domain.Templates;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace Practicer.App.Tests
//{
//    internal class GetTemplateSectionQueryTests
//    {
//        private GetTemplateSectionQuery Query = new GetTemplateSectionQuery();

//        private static readonly TemplateSignature[] Signatures = 
//            new TemplateSignature[] 
//            {
//                new TemplateSignature("1", "1"),
//                new TemplateSignature("2", "2"),
//                new TemplateSignature("3", "3"),
//                new TemplateSignature("4", "4"),
//                new TemplateSignature("5", "5"),
//            };

//        private static readonly TemplatePage RootPage =
//            new TemplatePage(
//                "ID",
//                "PageName",
//                new List<TemplatePage>()
//                {
//                    new TemplatePage("ID_Child1", "PageName_Child1", "2"),
//                    new TemplatePage(
//                        "ID_Child2",
//                        "PageName_Child2",
//                        new List<TemplatePage>()
//                        {
//                            new TemplatePage("ID_Child21", "PageName_Child21", "5")
//                        },
//                        "3"
//                    ),
//                    new TemplatePage("ID_Child3", "PageName_Child3", "4")
//                },
//                "1"
//            );

//        [Test]
//        public async Task Handle()
//        {
//            // Arrange
//            var signatureRepository = Substitute.For<IRepository<TemplateSignature>>();
//            signatureRepository
//                .GetAll()
//                .Returns(Result.SuccessTask(Signatures));

//            var signatureListRepository = Substitute.For<IRepository<TemplateSignatureList>>();
//            signatureListRepository
//                .GetBy(AppConfiguration.TemplateSignatureListID)
//                .Returns(Result.SuccessTask(SignatureList));

//            var templatePageRepository = Substitute.For<IRepository<TemplatePage>>();
//            templatePageRepository
//                .GetBy(AppConfiguration.TemplatePageRootID)
//                .Returns(Result.SuccessTask(RootPage));

//            templatePageRepository.GetBy("ID_Child1")
//                .Returns(Result.SuccessTask(RootPage.SubPages[0]));
//            templatePageRepository.GetBy("ID_Child2")
//                .Returns(Result.SuccessTask(RootPage.SubPages[1]));
//            templatePageRepository.GetBy("ID_Child3")
//                .Returns(Result.SuccessTask(RootPage.SubPages[2]));
//            templatePageRepository.GetBy("ID_Child21")
//                .Returns(Result.SuccessTask(RootPage.SubPages[1].SubPages[0]));

//            // Act
//            var result = await CreateHandler(
//                signatureRepository, signatureListRepository, templatePageRepository).Handle(Query);
//            var vm = result.Get();

//            // Assert
//            Assert.IsTrue(result.IsSuccess);

//            Assert.AreEqual(vm.Signatures[0].ID, "2");
//            Assert.AreEqual(vm.Signatures[1].ID, "3");
//            Assert.AreEqual(vm.Signatures[2].ID, "1");
//            Assert.AreEqual(vm.Signatures[3].ID, "5");
//            Assert.AreEqual(vm.Signatures[4].ID, "4");
//            Assert.AreEqual(vm.Pages[0].ID, "ID_Child1");
//            Assert.AreEqual(vm.Pages[1].ID, "ID_Child2");
//            Assert.AreEqual(vm.Pages[1].SubPages[0].ID, "ID_Child21");
//            Assert.AreEqual(vm.Pages[2].ID, "ID_Child3");
//        }

//        [Test]
//        public async Task Handle_WhenSignatureRepoEmpty()
//        {
//            // Arrange
//            var signatureRepository = Substitute.For<IRepository<TemplateSignature>>();
//            signatureRepository
//                .GetAll()
//                .Returns(Result.SuccessTask(Array.Empty<TemplateSignature>()));

//            var signatureListRepository = Substitute.For<IRepository<TemplateSignatureList>>();
//            signatureListRepository
//                .GetBy(AppConfiguration.TemplateSignatureRootID)
//                .Returns(Result.SuccessTask());

//            var templatePageRepository = Substitute.For<IRepository<TemplatePage>>();
//            templatePageRepository
//                .GetBy(AppConfiguration.TemplatePageRootID)
//                .Returns(Result.SuccessTask(RootPage));

//            templatePageRepository.GetBy("ID_Child1")
//                .Returns(Result.SuccessTask(RootPage.SubPages[0]));
//            templatePageRepository.GetBy("ID_Child2")
//                .Returns(Result.SuccessTask(RootPage.SubPages[1]));
//            templatePageRepository.GetBy("ID_Child3")
//                .Returns(Result.SuccessTask(RootPage.SubPages[2]));
//            templatePageRepository.GetBy("ID_Child21")
//                .Returns(Result.SuccessTask(RootPage.SubPages[1].SubPages[0]));

//            // Act
//            var result = await CreateHandler(
//                signatureRepository, signatureListRepository, templatewPageRepository).Handle(Query);
//            var vm = result.Get();

//            // Assert
//            Assert.IsTrue(result.IsSuccess);
//            Assert.AreEqual(vm.Signatures.Length, 0);
//        }

//        private IQueryHandler<GetTemplateSectionQuery, GetTemplateSectionQueryDTO> CreateHandler(params object[] repositories) =>
//            Extensions.CreateHandler<GetTemplateSectionQueryHandler>(repositories);
//    }
//}
