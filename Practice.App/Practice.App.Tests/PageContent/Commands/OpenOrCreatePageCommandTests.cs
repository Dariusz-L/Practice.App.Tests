using Common.Basic.Blocks;
using Common.Basic.CQRS.Command;
using Common.Basic.Repository;
using NSubstitute;
using NUnit.Framework;
using Practicer.App.Commands;
using Practicer.Domain.Pages.Content;
using System.Threading.Tasks;

namespace Practicer.App.Tests
{
    internal class OpenOrCreatePageCommandTests
    {
        private const string PageID = "PageID";
        private const string PageName = "PageName";
        private static readonly Page Page = new Page(PageID, PageName);

        private OpenOrCreatePageCommand Command = new OpenOrCreatePageCommand(PageID, PageName, "ReferringPageID");

        [Test]
        public async Task GivenCorrectCommand_WhenHandle_ThenPageHasSamePropertiesAsCommand()
        {
            // Arrange
            var repository = Substitute.For<IRepository<Page>>();
            repository.GetBy(PageID).Returns(Result.SuccessTask());
            repository.Save(Arg.Any<Page>()).Returns(Result.SuccessTask(Page));

            // Act
            await CreateHandler(repository).Handle(Command);

            // Assert
            await repository.Received().Save(Arg.Is<Page>(p => p.ID == PageID && p.Name == PageName));
        }

        private ICommandHandler<OpenOrCreatePageCommand> CreateHandler(object repository) =>
            Extensions.CreateHandler<OpenOrCreatePageCommandHandler>(repository);
    }
}
