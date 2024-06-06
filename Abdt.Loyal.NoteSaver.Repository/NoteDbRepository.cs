using Abdt.Loyal.NoteSaver.Domain;
using Abdt.Loyal.NoteSaver.Domain.Exceptions;
using Abdt.Loyal.NoteSaver.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Abdt.Loyal.NoteSaver.Repository
{
    public class NoteDbRepository : IRepository<Note>
    {
        private readonly Expression<Func<Note, bool>> _notDeleted = x => !x.IsDeleted;
        private NoteContext _noteContext;

        public NoteDbRepository(NoteContext context)
        {
            _noteContext = context;
        }

        /// <inheritdoc />
        public async Task<long> Add(Note item)
        {
            var currentDate = DateTimeOffset.Now;
            item.CreatedAt = currentDate;
            item.UpdatedAt = currentDate;
            item.IsDeleted = false;

            await _noteContext.Notes.AddAsync(item);
            await _noteContext.SaveChangesAsync();

            return item.Id;
        }

        /// <inheritdoc />
        public async Task Delete(long id)
        {
            if (id <= 0)
                throw new BelowZeroIdentifierException(id);

            _noteContext.Notes.Remove(new Note { Id = id });

            await _noteContext.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<Note?> GetById(long id)
        {
            if (id <= 0)
                throw new BelowZeroIdentifierException(id);

            var note = await _noteContext.Notes.AsNoTracking().Where(_notDeleted).FirstOrDefaultAsync(x => x.Id == id);

            return note;
        }

        /// <inheritdoc />
        public async Task<Page<Note>> GetPage(ushort pageNumber, int itemsCount)
        {
            if (itemsCount > 100)
                itemsCount = 100;

            var pageParams = GetPageParameters(pageNumber, itemsCount);

            var page = new Page<Note>()
            {
                Items = await GetItems(pageParams.ItemsToSkip, itemsCount),
                AllItemsCount = GetAllItemsCount(),
                CurrentPageNumber = pageParams.PageNumber
            };

            return page;
        }

        /// <inheritdoc />
        public async Task<Note?> Update(Note item)
        {
            ArgumentNullException.ThrowIfNull(item, nameof(item));

            item.UpdatedAt = DateTimeOffset.Now;

            _noteContext.Update(item);
            await _noteContext.SaveChangesAsync();

            return item;
        }

        /// <summary>
        /// Подсчитывает количество элементов.
        /// </summary>
        /// <returns>Количество элементов</returns>
        private uint GetAllItemsCount() => Convert.ToUInt32(_noteContext.Notes.Count(_notDeleted));

        /// <summary>
        /// Высчитывает параметры страницы.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="itemsCount"></param>
        /// <returns>Кортеж параметров страницы</returns>
        private (ushort PageNumber, int ItemsToSkip) GetPageParameters(ushort pageNumber, int itemsCount)
        {
            var itemsToSkip = (pageNumber - 1) * itemsCount;

            if (_noteContext.Notes.Count(_notDeleted) - itemsToSkip > 0)
                return (pageNumber, itemsToSkip);

            ushort extraPages = (ushort)(Math.Abs(_noteContext.Notes.Count(_notDeleted) - itemsToSkip) / itemsCount + 1);
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
        private async Task<ICollection<Note>> GetItems(int itemsToSkip, int itemsCount)
        {
            var items = await _noteContext.Notes
                .AsNoTracking()
                .Skip(itemsToSkip)
                .Take(itemsCount)
                .Where(_notDeleted)
                .ToArrayAsync();

            return items;
        }
    }
}
