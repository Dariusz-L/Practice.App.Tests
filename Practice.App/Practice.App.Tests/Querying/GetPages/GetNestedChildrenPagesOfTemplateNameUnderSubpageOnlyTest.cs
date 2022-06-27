using Common.Basic.Blocks;
using Common.Basic.Files;
using Common.Basic.Json;
using Common.Basic.Mapping;
using Common.Basic.Repository;
using NSubstitute;
using NUnit.Framework;
using Practicer.App.Querying;
using Practicer.Domain.Pages.Content;
using Practicer.Domain.Templates;
using TestApp.Domain;

namespace Practice.App.Tests.Querying.GetPages
{
    internal class GetNestedChildrenPagesOfTemplateNameUnderSubpageOnlyTest
    {
        private static readonly QueryDTO query = new QueryDTO()
        {
            Name = "Words",
            Expressions = new ExpressionDTO[]
            {
                new ExpressionDTO()
                {
                    Type = ExpressionType.GetNestedChildrenPagesOfTemplateNameUnderSubpageOnly,
                    Operand = new OperandDTO()
                    {
                        VariableName = VariableNames.Input
                    },
                    Parameters = new ParametersDTO()
                    {
                        Type = ParametersType.String, // Template Signature Name
                        StringParameter = "Word"
                    },
                    OutputName = "Words Under Subpage Only"
                },
            }
        };

        [OneTimeSetUp]
        public void Setup() =>
            Pages.Setup();

        [Test]
        public void Get()
        {
            const string inputPageID = "6f1c113e-259e-4ffe-82d8-f95321203fe5"; // Misa Ammo
            const string queryID = "queryID";

            GetRepositories(
                out var pageRepository,
                out var templateSignatureRepository);

            var queryRepository = Substitute.For<IRepositoryNoTask<QueryDTO>>();
            queryRepository.GetBy(queryID).Returns(query.ToResult());

            var handler = new GetQueryPagesQueryHandler(pageRepository, templateSignatureRepository, queryRepository);

            var pages = handler.Execute(inputPageID, queryID);

            Assert.True(pages.Length == 2);
            Assert.True(pages[0] == Pages.HashiKanjiCharacter);
            Assert.True(pages[1] == Pages.RuHiraganaCharacter);
        }

        private static void GetRepositories(
            out IRepository<Page> pageRepository,
            out IRepository<TemplateSignature> templateSignatureRepository)
        {
            var jsonConverter = new NewtonsoftJsonConverter();
            var dirOps = new DirectoryOperations();
            var fileOps = new FileOperations();

            var dataPath = @"C:\Users\dariu\AppData\LocalLow\daretsuki\Practice-Prototype";

            pageRepository = 
                new LocalStorageRepository<Page>(dataPath + @"\Pages", dirOps, fileOps, jsonConverter);

            templateSignatureRepository = 
                new LocalStorageRepository<TemplateSignature>(dataPath + @"\TemplateSignatures", dirOps, fileOps, jsonConverter);
        }
    }
}
