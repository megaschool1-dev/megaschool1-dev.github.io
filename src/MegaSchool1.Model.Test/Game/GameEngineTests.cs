using System.Transactions;
using FluentAssertions;
using FsCheck;
using MegaSchool1.Model.Game;
using MegaSchool1.Model.Game.Expense;
using NUnit.Framework;

namespace MegaSchool1.Model.Test.Game;

[TestFixture]
public class GameEngineTests
{
    private static Gen<GameState> ModerateGame() => Gen.Fresh(() => GameState.Moderate());

    private static GameState BarFunc(GameState game) { GameEngine.SummonTreasureMasters(game); return game; }

    private static Gen<Func<GameState, GameState>> Bar => Gen.Constant(BarFunc);

    private static Gen<GameState> WealthMembershipSummon(Gen<GameState> game) => game.Apply(Gen.Constant((GameState game) =>
    {
        GameEngine.SummonTreasureMasters(game);
        return game;
    }));

    private static GameState InstantPayRaiseSummon(GameState g)
    {
        GameEngine.InstantPayRaise(g);

        return g;
    }

    private static GameState BillNegotiatorSummon(GameState g)
    {
        GameEngine.SummonBillNegotiator(g);

        return g;
    }

    private static GameState HealthSharing(GameState game)
    {
        GameEngine.SummonHealthSharing(game);
        return game;
    }

    private static Gen<Func<GameState, GameState>[]> IndividualWealthMembershipServices(params Func<GameState, GameState>[] exclude) =>
        Gen.Shuffle([
            InstantPayRaiseSummon,
            BillNegotiatorSummon,
            HealthSharing
        ])
        .Select(services => services.Except(exclude).ToArray());

    private static Gen<Func<GameState, GameState>> RandomWealthMembershipService(params Func<GameState, GameState>[] exclude) =>
        IndividualWealthMembershipServices(exclude).Select(services => services.First());

    [FsCheck.NUnit.Property]
    public Property WealthMembership()
    {
       return Prop.ForAll(ModerateGame().ToArbitrary(), RandomWealthMembershipService().ToArbitrary(),
            (game, addServiceFunc) =>
            {
                GameEngine.SummonTreasureMasters(game);
                var checkingBefore = game.CheckingAccountBalance;

                addServiceFunc(game);

                game.CheckingAccountBalance
                    .Should().Be(checkingBefore);
            });
    }

    [FsCheck.NUnit.Property]
    public Property IndividualFinancialService()
    {
        return Prop.ForAll(ModerateGame().ToArbitrary(), RandomWealthMembershipService(InstantPayRaiseSummon, BillNegotiatorSummon).ToArbitrary(),
            (game, addServiceFunc) =>
            {
                var checkingBefore = game.CheckingAccountBalance;

                addServiceFunc(game);
                
                game.CheckingAccountBalance
                    .Should().BeLessThan(checkingBefore,  $"Expenses: {string.Join(',', game.Expenses.Select(e => e.Description))}");
            });
    }

    [FsCheck.NUnit.Property]
    public Property MoneyBackGuarantee()
    {
        return Prop.ForAll(ModerateGame().ToArbitrary(),
            game =>
            {
                // arrange
                var checkingBefore = game.CheckingAccountBalance;

                // act
                var monthlySavings = GameEngine.SummonTreasureMasters(game);

                // assert
                if (monthlySavings.TryPickT0(out var savings, out _))
                {
                    savings.Value
                        .Should().BePositive($"{monthlySavings}");

                    game.CheckingAccountBalance
                        .Should().BeGreaterThan(checkingBefore, $"{monthlySavings}");
                }
                else
                {
                    game.CheckingAccountBalance
                        .Should().BeGreaterOrEqualTo(checkingBefore, $"{monthlySavings}");
                }
            });
    }
}