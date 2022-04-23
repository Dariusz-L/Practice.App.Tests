using Common.Basic.Blocks;
using Common.Basic.CQRS.Command;
using Common.Basic.Repository;
using NSubstitute;
using NUnit.Framework;
using Practicer.App.Commands;
using Practicer.Domain.Pages.Content;
using System.Threading;
using System.Threading.Tasks;

namespace Practicer.App.Tests
{
    internal class CreateSubPageCommandTests
    {
        private const string ParentID = "ParentID";
        private const string ParentName = "ParentName";
        private const string PropertyName = "PropertyName";
        private static readonly Page ParentPage = new Page(ParentID, ParentName);

        private CreateSubPageCommand Command = new CreateSubPageCommand(ParentID, PropertyName);

        [Test]
        public async Task GivenCorrectCommand_WhenHandle_ThenSavedPageAndSavedNewOne()
        {
            var currentSyncContext = SynchronizationContext.Current;

            // Arrange
            var repository = Substitute.For<IRepository<Page>>();
            repository.GetBy(ParentID).Returns(Result.SuccessTask(ParentPage));
            repository.Save(Arg.Any<Page>()).Returns(Result.SuccessTask());

            // Act
            await CreateHandler(repository).Handle(Command);

            // Assert 
            await repository.Received().Save(Arg.Is<Page>(p => p.ID == ParentID && p.Name == ParentName));
            await repository.Received().Save(Arg.Is<Page>(p => p.ID != ParentID && p.Name == PropertyName));
        }

        private ICommandHandler<CreateSubPageCommand, string> CreateHandler(object repository) =>
            Extensions.CreateHandler<CreateSubPageCommandHandler>(repository);
    }
}
