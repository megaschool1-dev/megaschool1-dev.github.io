using Stellar;
using StellarDotnetSdk.Assets;

namespace USA.Model;

public record CurrencySystem(AssetTypeCreditAlphaNum Asset, KeyPairBasic Issuing, KeyPairBasic[] Distribution);