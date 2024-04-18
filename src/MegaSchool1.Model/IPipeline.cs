namespace MWRCheatSheet.Model;

public interface IPipeline<TSteps> where TSteps : struct, Enum
{
	string DisplayName(TSteps step);
}
