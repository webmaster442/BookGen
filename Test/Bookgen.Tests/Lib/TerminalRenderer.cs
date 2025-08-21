using Bookgen.Lib.Markdown.Renderers;

namespace Bookgen.Tests.Lib;

internal class UT_TerminalRenderer
{
    private const string Input =
        """
        \e[0mAlapértelmezett szöveg
        \e[1mFélkövér (bold)\e[0m
        \e[3mDőlt (italic)\e[0m
        \e[4mAláhúzott (underline)\e[0m
        \e[30m\e[43mFekete előtér + Sárga háttér\e[0m
        \e[31mPiros előtér\e[0m
        \e[32mZöld előtér\e[0m
        \e[33mSárga előtér\e[0m
        \e[34mKék előtér\e[0m
        \e[35mBíbor előtér\e[0m
        \e[36mCián előtér\e[0m
        \e[37mFehér előtér\e[0m
        \e[40mFekete háttér\e[0m
        \e[41mPiros háttér\e[0m
        \e[42mZöld háttér\e[0m
        \e[43mSárga háttér\e[0m
        \e[44mKék háttér\e[0m
        \e[45mBíbor háttér\e[0m
        \e[46mCián háttér\e[0m
        \e[47m\e[30mFehér háttér + Fekete szöveg\e[0m
        \e[38;2;255;128;0m24 bites előtér (narancs)\e[0m
        \e[48;2;0;128;255m24 bites háttér (világoskék)\e[0m
        """;

    private const string ExpectedOutput =
        """
        Alapértelmezett szöveg
        <span style="font-weight:bold;">Félkövér (bold)</span>
        <span style="font-style:italic;">Dőlt (italic)</span>
        <span style="text-decoration:underline;">Aláhúzott (underline)</span>
        <span style="color:black;"><span style="background-color:yellow;">Fekete előtér + Sárga háttér</span></span>
        <span style="color:red;">Piros előtér</span>
        <span style="color:green;">Zöld előtér</span>
        <span style="color:yellow;">Sárga előtér</span>
        <span style="color:blue;">Kék előtér</span>
        <span style="color:magenta;">Bíbor előtér</span>
        <span style="color:cyan;">Cián előtér</span>
        <span style="color:white;">Fehér előtér</span>
        <span style="background-color:black;">Fekete háttér</span>
        <span style="background-color:red;">Piros háttér</span>
        <span style="background-color:green;">Zöld háttér</span>
        <span style="background-color:yellow;">Sárga háttér</span>
        <span style="background-color:blue;">Kék háttér</span>
        <span style="background-color:magenta;">Bíbor háttér</span>
        <span style="background-color:cyan;">Cián háttér</span>
        <span style="background-color:white;"><span style="color:black;">Fehér háttér + Fekete szöveg</span></span>
        <span style="color: rgb(255, 128, 0);">24 bites előtér (narancs)</span>
        <span style="background-color: rgb(0, 128, 255);">24 bites háttér (világoskék)</span>
        """;

    [Test]
    public void EnsureThat_RenderAnsiCodeWorks()
    {
        string rendered = TerminalRenderer.RenderAnsiCode(Input);
        Assert.That(rendered, Is.EqualTo(ExpectedOutput));
    }
}
