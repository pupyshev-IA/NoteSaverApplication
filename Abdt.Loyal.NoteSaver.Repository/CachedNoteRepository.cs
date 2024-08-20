using Abdt.Loyal.NoteSaver.Domain;
using Abdt.Loyal.NoteSaver.Repository.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace Abdt.Loyal.NoteSaver.Repository
{
    public class CachedNoteRepository : IRepository<Note>
    {
        private readonly NoteDbRepository _decorated;
        private readonly IMemoryCache _cache;

        public CachedNoteRepository(NoteDbRepository decorated, IMemoryCache cache)
        {
            _decorated = decorated;
            _cache = cache;
        }

        public Task<long> Add(Note item)
        {
            return _decorated.Add(item);
        }

        public Task Delete(long id)
        {
            return _decorated.Delete(id);
        }

        public Task<Note?> GetById(long id)
        {
            string key = $"note-{id}";
            return _cache.GetOrCreateAsync(
                key,
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

                    return _decorated.GetById(id);
                });
        }

        public Task<Page<Note>> GetPage(ushort pageNumber, int itemsCount)
        {
            string key = $"page-num:{pageNumber}&itemsCount:{itemsCount}";
            return _cache.GetOrCreateAsync(
                key,
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));

                    return _decorated.GetPage(pageNumber, itemsCount);
                });
        }

        public Task<Note?> Update(Note item)
        {
            return _decorated.Update(item);
        }
    }
}
