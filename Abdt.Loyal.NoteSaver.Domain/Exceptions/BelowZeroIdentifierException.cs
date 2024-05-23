namespace Abdt.Loyal.NoteSaver.Domain.Exceptions
{
    public class BelowZeroIdentifierException : ApplicationException
    {
        private const string DefaultExceptionMessage = $"Identifier below zero is not acceptable. See {nameof(Identifier)} property.";
        public long Identifier { get; }

        public BelowZeroIdentifierException(long identifier)
            : base(DefaultExceptionMessage)
        {
            Identifier = identifier;
        }

        public BelowZeroIdentifierException(long identifier, string message) : base(message)
        {
            Identifier = identifier;
        }
    }
}
