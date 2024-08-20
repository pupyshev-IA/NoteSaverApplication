using Abdt.Loyal.NoteSaver.Domain;
using Abdt.Loyal.NoteSaver.Domain.Exceptions;
using Abdt.Loyal.NoteSaver.Repository.Abstractions;
using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace Abdt.Loyal.NoteSaver.Repository
{
    public class NoteInMemoryRepository //: IRepository<Note>
    {
        private static ConcurrentDictionary<long, Note> _storage = new ConcurrentDictionary<long, Note>();
        private static long _currentId = 0;

        /// <inheritdoc />
        public Task<long> Add(Note item)
        {
            return Task.Run(() =>
            {
                var currentDate = DateTimeOffset.Now;
                item.CreatedAt = currentDate;
                item.UpdatedAt = currentDate;

                _currentId++;

                item.Id = _currentId;

                _storage.TryAdd(item.Id, item);

                return item.Id;
            });
        }

        /// <inheritdoc />
        public Task Delete(long id)
        {
            if (id <= 0)
                throw new BelowZeroIdentifierException(id);

            return Task.Run(() => _storage.Remove(id, out var note));
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
        public Task<Page<Note>> GetPage(ushort pageNumber, int itemsCount)
        {
            if (itemsCount > 100)
                itemsCount = 100;

            var pageParams = GetPageParameters(pageNumber, itemsCount);

            var page = new Page<Note>()
            {
                Items = GetItems(pageParams.ItemsToSkip, itemsCount),
                AllItemsCount = GetAllItemsCount(),
                CurrentPageNumber = pageParams.PageNumber
            };

            return Task.FromResult(page);
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

        /// <summary>
        /// Подсчитывает количество элементов.
        /// </summary>
        /// <returns>Количество элементов</returns>
        private uint GetAllItemsCount() => Convert.ToUInt32(_storage.Count);

        /// <summary>
        /// Высчитывает параметры страницы.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="itemsCount"></param>
        /// <returns>Кортеж параметров страницы</returns>
        private (ushort PageNumber, int ItemsToSkip) GetPageParameters(ushort pageNumber, int itemsCount)
        {
            var itemsToSkip = (pageNumber - 1) * itemsCount;

            if (_storage.Count - itemsToSkip > 0)
                return (pageNumber, itemsToSkip);

            ushort extraPages = (ushort)(Math.Abs(_storage.Count - itemsToSkip) / itemsCount + 1);
            ushort actualPage = (ushort)(pageNumber - extraPages);

            var newItemsToSkip = (actualPage - 1) * itemsCount;

            return (actualPage, newItemsToSkip);
        }

        /// <summary>
        /// Извлекает коллекцию элементов из хранилища.
        /// </summary>
        /// <param name="itemsToSkip"></param>
        /// <param name="itemsCount"></param>
        /// <returns>Возвращает коллекцию элементов</returns>
        private ICollection<Note> GetItems(int itemsToSkip, int itemsCount)
        {
            var items = _storage
                .Select(x => x.Value)
                .Skip(itemsToSkip)
                .Take(itemsCount)
                .ToImmutableArray();

            return items;
        }
    }
}
