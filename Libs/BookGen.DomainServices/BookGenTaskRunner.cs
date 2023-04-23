//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.TaskRunner;

namespace BookGen.DomainServices;

public class BookGenTaskRunner
{
    private readonly Dictionary<string, bool> _confirmVars;
    private readonly Dictionary<string, string> _promptResults;

    public BookGenTaskRunner()
    {
        _confirmVars = new Dictionary<string, bool>();
        _promptResults = new Dictionary<string, string>();
    }

    public void SetConfirmResult(string varialbe, bool result)
    {
        _confirmVars[varialbe] = result;
    }

    public void SetPromptResult(string varialbe, string result)
    {
        _promptResults[varialbe] = result;
    }

    public Task RunTask(BookGenTask bookGenTask)
    {
        throw new NotImplementedException();
    }
}
