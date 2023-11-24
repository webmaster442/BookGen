namespace BookGen.Interfaces;

/// <summary>
/// https://developer.mozilla.org/en-US/docs/Web/API/Console_API
/// </summary>
public interface IJavascriptConsole
{
    public static abstract void Clear();

    public static abstract void Assert(bool assertation, object obj1);
    public static abstract void Assert(bool assertation, object obj1, object obj2);
    public static abstract void Assert(bool assertation, params object[] objects);
    public static abstract void Assert(bool assertation, string msg);
    public static abstract void Assert(bool assertation, string msg, params object[] substitutions);

    public static abstract void CountReset();
    public static abstract void CountReset(string label);

    public static abstract void Count();
    public static abstract void Count(string label);

    public static abstract void Debug(params object[] objects);
    public static abstract void Debug(string msg, params object[] substitutions);

    public static abstract void Dir(object o);
    public static abstract void DirXml(object o);

    public static abstract void Error(params object[] objects);
    public static abstract void Error(string msg);
    public static abstract void Error(string msg, params object[] objects);

    public static abstract void Exception(params object[] objects);
    public static abstract void Exception(string msg);
    public static abstract void Exception(string msg, params object[] objects);

    public static abstract void Info(params object[] objects);
    public static abstract void Info(string msg);
    public static abstract void Info(string msg, params object[] objects);

    public static abstract void Log(params object[] objects);
    public static abstract void Log(string msg);
    public static abstract void Log(string msg, params object[] objects);

    public static abstract void Warn(params object[] objects);
    public static abstract void Warn(string msg);
    public static abstract void Warn(string msg, params object[] objects);
}
