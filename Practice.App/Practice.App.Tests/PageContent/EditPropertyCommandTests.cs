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
    internal class EditPropertyCommandTests
    {
        private const string PropertyID = "PropertyID";
        private const string OldName = "Name";
        private const string NewName = "Name";
        private static readonly Page PropertyPage = new Page(PropertyID, OldName);

        private EditPropertyCommand Command = new EditPropertyCommand(PropertyID, NewName);

        [Test]
        public async Task GivenCorrectCommand_WhenHandle_ThenPageHasSamePropertiesAsCommand()
        {
            // Arrange
            var repository = Substitute.For<IRepository<Page>>();
            repository.GetBy(PropertyID).Returns(Result.SuccessTask(PropertyPage));

            // Act
            await CreateHandler(repository).Handle(Command);

            // Assert
            await repository.Received().Save(Arg.Is<Page>(p => p.Name == NewName));
        }

        private ICommandHandler<EditPropertyCommand> CreateHandler(object repository) =>
            Extensions.CreateHandler<EditPropertyCommandHandler>(repository);
    }
}
