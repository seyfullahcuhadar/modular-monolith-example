using System.Reflection;

namespace Catalog.Presentation;

internal static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
