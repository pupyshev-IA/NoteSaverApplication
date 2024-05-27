﻿using Abdt.Loyal.NoteSaver.BusinessLogic.Abstractions;
using Abdt.Loyal.NoteSaver.Domain;

namespace Abdt.Loyal.NoteSaver.BusinessLogic
{
    public class Validator : INoteValidator
    {
        /// <inheritdoc />        
        public bool IsValid(Note note) 
        {
            if (note == null)
                return false;

            return !string.IsNullOrWhiteSpace(note.Title);
        }

        /// <inheritdoc />
        public bool IsValidId(long id)
        {
            return id > 0;
        }
    }
}
