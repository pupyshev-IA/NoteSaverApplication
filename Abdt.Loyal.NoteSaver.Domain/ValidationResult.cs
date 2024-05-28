namespace Abdt.Loyal.NoteSaver.Domain
{
    public class ValidationResult<T>
    {
        public bool IsValid => Errors.Count == 0;
        public Dictionary<string, List<string>> Errors { get; set; }
        public T? Item { get; init; }

        public ValidationResult(T? item)
        {
            Errors = new Dictionary<string, List<string>>();
            Item = item;
        }

        public void AddError(string fieldName, string errorMessage)
        {
            if (Errors.ContainsKey(fieldName))
            {
                Errors[fieldName].Add(errorMessage);
            }
            else
            {
                Errors.Add(fieldName, new List<string> { errorMessage });
            }
        }

        public void AddErrors(string fieldName, IEnumerable<string> errorMessages)
        {
            if (Errors.ContainsKey(fieldName))
            {
                Errors[fieldName].AddRange(errorMessages);
                return;
            }

            var errors = new List<string>();
            errors.AddRange(errorMessages);
            Errors.Add(fieldName, errors);
        }
    }
}
