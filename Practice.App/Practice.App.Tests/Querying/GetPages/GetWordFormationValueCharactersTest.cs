using Common.Basic.Blocks;
using Common.Basic.Repository;
using NSubstitute;
using NUnit.Framework;
using Practicer.App.Querying;
using System.Collections.Generic;
using TestApp.Domain;

namespace Practice.App.Tests.Querying.GetPages
{
    internal class GetWordFormationValueCharactersTest
    {
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

        [OneTimeSetUp]
        public void Setup() =>
            Pages.Setup();

        //[Test]
        //public void Get()
        //{
        //    string inputPageID = "inputPageID";
        //    string queryID = "queryID";

        //    var pageRepository = Substitute.For<IRepositoryNoTask<IPage>>();
        //    pageRepository.GetBy(inputPageID).Returns(Pages.WordFormationPage.ToResult());
        //    pageRepository.GetAll().Returns(Pages.Collection.ToResult());

        //    var queryRepository = Substitute.For<IRepositoryNoTask<QueryDTO>>();
        //    queryRepository.GetBy(queryID).Returns(query.ToResult());

        //    var handler = new GetQueryPagesQueryHandler(pageRepository, null, queryRepository);

        //    var pages = handler.Execute(inputPageID, queryID);

        //    Assert.True(pages.Length == 2);
        //    Assert.True(pages[0] == Pages.HashiKanjiCharacter);
        //    Assert.True(pages[1] == Pages.RuHiraganaCharacter);
        //}
    }
}
