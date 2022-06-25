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

        private CreateTemplateSignatureCommand Command = new CreateTemplateSignatureCommand(AppConfiguration.TemplateSignatureRootID, Index, Name);

        //[Test]
        //public async Task Create_WhenNotExistsOfName()
        //{
        //    // Arrange
        //    var sRepo = Substitute.For<IRepository<TemplateSignature>>();
        //    sRepo.ExistsOfName(Arg.Any<string>(), Arg.Any<Func<TemplateSignature, string>>())
        //        .Returns(Result.SuccessTask(false));
        //    sRepo.Save(Arg.Any<TemplateSignature>())
        //        .Returns(Result.SuccessTask(TemplateSignature));

        //    // Act
        //    await CreateHandler(sRepo).Handle(Command);

        //    // Assert
        //    await sRepo.Received().Save(Arg.Any<TemplateSignature>());
        //    await sRepo.Received().ExistsOfName(Arg.Any<string>(), Arg.Any<Func<TemplateSignature, string>>());
        //}

        //[Test]
        //public async Task CreateNot_WhenExistsOfName()
        //{
        //    // Arrange
        //    var sRepo = Substitute.For<IRepository<TemplateSignature>>();
        //    sRepo.GetBy(Arg.Any<string>())
        //        .Returns(Result.SuccessTask(TemplateSignature));
        //    sRepo.ExistsOfName(Arg.Any<string>(), Arg.Any<Func<TemplateSignature, string>>())
        //        .Returns(Result<bool>.SuccessTask(true));

        //    // Act
        //    await CreateHandler(sRepo).Handle(Command);

        //    // Assert
        //    await sRepo.DidNotReceive().Save(Arg.Any<TemplateSignature>());
        //    await sRepo.Received().ExistsOfName(Arg.Any<string>(), Arg.Any<Func<TemplateSignature, string>>());
        //}

        private ICommandHandler<CreateTemplateSignatureCommand> CreateHandler(params object[] repositories) =>
            Extensions.CreateHandler<CreateTemplateSignatureCommandHandler>(repositories);
    }
}
