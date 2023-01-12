using BookGen.Gui;
using BookGen.Gui.MenuEnums;

namespace BookGen.ConsoleUi;

public sealed class MainMenu : MenuBase
{
    protected override async Task OnRender(Renderer renderer)
    {
        renderer.Clear();
        renderer.FigletText("BookGen Gui", ConsoleColor.Green);
        renderer.BlankLine(2);

        await renderer.SelectionMenu("Select acion", GetEnumItems<MainMenuAction>());
    }
}
