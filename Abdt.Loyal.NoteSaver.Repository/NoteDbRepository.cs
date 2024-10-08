﻿using Abdt.Loyal.NoteSaver.Domain;
using Abdt.Loyal.NoteSaver.Domain.Exceptions;
using Abdt.Loyal.NoteSaver.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Abdt.Loyal.NoteSaver.Repository
{
    public class NoteDbRepository : IRepository<Note>
    {
        private NoteContext _noteContext;

        public NoteDbRepository(NoteContext context)
        {
            _noteContext = context;
        }

        /// <inheritdoc />
        public async Task<Note> Add(Note item)
        {
            await _noteContext.Notes.AddAsync(item);
            await _noteContext.SaveChangesAsync();

            return item;
        }

        /// <inheritdoc />
        public async Task Delete(long id)
        {
            if (id <= 0)
                throw new BelowZeroIdentifierException(id);

            var note = await _noteContext.Notes.FindAsync(id);
            if (note is not null)
                _noteContext.Notes.Remove(note);

            await _noteContext.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<Note?> GetById(long id)
        {
            if (id <= 0)
                throw new BelowZeroIdentifierException(id);

            var note = await _noteContext.Notes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

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

            _noteContext.Update(item);
            await _noteContext.SaveChangesAsync();

            return item;
        }

        /// <summary>
        /// Подсчитывает количество элементов.
        /// </summary>
        /// <returns>Количество элементов</returns>
        private uint GetAllItemsCount() => Convert.ToUInt32(_noteContext.Notes.Count());

        /// <summary>
        /// Высчитывает параметры страницы.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="itemsCount"></param>
        /// <returns>Кортеж параметров страницы</returns>
        private (ushort PageNumber, int ItemsToSkip) GetPageParameters(ushort pageNumber, int itemsCount)
        {
            var itemsToSkip = (pageNumber - 1) * itemsCount;

            if (_noteContext.Notes.Count() - itemsToSkip > 0)
                return (pageNumber, itemsToSkip);

            ushort extraPages = (ushort)(Math.Abs(_noteContext.Notes.Count() - itemsToSkip) / itemsCount + 1);
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
                .ToArrayAsync();

            return items;
        }
    }
}
