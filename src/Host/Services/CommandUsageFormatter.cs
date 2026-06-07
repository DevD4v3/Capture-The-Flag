namespace CTF.Host.Services;

public class CommandUsageFormatter : ICommandTextFormatter
{
    public string FormatCommandUsage(
        string commandName,
        string group,
        CommandParameterInfo[] parameters,
        bool includeSlash = true)
    {
        var prefix = includeSlash ? "/" : "";
        var groupPrefix = string.IsNullOrEmpty(group)
            ? ""
            : $"{group} ";

        var parameterText = string.Join(
            " ",
            parameters.Select(parameterInfo => $"[{parameterInfo.Name}]")
        );

        return $"{prefix}{groupPrefix}{commandName} {parameterText}".Trim();
    }
}
