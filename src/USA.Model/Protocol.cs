using OneOf;
using OneOf.Types;
using Stellar;
using StellarDotnetSdk.Accounts;
using StellarDotnetSdk.Transactions;
using StellarDotnetSdk.Assets;

namespace USA.Model;

public static class Protocol
{
    private static async Task<StellarKeyPair> CreateRandomKeyPairAsync(OneOf<Func<Task<string>>, None> keyPairGenerator)
    {
        if(keyPairGenerator.TryPickT0(out var keyPairGetter, out _))
        {
            return await Util.CreateKeyPairRandomAsync(keyPairGetter);
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

    public static async Task RunAsync()
    {
        //TODO Add memos

        var networkUrl = Wallet.PublicNetworkUrl.Testnet;
        var issuingAccountDomain = HomeDomain.From("megaschool1.me");
        var distributionAccountDomain = HomeDomain.From("thirdoption.party");
        var serviceFeeAccountHomeDomain = HomeDomain.From("thirdoption.party");
        var bootstrapSeed = KeyPair.Random().SecretSeed!;
        var serviceFeeAccountSeed = KeyPair.Random().SecretSeed!;
        var founderFeeAccountSeed = KeyPair.Random().SecretSeed!;

        var bootstrap = new Wallet(
            networkUrl,
            bootstrapSeed,
            new None());

        await Wallet.CreateAccountAsync(
            KeyPair.FromSecretSeed(bootstrapSeed),
            bootstrap.NetworkUrl);
        await bootstrap.InitializeAsync();
        Console.WriteLine($"Bootstrap: {bootstrap.Account.AsT0.KeyPair.AccountId}:{bootstrapSeed}");

        var currency = await Util.CreateCurrencySystemAsync(
            "USA2025",
            100L * 1000 * 1000 * 1000 * 1000,
            Util.MaxTrustlineLimit,
            issuingAccountDomain,
            distributionAccountDomain,
            bootstrap);

        var usa = Asset.CreateNonNativeAsset("USA2025", currency.Issuing.AccountId);

        var serviceFee = new Wallet(
            networkUrl,
            serviceFeeAccountSeed!,
            serviceFeeAccountHomeDomain);
        await Wallet.CreateAccountAsync(KeyPair.FromSecretSeed(serviceFee.AccountSecretSeed.AsT0), serviceFee.NetworkUrl);
        await serviceFee.InitializeAsync();
        Console.WriteLine($"Service Fee: {serviceFee.Account.AsT0.KeyPair.AccountId}:{serviceFeeAccountSeed}");

        var distributor = new Wallet(networkUrl, currency.Distributions.First().SecretSeed, new None());
        await distributor.InitializeAsync();

        var tx = new TransactionBuilder(distributor.Account.AsT0);
        Util.SendPayment(
            usa,
            (100L * 1000 * 1000 * 1000).ToString(),
            distributor.Account.AsT0.KeyPair,
            serviceFee.Account.AsT0.KeyPair,
            tx);

        var txSigned = tx.Build();
        txSigned.Sign(distributor.AccountSecretSeed.MapT0(seed => KeyPair.FromSecretSeed(seed)).AsT0, distributor.NetworkInfo.AsT0);
        txSigned.Sign(serviceFee.AccountSecretSeed.MapT0(seed => KeyPair.FromSecretSeed(seed)).AsT0, serviceFee.NetworkInfo.AsT0);

        await serviceFee.SubmitTransactionAsync(txSigned, false, "Service Fee");

        // send founder's fee
        var founder = new Wallet(
            networkUrl,
            founderFeeAccountSeed,
            new None());
        await Wallet.CreateAccountAsync(KeyPair.FromSecretSeed(founder.AccountSecretSeed.AsT0), founder.NetworkUrl);
        await founder.InitializeAsync();
        Console.WriteLine($"Founder Fee: {founder.Account.AsT0.KeyPair.AccountId}:{founderFeeAccountSeed}");

        var founderFeeTx = new TransactionBuilder(serviceFee.Account.AsT0);
        Util.CreateTrustline(
            usa,
            founder.Account.AsT0.KeyPair,
            founderFeeTx);

        Util.SendPayment(
            usa,
            (10L * 1000 * 1000).ToString(),
            serviceFee.Account.AsT0.KeyPair,
            founder.Account.AsT0.KeyPair,
            founderFeeTx);

        var founderFeeTxSigned = founderFeeTx.Build();
        founderFeeTxSigned.Sign(serviceFee.AccountSecretSeed.MapT0(seed => KeyPair.FromSecretSeed(seed)).AsT0, serviceFee.NetworkInfo.AsT0);
        founderFeeTxSigned.Sign(founder.AccountSecretSeed.MapT0(seed => KeyPair.FromSecretSeed(seed)).AsT0, founder.NetworkInfo.AsT0);

        await serviceFee.SubmitTransactionAsync(founderFeeTxSigned, false, "Founder Fee");
    }
}
