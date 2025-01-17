using Microsoft.JSInterop;
using OneOf;
using OneOf.Types;
using Stellar;
using StellarDotnetSdk;
using StellarDotnetSdk.Accounts;
using StellarDotnetSdk.Operations;
using StellarDotnetSdk.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USA.Model;

public static class Protocol
{
    private static async Task<StellarKeyPair> CreateRandomKeyPairAsync(OneOf<IJSRuntime, None> js)
    {
        if(js.TryPickT0(out var javascript, out _))
        {
            return await Util.CreateKeyPairRandomAsync(javascript);
        }
        else
        {
            return KeyPair.Random();
        }
    }

    public static async Task RunAsync(OneOf<IJSRuntime, None> js)
    {
        var keyPair = await CreateRandomKeyPairAsync(js);
        var wallet = new Wallet(
          "https://localhost:8081",
          keyPair.Seed,
          "https://thirdoption.party");

        await Wallet.CreateAccountAsync(
            keyPair.AccountId,
            wallet.NetworkUrl);
        await wallet.InitializeAsync(keyPair.AccountId);

        var currency = await Util.CreateCurrencySystemAsync(
            "USA2025",
            100L * 1000 * 1000 * 1000 * 1000,
            Util.MaxTrustlineLimit,
            wallet);

        // send USA creator first installment 10M USA
        var usaCreator = new Wallet(
            "https://localhost:8081",
            KeyPair.Random().SecretSeed!,
            "https://megaschool1.me");
        await Wallet.CreateAccountAsync(KeyPair.FromSecretSeed(usaCreator.AccountSecretSeed.AsT0), usaCreator.NetworkUrl);
        await usaCreator.InitializeAsync();

        //Util.CreateTrustline()
    }
}
