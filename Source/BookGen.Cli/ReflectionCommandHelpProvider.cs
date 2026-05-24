using System.ComponentModel;
using System.Reflection;
using System.Text;

using BookGen.Cli.Annotations;

namespace BookGen.Cli;

public sealed class ReflectionCommandHelpProvider : ICommandHelpProvider
{
    public string GetHelp(string commandName, Type argumentType)
    {
        var arguments = new Dictionary<int, string>();
        var switches = new Dictionary<string, string>();

        var sb = new StringBuilder();
        sb.AppendLine(commandName)
          .AppendLine();

        PropertyInfo[] properties = argumentType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in properties)
        {
            var switchAttribute = property.GetCustomAttribute<SwitchAttribute>();
            var argumentAttribute = property.GetCustomAttribute<ArgumentAttribute>();
            var description = property.GetCustomAttribute<DescriptionAttribute>()?.Description ?? "No description found";
            if (switchAttribute != null)
            {
                switches.Add($"-{switchAttribute.ShortName}, --{switchAttribute.LongName}", description);
            }
            else if (argumentAttribute != null)
            {
                arguments.Add(argumentAttribute.Index, description);
            }
        }

        if (arguments.Count > 0)
        {
            sb.AppendLine("Arguments:").AppendLine();
            foreach (KeyValuePair<int, string> argument in arguments.OrderBy(x => x.Key))
            {
                sb.AppendLine($"  {argument.Key} - {argument.Value}");
            }
        }

        if (switches.Count > 0)
        {
            sb.AppendLine("Switches:").AppendLine();
            foreach (KeyValuePair<string, string> @switch in switches.OrderBy(x => x.Key))
            {
                sb.AppendLine($"  {@switch.Key} - {@switch.Value}");
            }
        }

        return sb.ToString();
    }
}
