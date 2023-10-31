using BookGen.Domain.Www;

namespace Www;
internal class MainMenu
{
    private readonly WwwUrl[] _favorites;
    private readonly WwwBang[] _bangs;

    public MainMenu(WwwUrl[] favorites, WwwBang[] bangs)
    {
        _favorites = favorites;
        _bangs = bangs;
    }

    internal void Run()
    {
        throw new NotImplementedException();
    }
}
