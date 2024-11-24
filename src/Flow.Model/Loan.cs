using Foundation.Model;
using OneOf;
using OneOf.Types;

namespace Flow.Model;

public record Loan(string Description, decimal Balance, Percentage Apy, OneOf<decimal, None> MinimumMonthlyPayment);
