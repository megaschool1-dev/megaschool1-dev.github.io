using OneOf.Types;
using Stellar;
using StellarDotnetSdk.Accounts;
using StellarDotnetSdk.Transactions;
using TUnit.Core;
using USA.Model;

namespace USA.Test;

public class Program
{
    [Test]
    public async Task Test1()
    {
        await Protocol.RunAsync(new None());
    }
}
