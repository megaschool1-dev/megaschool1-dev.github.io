using OneOf;
using OneOf.Types;
using StellarDotnetSdk.Accounts;

namespace Stellar;

public record KeyPairBasic(string AccountId, string SecretSeed);

[GenerateOneOf]
public partial class StellarKeyPair : OneOfBase<KeyPair, KeyPairBasic>
{
    public string AccountId => this.Match(keyPair => keyPair.AccountId, keyPairBasic => keyPairBasic.AccountId);
    public OneOf<string, None> Seed => this.Match(keyPair => keyPair.SecretSeed != null ? OneOf<string, None>.FromT0(keyPair.SecretSeed) : new None(), keyPairBasic => keyPairBasic.SecretSeed);
}
