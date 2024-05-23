using Abdt.Loyal.NoteSaver.Domain;
using Abdt.Loyal.NoteSaver.Domain.Exceptions;
using Abdt.Loyal.NoteSaver.Repository.Abstractions;

namespace Abdt.Loyal.NoteSaver.Repository
{
    public class NoteRepository : IRepository<Note>
    {
        private static Dictionary<long, Note> _storage = new Dictionary<long, Note>();
        private static long _currentId = 0;

        /// <inheritdoc />
        public Note Add(Note item)
        {
            var currentDate = DateTimeOffset.Now;
            item.CreatedAt = currentDate;
            item.UpdatedAt = currentDate;

            _currentId++;

            item.Id = _currentId;

            _storage.Add(item.Id, item);

            return item;
        }

        /// <inheritdoc />
        public void Delete(long id)
        {
            if (id <= 0)
                throw new BelowZeroIdentifierException(id);

            _storage.Remove(id);
        }

        /// <inheritdoc />
        public ICollection<Note> GetAllItems()
        {
            return _storage.Values;
        }

        /// <inheritdoc />
        public Note? GetById(long id)
        {
            if (id <= 0)
                throw new BelowZeroIdentifierException(id);

            if (_storage.TryGetValue(id, out var item))
                return item;

            return null;
        }

        /// <inheritdoc />
        public Note? Update(Note item)
        {
            ArgumentNullException.ThrowIfNull(item, nameof(item));

            var updatedNote = GetById(item.Id);

            if (updatedNote == null)
                return null;

            updatedNote.Title = item.Title;
            updatedNote.Content = item.Content;
            updatedNote.UpdatedAt = DateTimeOffset.Now;

            return updatedNote;
        }
    }
}