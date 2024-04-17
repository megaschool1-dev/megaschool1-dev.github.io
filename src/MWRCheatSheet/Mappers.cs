using MWRCheatSheet.Model;
using MWRCheatSheet.ViewModel;
using Riok.Mapperly.Abstractions;

namespace MWRCheatSheet;

[Mapper]
public partial class Mappers
{
    public partial EventViewModel EventDtoToEventViewModel(EventDto eventDto);
}