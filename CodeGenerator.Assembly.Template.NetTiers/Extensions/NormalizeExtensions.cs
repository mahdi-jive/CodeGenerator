using System.Globalization;
using System.Text.RegularExpressions;

namespace CodeGenerator.Assembly.Template.NetTiers.Extensions
{
    public static class NormalizeExtensions
    {
        public static string PascalCaseCustom(this string name)
        {
            string pascalName = string.Empty;
            string notStartingAlpha = Regex.Replace(name, "^[^a-zA-Z]+", string.Empty);
            string workingString = ToLowerExceptCamelCase(notStartingAlpha);
            pascalName = RemoveSeparatorAndCapNext(workingString);

            return pascalName;
        }
        /// <summary>
        /// Converts to lower except camel case.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private static string ToLowerExceptCamelCase(string input)
        {
            char[] chars = input.ToCharArray();
            char[] origChars = input.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                int left = (i > 0 ? i - 1 : i);
                int right = (i < chars.Length - 1 ? i + 1 : i);

                if (i != left &&
                        i != right)
                {
                    if (Char.IsUpper(chars[i]) &&
                            Char.IsLetter(chars[left]) &&
                            Char.IsUpper(chars[left]))
                    {
                        chars[i] = Char.ToLower(chars[i], CultureInfo.InvariantCulture);
                    }
                    else if (Char.IsUpper(chars[i]) &&
                                Char.IsLetter(chars[right]) &&
                                Char.IsUpper(chars[right]) &&
                                Char.IsUpper(origChars[left]))
                    {
                        chars[i] = Char.ToLower(chars[i], CultureInfo.InvariantCulture);
                    }
                    else if (Char.IsUpper(chars[i]) &&
                                !Char.IsLetter(chars[right]))
                    {
                        chars[i] = Char.ToLower(chars[i], CultureInfo.InvariantCulture);
                    }
                }

                string x = new string(chars);
            }

            if (chars.Length > 0)
            {
                chars[chars.Length - 1] = Char.ToLower(chars[chars.Length - 1], CultureInfo.InvariantCulture);
            }

            return new string(chars);
        }

        /// <summary>
        /// Removes the separator and capitalises next character.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private static string RemoveSeparatorAndCapNext(string input)
        {
            char[] splitter = new char[] { '-', '_', ' ' }; // potential chars to split on
            string workingString = input.TrimEnd(splitter);
            char[] chars = workingString.ToCharArray();

            if (chars.Length > 0)
            {
                int under = workingString.IndexOfAny(splitter);
                while (under > -1)
                {
                    chars[under + 1] = Char.ToUpper(chars[under + 1], CultureInfo.InvariantCulture);
                    workingString = new String(chars);
                    under = workingString.IndexOfAny(splitter, under + 1);
                }

                chars[0] = Char.ToUpper(chars[0], CultureInfo.InvariantCulture);

                workingString = new string(chars);
            }
            string regexReplacer = "[" + new string(new char[] { '-', '_', ' ' }) + "]";

            return Regex.Replace(workingString, regexReplacer, string.Empty);
        }
        public static string GetCamelCaseName(this string name)
        {
            if (name == null)
                return string.Empty;
            // first get the PascalCase version of the name
            string pascalName = PascalCaseCustom(name);
            // now lowercase the first character to transform it to camelCase
            return pascalName.Substring(0, 1).ToLower() + pascalName.Substring(1);
        }

    }
}
