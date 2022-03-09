namespace Geev.Core.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Abp: Converts PascalCase string to camelCase string.
    /// </summary>
    /// <param name="str">String to convert</param>
    /// <param name="invariantCulture">Invariant culture</param>
    /// <returns>camelCase of the string</returns>
    public static string ToCamelCase(this string str, bool invariantCulture = true)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return str;
        }

        if (str.Length == 1)
        {
            return invariantCulture ? str.ToLowerInvariant() : str.ToLower();
        }

        return (invariantCulture ? char.ToLowerInvariant(str[0]) : char.ToLower(str[0])) + str.Substring(1);
    }

}
