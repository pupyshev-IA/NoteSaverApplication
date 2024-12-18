﻿namespace Abdt.Loyal.NoteSaver.Web.Shared
{
    public class Page<T>
    {
        public required ICollection<T> Items { get; set; } = Array.Empty<T>();
        public uint AllItemsCount { get; set; }
        public ushort CurrentPageNumber { get; set; }
    }
}
