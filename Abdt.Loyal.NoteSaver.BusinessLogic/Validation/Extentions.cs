using Abdt.Loyal.NoteSaver.Domain;
using C = Abdt.Loyal.NoteSaver.BusinessLogic.Validation.Constants;

namespace Abdt.Loyal.NoteSaver.BusinessLogic.Validation
{
    public static class Extentions
    {
        public static ValidationResult<T> Required<T>(this T value, string name, string? userMessage = null) where T : class
        {
            var result = new ValidationResult<T>(value);

            if (value is null)
                result.AddError(name, userMessage is null ? string.Format(C._defaultRequiredMessage, name) : userMessage);   

            return result;
        }

        public static ValidationResult<T> Required<T>(this ValidationResult<T> result, string name, string? userMessage = null) where T : class
        {
            if (result.Item is null)
                result.AddError(name, userMessage is null ? string.Format(C._defaultRequiredMessage, name) : userMessage);

            return result;
        }

        public static ValidationResult<string> Required(this string value, string name, string? userMessage = null)
        {
            var result = new ValidationResult<string>(value);

            if (string.IsNullOrWhiteSpace(value))
                result.AddError(name, userMessage is null ? string.Format(C._defaultRequiredMessage, name) : userMessage);

            return result;
        }

        public static ValidationResult<string> Required(this ValidationResult<string> result, string name, string? userMessage = null)
        {
            if (string.IsNullOrWhiteSpace(result.Item))
                result.AddError(name, userMessage is null ? string.Format(C._defaultRequiredMessage, name) : userMessage);

            return result;
        }

        public static ValidationResult<T> Between<T>(this T item, T start, T end, string name, string? userMessage = null) where T: IComparable<T>
        {
            var result = new ValidationResult<T>(item);

            if (item.CompareTo(start) < 0 || item.CompareTo(end) > 0)
                result.AddError(name, userMessage is null ? string.Format(C._defaultBetweenMessage, name) : userMessage);

            return result;
        }
    }
}
