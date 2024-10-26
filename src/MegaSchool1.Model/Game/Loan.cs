using OneOf;
using OneOf.Types;

namespace MegaSchool1.Model.Game;

public record Loan(string Description, decimal Balance, Percentage Apy, OneOf<decimal, None> MinimumMonthlyPayment);
