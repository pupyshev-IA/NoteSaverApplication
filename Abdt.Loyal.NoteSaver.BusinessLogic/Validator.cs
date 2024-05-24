using Abdt.Loyal.NoteSaver.BusinessLogic.Abstractions;
using Abdt.Loyal.NoteSaver.Domain;

namespace Abdt.Loyal.NoteSaver.BusinessLogic
{
    public class Validator : INoteValidator
    {
        /// <inheritdoc />        
        public bool IsValid(Note note) 
        {
            if (note.Id <= 0)
                return false;

            if (string.IsNullOrWhiteSpace(note.Title))
                return false;

            return true;
        }
    }
}
