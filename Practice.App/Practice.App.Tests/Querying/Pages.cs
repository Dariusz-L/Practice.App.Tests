using NSubstitute;
using System;
using TestApp.Domain;

namespace Practice.App.Tests.Querying
{
    internal static class Pages
    {
        public static readonly IPage HashiKanjiCharacter = Substitute.For<IPage>();
        public static readonly IPage SuHiraganaCharacter = Substitute.For<IPage>();
        public static readonly IPage RuHiraganaCharacter = Substitute.For<IPage>();

        public static readonly IPage WordFormationPageValue = Substitute.For<IPage>();
        public static readonly IPage WordFormationPage = Substitute.For<IPage>();

        public static readonly IPage[] Collection = new[]
        {
            HashiKanjiCharacter,
            SuHiraganaCharacter,
            RuHiraganaCharacter,
            WordFormationPageValue,
            WordFormationPage
        };

        public static void Setup()
        {
            HashiKanjiCharacter.SetPage("HashiKanjiCharacterID", "走", "Kanji Character");
            RuHiraganaCharacter.SetPage("RuHiraganaCharacterID", "る", "Hiragana Character");
            SuHiraganaCharacter.SetPage("SuHiraganaCharacterID", "す", "Hiragana Character");
            WordFormationPageValue.SetPage("WordFormationPageValueID", "走る", "Word Formation Value");
            WordFormationPage.SetPage("WordFormationPageID", "Affirmative", "Word Formation", WordFormationPageValue);
        }
    }

    public static class PageExtensions
    {
        public static void SetPage(this IPage page, string id, string name, string templateName)
        {
            page.ID.Returns(id);
            page.Name.Returns(name);
            page.TemplateName.Returns(templateName);
            page.Items.Returns(Array.Empty<IPage>());
        }

        public static void SetPage(this IPage page, string id, string name, string templateName, params IPage[] items)
        {
            page.SetPage(id, name, templateName);
            page.Items.Returns(items);
        }
    }
}
