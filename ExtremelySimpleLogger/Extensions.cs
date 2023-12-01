using System;
using System.Linq;

namespace ExtremelySimpleLogger {
    /// <summary>
    /// A set of extension methods for logging-related activities, like converting <see cref="ConsoleColor"/> to ANSI color codes.
    /// </summary>
    public static class Extensions {

        private static readonly int[] AnsiCodes = new[] {
            ConsoleColor.Black, ConsoleColor.DarkRed, ConsoleColor.DarkGreen, ConsoleColor.DarkYellow,
            ConsoleColor.DarkBlue, ConsoleColor.DarkMagenta, ConsoleColor.DarkCyan, ConsoleColor.Gray,
            ConsoleColor.DarkGray, ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Yellow,
            ConsoleColor.Blue, ConsoleColor.Magenta, ConsoleColor.Cyan, ConsoleColor.White
        }.Select((s, i) => (int) s).ToArray();

        /// <summary>
        /// Converts the given <see cref="ConsoleColor"/> to its ANSI escape sequence representation. If the supplied <paramref name="color"/> is <see langword="null"/>, the reset escape sequence for the given color type will be returned.
        /// </summary>
        /// <param name="color">The color. If <see langword="null"/>, the reset escape sequence for the given color type will be returned.</param>
        /// <param name="background">Whether to return a background color. If this is <see langword="false"/>, a foreground color is returned instead.</param>
        /// <returns>The ANSI escape sequence representation of the given <paramref name="color"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the <paramref name="color"/> is not in defined range.</exception>
        public static string ToAnsiCode(this ConsoleColor? color, bool background = false) {
            if (color.HasValue) {
                if (color < 0 || (int) color >= Extensions.AnsiCodes.Length)
                    throw new ArgumentOutOfRangeException(nameof(color), color, null);
                return $"\x1B[{(background ? 48 : 38)};5;{Extensions.AnsiCodes[(int) color]}m";
            }
            return $"\x1B[{(background ? 49 : 39)}m";
        }

        /// <summary>
        /// Wraps the given string in the ANSI escape sequence representation of the given <paramref name="color"/> and the appropriate reset escape sequence using <see cref="ToAnsiCode"/>.
        /// </summary>
        /// <param name="s">The string to wrap.</param>
        /// <param name="color">The color.</param>
        /// <param name="background">Whether to use <paramref name="color"/> as a background color. If this is <see langword="false"/>, a foreground color is used instead.</param>
        /// <returns>The given string, wrapped in ANSI color codes.</returns>
        public static string WrapAnsiCode(this string s, ConsoleColor color, bool background = false) {
            return Extensions.ToAnsiCode(color, background) + s + Extensions.ToAnsiCode(null, background);
        }

    }
}
