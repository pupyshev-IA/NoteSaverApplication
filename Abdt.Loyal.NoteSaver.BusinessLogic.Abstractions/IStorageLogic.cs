﻿using Abdt.Loyal.NoteSaver.Domain;

namespace Abdt.Loyal.NoteSaver.BusinessLogic.Abstractions
{
    public interface IStorageLogic<T>
    {
        Task<Page<T>> GetPage(ushort pageNumber, int itemsCount, long userId);
        Task<T> Add(T note);
        Task<T?> Get(long id, long userId);
        Task<T?> Update(T note);
        Task Delete(long id);
        //Task RestoreDeleted(long id);
    }
}
