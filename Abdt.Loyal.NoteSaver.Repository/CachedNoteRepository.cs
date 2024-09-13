using Abdt.Loyal.NoteSaver.Domain;
using Abdt.Loyal.NoteSaver.Repository.Abstractions;

namespace Abdt.Loyal.NoteSaver.Repository
{
    public class CachedNoteRepository : IRepository<Note>
    {
        private readonly NoteDbRepository _decorated;
        private readonly ICache _cache;

        public CachedNoteRepository(NoteDbRepository decorated, ICache cache)
        {
            _decorated = decorated;
            _cache = cache;
        }

        public async Task<Note> Add(Note item)
        {
            var addedNote = await _decorated.Add(item);
            await Task.Run(() => _cache.Set(item.Id.ToString(), item));
            
            return addedNote;
        }

        public async Task Delete(long id)
        {
            await _decorated.Delete(id);
            await Task.Run(() => _cache.Invalidate<Note>(id.ToString()));
        }

        public async Task<Note?> GetById(long id, long userId)
        {
            var cachedNote = await Task.Run(() => _cache.Get<Note>(id.ToString()));
            if (cachedNote is not null)
                return cachedNote;

            var note = await _decorated.GetById(id, userId);
            await Task.Run(() => _cache.Set(id.ToString(), note));
            return note;
        }

        public async Task<Page<Note>> GetPage(ushort pageNumber, int itemsCount, long userId)
        {
            return await _decorated.GetPage(pageNumber, itemsCount, userId);
        }

        public async Task<Note?> Update(Note item)
        {
            var updatedNote = await _decorated.Update(item);
            await Task.Run(() => _cache.Set(item.Id.ToString(), updatedNote));

            return updatedNote;
        }
    }
}
