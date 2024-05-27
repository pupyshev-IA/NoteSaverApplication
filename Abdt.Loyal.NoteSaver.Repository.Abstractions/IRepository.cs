﻿namespace Abdt.Loyal.NoteSaver.Repository.Abstractions
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Добавляет в хранилище новый элемент.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Возвращает добавленный элемент с проинициализированными служебными полями</returns>
        Task<long> Add(T item);

        /// <summary>
        /// Обновляет элемент в хранилище.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Возвращает обновленный элемент или null если элемент в хранилище не найден</returns>
        Task<T?> Update(T item);

        /// <summary>
        /// Удаляет заданный элемент из хранилища. Если элемент не найден, то ошибки не будет.
        /// </summary>
        /// <param name="id"></param>
        Task Delete(long id);

        /// <summary>
        /// Извлекает коллекцию элементов из хранилища.
        /// </summary>
        /// <returns>Возвращает коллекцию элементов</returns>
        Task<ICollection<T>> GetAllItems();

        /// <summary>
        /// Находит элемент по идентификатору в хранилище.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Возвращает найденный элемент или null если элемент не найден</returns>
        Task<T?> GetById(long id);
    }
}
