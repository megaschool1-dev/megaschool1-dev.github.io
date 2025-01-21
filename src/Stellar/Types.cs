using OneOf;
using OneOf.Types;
using StellarDotnetSdk.Accounts;
using ValueOf;

namespace Stellar;

public record KeyPairBasic(string AccountId, string SecretSeed);

public class HomeDomain : ValueOf<string, HomeDomain>
{
    protected override void Validate()
    {
        if (!TryValidate())
        {
            throw new Exception($"'{Value}' should be a URL domain WITHOUT the 'https://' and WITHOUT the trailing '/'!");
        }
    }

    protected override bool TryValidate() => Uri.IsWellFormedUriString($"https://{this.Value}/", UriKind.Absolute);
}

[GenerateOneOf]
public partial class StellarKeyPair : OneOfBase<KeyPair, KeyPairBasic>
{
    public string AccountId => this.Match(keyPair => keyPair.AccountId, keyPairBasic => keyPairBasic.AccountId);
    public OneOf<string, None> Seed => this.Match(keyPair => keyPair.SecretSeed != null ? OneOf<string, None>.FromT0(keyPair.SecretSeed) : new None(), keyPairBasic => keyPairBasic.SecretSeed);
}
