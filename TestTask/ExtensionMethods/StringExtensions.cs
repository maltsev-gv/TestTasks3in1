using System.Collections.Generic;

namespace TestTask.ExtensionMethods
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string inStr)
        {
            return string.IsNullOrEmpty(inStr);
        }

        public static bool IsNullOrWhiteSpace(this string inStr)
        {
            return string.IsNullOrWhiteSpace(inStr);
        }

        public static string JoinedString<T>(this IEnumerable<T> enumerable, string separator = ",")
        {
            return string.Join(separator, enumerable);
        }
        
        public static string Formatted(this string inStr, params object[] args)
        {
            return string.Format(inStr, args);
        }

        /// <summary>
        /// Возвращает слова в падеже, зависимом от заданного числа 
        /// </summary>
        /// <param name="number">Число от которого зависит выбранное слово</param>
        /// <param name="nominative">Именительный падеж слова. Например "день"</param>
        /// <param name="genitive">Родительный падеж слова. Например "дня"</param>
        /// <param name="plural">Множественное число слова. Например "дней"</param>
        /// <param name="includeNumber">включать ли в результирующую строку само число</param>
        /// <returns></returns>
        public static string GetDeclension(this int number, string nominative, string genitive, string plural, bool includeNumber = true)
        {
            number %= 100;
            if (number >= 11 && number <= 19)
            {
                return includeNumber ? $"{number} {plural}" : $"{plural}";
            }

            string result;
            var i = number % 10;
            switch (i)
            {
                case 1:
                    result = nominative;
                    break;
                case 2:
                case 3:
                case 4:
                    result = genitive;
                    break;
                default:
                    result = plural;
                    break;
            }
            return includeNumber ? $"{number} {result}" : $"{result}";
        }
    }
}
