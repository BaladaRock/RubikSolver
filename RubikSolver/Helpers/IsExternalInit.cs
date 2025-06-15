namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// WE need this for C# 9.0+ projects that target frameworks
    /// (netstandard2.0, net48 etc.) for init-only properties / records.
    /// </summary>
    internal static class IsExternalInit { }
}
