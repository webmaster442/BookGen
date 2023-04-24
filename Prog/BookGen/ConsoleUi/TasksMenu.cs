//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.TaskRunner;
using BookGen.Gui;

namespace BookGen.ConsoleUi;
internal class TasksMenu : MenuBase
{
    private const string Exit = "Exit";

    private readonly BookGenTask[] _tasks;
    private readonly string[] _taskMenu;
    private readonly BookGenTaskRunner _taskRunner;
    private readonly string _directory;

    public TasksMenu(BookGenTaskRunner taskRunner, string directory,  BookGenTask[] tasks)
    {
        _tasks = tasks;
        _taskRunner = taskRunner;
        _directory = directory;
        _taskMenu = _tasks.Select(x => x.Name).Append(Exit).ToArray();
    }

    protected override async Task OnRender(Renderer renderer)
    {
        bool shouldRun = true;
        while (shouldRun)
        {
            renderer.Clear();
            renderer.FigletText("Tasks", ConsoleColor.Magenta);
            renderer.DisplayPath("Working directory", _directory);
            string selected = await renderer.SelectionMenu("Select task", _taskMenu);
            if (selected == "Exit") 
            {
                shouldRun = false;
                continue;
            }
            await RunTask(_tasks.First(x => x.Name == selected), renderer);
            await renderer.WaitKey();
        }
    }

    private async Task RunTask(BookGenTask bookGenTask, Renderer renderer)
    {
        foreach (var item in bookGenTask.Items)
        {
            if (item is InputPrompt prompt)
            {
                var result = await renderer.Prompt(prompt.Message);
                _taskRunner.SetPromptResult(prompt.Varialbe, result);
            }
            else if (item is Confirm confirm)
            {
                var result = await renderer.Confirm(confirm.Message);
                _taskRunner.SetConfirmResult(confirm.Varialbe, result);
            }
            else
            {
                _taskRunner.RunTask(item);
            }
        }
    }
}
