using Common.Basic.Blocks;
using Common.Basic.CQRS.Command;
using Common.Basic.Repository;
using NSubstitute;
using NUnit.Framework;
using Practicer.App.Commands;
using Practicer.Domain.Pages.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Practicer.App.Tests
{
    internal class RemovePropertyCommandTests
    {
        private const string ChildID = "ChildID";
        private static readonly Page ChildPage = new Page(ChildID, ChildID);

        private const string ParentID = "ParentID";
        private static readonly Page ParentPage = new Page(ParentID, 0, ParentID, 
            new List<string>() { ChildID },
            new List<SubPage>() { new SubPage(ChildID) },
            new List<Link>() {},
            new List<SubProperty>() {});

        private RemovePropertyCommand Command = new RemovePropertyCommand(ParentID, ChildID);

        [Test]
        public async Task Remove()
        {
            // Arrange
            var repository = Substitute.For<IRepository<Page>>();
            repository.GetBy(ParentID).Returns(Result.SuccessTask(ParentPage));
            repository.Save(ParentPage).Returns(Result.SuccessTask());

            // Act
            await CreateHandler(repository).Handle(Command);

            // Assert
            await repository.Received().Save(Arg.Is<Page>(p => p.ID == ParentID));
        }

        private ICommandHandler<RemovePropertyCommand> CreateHandler(object repository) =>
            Extensions.CreateHandler<RemovePropertyCommandHandler>(repository);
    }
}
