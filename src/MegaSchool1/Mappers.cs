using MegaSchool1.Model;
using MegaSchool1.Model.Dto;
using MegaSchool1.ViewModel;
using OneOf;
using OneOf.Types;
using Riok.Mapperly.Abstractions;

namespace MegaSchool1;

[Mapper]
public partial class Mappers
{
    [MapProperty(nameof(EventDto.StartDate), nameof(EventViewModel.StartDate), Use = nameof(NullableToOption))]
    public partial EventViewModel EventDtoToEventViewModel(EventDto eventDto);

    public static OneOf<Video, None> TestimonialToVideo(Testimonial testimonial) => GetVideo(testimonial.Video);

    private static OneOf<Video, None> GetVideo(ShareableDto dto) => dto.Platform switch
    {
        VideoPlatform.YouTube => !string.IsNullOrWhiteSpace(dto.Id) ? (Video)new YouTube(dto.Id) : new None(),
        VideoPlatform.TikTok => !string.IsNullOrWhiteSpace(dto.Id) && !string.IsNullOrWhiteSpace(dto.UserHandle) ? (Video)new TikTok(dto.UserHandle, dto.Id) : new None(),
        VideoPlatform.Vimeo => !string.IsNullOrWhiteSpace(dto.Id) ? (Video)new Vimeo(dto.Id, !string.IsNullOrWhiteSpace(dto.Hash) ? dto.Hash : new None()) : new None(),
        VideoPlatform.Facebook => !string.IsNullOrWhiteSpace(dto.Id) && !string.IsNullOrWhiteSpace(dto.UserHandle) ? (Video)new Facebook(dto.UserHandle, dto.Id) : new None(),
        VideoPlatform.StartMeeting => !string.IsNullOrWhiteSpace(dto.Id) ? (Video)new StartMeeting(dto.Id) : new None(),
        VideoPlatform.Html5 => Uri.IsWellFormedUriString(dto.Url, UriKind.Absolute) ? (Video)new Html5(new(dto.Url)) : new None(),
        _ => new None()
    };

    public ShareableViewModel ShareableDtoToViewModel(ShareableDto dto)
    {
        var viewModel = new ShareableViewModel();

        viewModel.Id = dto.ContentId;
        viewModel.Video = GetVideo(dto).MapT0(v => new VideoViewModel(v, dto.Duration ?? TimeSpan.MinValue));
        viewModel.Url = !string.IsNullOrWhiteSpace(dto.Url) ? dto.Url : new None();
        viewModel.AppDescription = dto.AppTitle;
        viewModel.Title = dto.ShareableTitle;
        viewModel.ShareableImage = dto.Image ?? viewModel.ShareableImage;
        viewModel.CapturePageImage = dto.CapturePageImage ?? viewModel.CapturePageImage;
        viewModel.Download = !string.IsNullOrWhiteSpace(dto.DownloadUrl) ? (!string.IsNullOrWhiteSpace(dto.DownloadText) ? dto.DownloadText : dto.DownloadUrl, new Uri(dto.DownloadUrl)) : new None();

        var validPromo =
            // valid promo prompt
            !string.IsNullOrWhiteSpace(dto.Promo)
            &&
            // valid promo expiration
            dto.PromoExpiration != null;
        viewModel.Promo = validPromo ? (dto.Promo!, dto.PromoExpiration!.Value) : ("For $100 off, text", Constants.FinancialIndependenceMonthPromoExpiration);

        viewModel.Event = dto.Event != null ? EventDtoToEventViewModel(dto.Event) : new None();
        viewModel.ShowBusinessSignUp = dto.ShowBusinessSignUp;
        viewModel.HideShortCodePrompt = dto.HideShortCodePrompt;
        viewModel.Images = dto.Images.Any() ? dto.Images : new None();
        viewModel.AuxiliaryText = !string.IsNullOrWhiteSpace(dto.AuxiliaryText) ? dto.AuxiliaryText : new None();
        viewModel.Metadata = !string.IsNullOrWhiteSpace(dto.Metadata) ? dto.Metadata : new None();

        return viewModel;
    }

    public static OneOf<T, None> GetNullableToOption<T>(T? value) where T : struct => value.HasValue ? value.Value : new None();
    public static OneOf<DateTimeOffset, None> NullableToOption(DateTimeOffset? value) => GetNullableToOption(value);
}