namespace Abdt.Loyal.NoteSaver.Domain.Exceptions
{
    public class ApplicationException : Exception
    {
        public ApplicationException() : base() { }

        public ApplicationException(string message) : base(message) { }

        public ApplicationException(string message, Exception innerExeption)
            : base(message, innerExeption) { }
    }
}
