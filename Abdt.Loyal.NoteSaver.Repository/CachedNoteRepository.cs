using Abdt.Loyal.NoteSaver.Domain;
using Abdt.Loyal.NoteSaver.Repository.Abstractions;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Abdt.Loyal.NoteSaver.Repository
{
    public class CachedNoteRepository : IRepository<Note>
    {
        private readonly NoteDbRepository _decorated;
        private readonly IDistributedCache _cache;

        public CachedNoteRepository(NoteDbRepository decorated, IDistributedCache cache)
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

        public async Task<Note?> GetById(long id)
        {
            string key = $"note:{id}";
            string? cachedNote = await _cache.GetStringAsync(key);
            Note? note;

            if (string.IsNullOrEmpty(cachedNote))
            {
                note = await _decorated.GetById(id);
                if (note is null)
                    return note;

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(note));

                return note;
            }

            note = JsonSerializer.Deserialize<Note>(cachedNote);
            return note;
        }

        public async Task<Page<Note>> GetPage(ushort pageNumber, int itemsCount)
        {
            string key = $"page-num:{pageNumber}&itemsCount{itemsCount}";
            string? cachedPage = await _cache.GetStringAsync(key);
            Page<Note>? page;

            if (string.IsNullOrEmpty(cachedPage))
            {
                page = await _decorated.GetPage(pageNumber, itemsCount);
                if (page is null)
                    return page;

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(page));

                return page;
            }

            page = JsonSerializer.Deserialize<Page<Note>>(cachedPage);
            return page;
        }

        public Task<Note?> Update(Note item)
        {
            return _decorated.Update(item);
        }
    }
}
