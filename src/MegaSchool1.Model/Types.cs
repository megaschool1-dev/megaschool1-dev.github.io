using OneOf;

namespace MegaSchool1.Model;

[GenerateOneOf]
public partial class ImageRef : OneOfBase<Image, Uri>;