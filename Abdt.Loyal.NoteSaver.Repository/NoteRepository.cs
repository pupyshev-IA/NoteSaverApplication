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
        public async Task<long> Add(Note item)
        {
            return await Task.Run(() =>
            {
                var currentDate = DateTimeOffset.Now;
                item.CreatedAt = currentDate;
                item.UpdatedAt = currentDate;

                _currentId++;

                item.Id = _currentId;

                _storage.Add(item.Id, item);

                return item.Id;
            });
        }

        /// <inheritdoc />
        public async Task Delete(long id)
        {
            if (id <= 0)
                throw new BelowZeroIdentifierException(id);

            await Task.Run(() => _storage.Remove(id));
        }

        /// <inheritdoc />
        public async Task<ICollection<Note>> GetAllItems()
        {
            return await Task.Run(() =>
            {
                return _storage.Values;
            });
        }

        /// <inheritdoc />
        public async Task<Note?> GetById(long id)
        {
            if (id <= 0)
                throw new BelowZeroIdentifierException(id);

            return await Task.Run(() =>
            {
                if (_storage.TryGetValue(id, out var item))
                    return item;

                return null;
            });
        }

        /// <inheritdoc />
        public async Task<Note?> Update(Note item)
        {
            ArgumentNullException.ThrowIfNull(item, nameof(item));

            return await Task.Run(() =>
            {
                var updatedNote = GetById(item.Id).Result;

                if (updatedNote == null)
                    return null;

                updatedNote.Title = item.Title;
                updatedNote.Content = item.Content;
                updatedNote.UpdatedAt = DateTimeOffset.Now;

                return updatedNote;
            });
        }
    }
}
