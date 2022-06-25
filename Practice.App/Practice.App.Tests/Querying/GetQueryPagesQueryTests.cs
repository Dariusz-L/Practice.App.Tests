using Common.Basic.Repository;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TestApp.App;
using TestApp.Domain;
using Common.Basic.Blocks;

namespace Practice.App.Tests.Querying
{
    internal class GetQueryPagesQueryTests
    {
        private static readonly IPage HashiKanjiCharacter = Substitute.For<IPage>();
        private static readonly IPage SuHiraganaCharacter = Substitute.For<IPage>();
        private static readonly IPage RuHiraganaCharacter = Substitute.For<IPage>();

        private static readonly IPage WordFormationPageValue = Substitute.For<IPage>();
        private static readonly IPage WordFormationPage = Substitute.For<IPage>();

        private static readonly IPage[] Pages = new[]
        {
            HashiKanjiCharacter,
            SuHiraganaCharacter,
            RuHiraganaCharacter,
            WordFormationPageValue,
            WordFormationPage
        };

        private static readonly QueryDTO query = new QueryDTO()
        {
            Name = "Characters",
            Expressions = new ExpressionDTO[]
            {
                new ExpressionDTO()
                {
                    Type = ExpressionType.FirstIf,
                    Operand = new OperandDTO()
                    {
                        VariableName = VariableNames.Input,
                        Type = PropertyType.Items
                    },
                    Parameters = new ParametersDTO()
                    {
                        Type = ParametersType.Predicate,
                        Predicate = new PredicateDTO()
                        {
                            Conditions = new ConditionDTO[]
                            {
                                new ConditionDTO()
                                {
                                    LeftVariableName = VariableNames.Iterator,
                                    LeftVariablePropertyType = PropertyType.TemplateName,

                                    Operator = ConditionOperatorType.Equals,
                                    RightVariableOperand = new ConditionOperand()
                                    {
                                        Type = ConditionOperandType.Template,
                                        TemplateName = "Word Formation Value"
                                    }
                                }
                            },

                        }
                    },
                    OutputName = "Word Formation Value"
                },

                new ExpressionDTO()
                {
                    Type = ExpressionType.GetPagesOfTemplateName,
                    Operand = new OperandDTO()
                    {
                        VariableName = VariableNames.Pages,
                        Type = PropertyType.This
                    },
                    Parameters = new ParametersDTO()
                    {
                        Type = ParametersType.StringArray,
                        StringArrayParameter = new List<string>()
                        {
                            "Hiragana Character",
                            "Katakana Character",
                            "Kanji Character"
                        }
                    },
                    OutputName = "Character Pages"
                },

                new ExpressionDTO()
                {
                    Type = ExpressionType.Where,
                    Operand = new OperandDTO()
                    {
                        VariableName = "Character Pages",
                        Type = PropertyType.This
                    },
                    Parameters = new ParametersDTO()
                    {
                        Type = ParametersType.Predicate,
                        Predicate = new PredicateDTO()
                        {
                            Conditions = new ConditionDTO[]
                            {
                                new ConditionDTO()
                                {
                                    LeftVariableName = VariableNames.Iterator,
                                    LeftVariablePropertyType = PropertyType.Name,

                                    Operator = ConditionOperatorType.EqualsAny,
                                    RightVariableOperand = new ConditionOperand()
                                    {
                                        Type = ConditionOperandType.Variable,
                                        VariableName = "Word Formation Value",
                                        VariablePropertyType = PropertyType.Name,
                                    }
                                }
                            },

                        }
                    },
                    OutputName = "Word Formation Value Character Pages"
                }
            }
        };

        [SetUp]
        public void Setup()
        {
            HashiKanjiCharacter.SetPage("HashiKanjiCharacterID", "走", "Kanji Character");
            RuHiraganaCharacter.SetPage("RuHiraganaCharacterID", "る", "Hiragana Character");
            SuHiraganaCharacter.SetPage("SuHiraganaCharacterID", "す", "Hiragana Character");
            WordFormationPageValue.SetPage("WordFormationPageValueID", "走る", "Word Formation Value");
            WordFormationPage.SetPage("WordFormationPageID", "Affirmative", "Word Formation", WordFormationPageValue);
        }

        [Test]
        public void Test1()
        {
            string inputPageID = "inputPageID";
            string queryID = "queryID";

            var pageRepository = Substitute.For<IRepositoryNoTask<IPage>>();
            pageRepository.GetBy(inputPageID).Returns(WordFormationPage.ToResult());
            pageRepository.GetAll().Returns(Pages.ToResult());

            var queryRepository = Substitute.For<IRepositoryNoTask<QueryDTO>>();
            queryRepository.GetBy(queryID).Returns(query.ToResult());

            var handler = new GetQueryPagesQueryHandler(pageRepository, queryRepository);

            var pages = handler.Execute(inputPageID, queryID);

            Assert.True(pages.Length == 2);
            Assert.True(pages[0] == HashiKanjiCharacter);
            Assert.True(pages[1] == RuHiraganaCharacter);
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
