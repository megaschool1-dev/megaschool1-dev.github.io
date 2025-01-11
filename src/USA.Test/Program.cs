using Stellar;
using StellarDotnetSdk.Accounts;
using TUnit.Core;

namespace USA.Test;

public class Program
{
    [Test]
    public void Test1()
    {
        var wallet = new Wallet(
            "https://localhost:8081",
            KeyPair.Random().SecretSeed,
            "https://thirdoption.party");
    }
}
