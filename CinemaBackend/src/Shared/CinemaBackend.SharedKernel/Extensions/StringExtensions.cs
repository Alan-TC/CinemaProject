using System.Diagnostics.CodeAnalysis;

namespace CinemaBackend.SharedKernel.Extensions;

public static class StringExtensions
{
    public static bool IsNotEmpty([NotNullWhen(true)]this string? value) => 
        !string.IsNullOrWhiteSpace(value);
    
    public static bool IsEmpty([NotNullWhen(false)]this string? value) => 
        string.IsNullOrWhiteSpace(value);
}
