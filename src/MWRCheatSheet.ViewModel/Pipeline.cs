using MWRCheatSheet.Model;

namespace MWRCheatSheet.ViewModel;

public abstract class Pipeline<TSteps> : IPipeline<TSteps>
    where TSteps : struct, Enum
{
    public TSteps FirstStep { get; } = Enum.GetValues<TSteps>().First(step => ((int)(object)step) != 0);
    public abstract string DisplayName(TSteps step);
}
