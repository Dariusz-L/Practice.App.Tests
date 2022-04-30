using Common.Basic.Blocks;
using Common.Basic.CQRS.Command;
using Common.Basic.Repository;
using NSubstitute;
using NUnit.Framework;
using Practicer.App.Templates;
using Practicer.Domain.Templates;
using Shimi;
using System;
using System.Threading.Tasks;

namespace Practicer.App.Tests
{
    internal class CreateTemplateSignatureCommandTests
    {
        private const string Name = "Template";
        private const int Index = 0;

        private readonly TemplateSignature TemplateSignature = new TemplateSignature("ID", Name);
        private readonly TemplateSignatureList TemplateSignatureList = new TemplateSignatureList(Name);

        private CreateTemplateSignatureCommand Command = new CreateTemplateSignatureCommand(Name, Index);

        [Test]
        public async Task Create_WhenNotExistsOfName()
        {
            // Arrange
            var sListRepo = Substitute.For<IRepository<TemplateSignatureList>>();
            sListRepo.GetBy(AppConfiguration.TemplateSignatureListID)
                .Returns(Result.SuccessTask());
            sListRepo.Save(Arg.Any<TemplateSignatureList>())
                .Returns(Result.SuccessTask(TemplateSignatureList));

            var sRepo = Substitute.For<IRepository<TemplateSignature>>();
            sRepo.ExistsOfName(Arg.Any<string>(), Arg.Any<Func<TemplateSignature, string>>())
                .Returns(Result.SuccessTask(false));
            sRepo.Save(Arg.Any<TemplateSignature>())
                .Returns(Result.SuccessTask(TemplateSignature));

            // Act
            await CreateHandler(sRepo, sListRepo).Handle(Command);

            // Assert
            await sRepo.Received().Save(Arg.Any<TemplateSignature>());
            await sRepo.Received().ExistsOfName(Arg.Any<string>(), Arg.Any<Func<TemplateSignature, string>>());
            await sListRepo.Received().GetBy(Arg.Any<string>());
            await sListRepo.Received().Save(Arg.Any<TemplateSignatureList>());
        }

        [Test]
        public async Task CreateNot_WhenExistsOfName()
        {
            // Arrange
            var sListRepo = Substitute.For<IRepository<TemplateSignatureList>>();
            sListRepo.GetBy(AppConfiguration.TemplateSignatureListID)
                .Returns(Result.SuccessTask(TemplateSignatureList));

            var sRepo = Substitute.For<IRepository<TemplateSignature>>();
            sRepo.GetBy(Arg.Any<string>())
                .Returns(Result.SuccessTask(TemplateSignature));
            sRepo.ExistsOfName(Arg.Any<string>(), Arg.Any<Func<TemplateSignature, string>>())
                .Returns(Result<bool>.SuccessTask(true));

            // Act
            await CreateHandler(sRepo, sListRepo).Handle(Command);

            // Assert
            await sRepo.DidNotReceive().Save(Arg.Any<TemplateSignature>());
            await sRepo.Received().ExistsOfName(Arg.Any<string>(), Arg.Any<Func<TemplateSignature, string>>());
            await sListRepo.Received().GetBy(Arg.Any<string>());
            await sListRepo.DidNotReceive().Save(Arg.Any<TemplateSignatureList>());
        }

        private ICommandHandler<CreateTemplateSignatureCommand> CreateHandler(params object[] repositories) =>
            Extensions.CreateHandler<CreateTemplateSignatureCommandHandler>(repositories);
    }
}
