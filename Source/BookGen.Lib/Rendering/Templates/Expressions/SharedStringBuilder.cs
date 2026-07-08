using System.Text;

namespace Bookgen.Lib.Rendering.Templates.Expressions;

internal sealed class SharedStringBuilder(int capacity)
{
    private readonly StringBuilder _sb = new(capacity);

    public SharedStringBuilder Append(char value)
    {
        _sb.Append(value);
        return this;
    }

    public override string ToString()
    {
        string value = _sb.ToString();
        _sb.Clear();
        return value;
    }
}
