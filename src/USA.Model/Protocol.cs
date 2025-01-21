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
using StellarDotnetSdk.Assets;

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

    public static void Misc()
    {
        var keyPair = KeyPair.Random();

        Console.WriteLine($"{keyPair.AccountId}:{keyPair.SecretSeed}");
    }

    public static async Task RunAsync(OneOf<IJSRuntime, None> js)
    {
        var networkUrl = Wallet.PublicNetworkUrl.Testnet;
        var issuingAccountDomain = HomeDomain.From("megaschool1.me");
        var distributionAccountDomain = HomeDomain.From("thirdoption.party");
        var serviceFeeAccountHomeDomain = HomeDomain.From("thirdoption.party");
        var serviceFeeAccountSeed = "SCHOE23YEIL6XKKKXXX5JM3Y2FPVQK7QK355TKSVSETRBNNOU6FEAWEA";
        var founderFeeAccountSeed = "SAY5NQMAASBRJ5YHV35FTLJLQ2PTIDGQ3ZJZ2HIBYHDO2X5OUEKIWRIW";

        // send founder's fee
        var founder = new Wallet(
            networkUrl,
            founderFeeAccountSeed,
            new None());
        await Wallet.CreateAccountAsync(KeyPair.FromSecretSeed(founder.AccountSecretSeed.AsT0), founder.NetworkUrl);
        await founder.InitializeAsync();
        
        (KeyPair Issuing, List<KeyPair> Distributions) currency = (KeyPair.FromAccountId("GBPIYT3QUOHNEKYX23NTY2BM24ZWBN3HCPTOHLJM7AXYGFIW5KGXVYJB"), [KeyPair.FromSecretSeed("SANFRSWETMTU5QIKWDYE63EOMA4AOMGGV3WBY7PMCBLSLYCPYRX55BVX")]);
        var usa = Asset.CreateNonNativeAsset("USA2025", currency.Issuing.AccountId);

        var serviceFee = new Wallet(networkUrl, serviceFeeAccountSeed, new None());
        await serviceFee.InitializeAsync();

        var tx = new TransactionBuilder(serviceFee.Account.AsT0);
        Util.CreateTrustline(
            usa,
            founder.Account.AsT0.KeyPair,
            tx);

        Util.SendPayment(
            usa,
            (10L * 1000 * 1000).ToString(),
            serviceFee.Account.AsT0.KeyPair,
            founder.Account.AsT0.KeyPair,
            tx);

        var txSigned = tx.Build();
        txSigned.Sign(serviceFee.AccountSecretSeed.MapT0(seed => KeyPair.FromSecretSeed(seed)).AsT0, serviceFee.NetworkInfo.AsT0);
        txSigned.Sign(founder.AccountSecretSeed.MapT0(seed => KeyPair.FromSecretSeed(seed)).AsT0, founder.NetworkInfo.AsT0);

        await serviceFee.SubmitTransactionAsync(txSigned, false, "Founder Fee");
    }

    private static async Task DoneAsync()
    {
        var networkUrl = Wallet.PublicNetworkUrl.Testnet;
        var issuingAccountDomain = HomeDomain.From("megaschool1.me");
        var distributionAccountDomain = HomeDomain.From("thirdoption.party");
        var serviceFeeAccountHomeDomain = HomeDomain.From("thirdoption.party");
        var serviceFeeAccountSeed = KeyPair.Random().SecretSeed!;

        var keyPair = await CreateRandomKeyPairAsync(new None());
        var wallet = new Wallet(
            networkUrl,
            keyPair.Seed,
            new None());

        await Wallet.CreateAccountAsync(
            keyPair.AccountId,
            wallet.NetworkUrl);
        await wallet.InitializeAsync(keyPair.AccountId);

        var currency = await Util.CreateCurrencySystemAsync(
            "USA2025",
            100L * 1000 * 1000 * 1000 * 1000,
            Util.MaxTrustlineLimit,
            issuingAccountDomain,
            distributionAccountDomain,
            wallet);

        var usa = Asset.CreateNonNativeAsset("USA2025", currency.Issuing.AccountId);

        var serviceFee = new Wallet(
            networkUrl,
            serviceFeeAccountSeed!,
            serviceFeeAccountHomeDomain);
        await Wallet.CreateAccountAsync(KeyPair.FromSecretSeed(serviceFee.AccountSecretSeed.AsT0), serviceFee.NetworkUrl);
        await serviceFee.InitializeAsync();

        var distributor = new Wallet(networkUrl, currency.Distributions.First().SecretSeed, new None());
        await distributor.InitializeAsync();

        var tx = new TransactionBuilder(distributor.Account.AsT0);
        Util.CreateTrustline(
            usa,
            serviceFee.Account.AsT0.KeyPair,
            tx);

        Util.SendPayment(
            usa,
            (100L * 1000 * 1000 * 1000).ToString(),
            distributor.Account.AsT0.KeyPair,
            serviceFee.Account.AsT0.KeyPair,
            tx);

        var txSigned = tx.Build();
        txSigned.Sign(serviceFee.AccountSecretSeed.MapT0(seed => KeyPair.FromSecretSeed(seed)).AsT0, distributor.NetworkInfo.AsT0);
        txSigned.Sign(distributor.AccountSecretSeed.MapT0(seed => KeyPair.FromSecretSeed(seed)).AsT0, distributor.NetworkInfo.AsT0);

        await distributor.SubmitTransactionAsync(txSigned, false, "Service Fee");

    }
}
