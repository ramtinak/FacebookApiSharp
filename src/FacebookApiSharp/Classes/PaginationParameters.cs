/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using System.Collections.Generic;

namespace FacebookApiSharp
{
    public class PaginationParameters
    {
        private PaginationParameters()
        {
            SessionId = System.Guid.NewGuid().ToString();
        }
        public bool HasPreviousPage { get; set; }
        public string StartCursor { get; set; } = string.Empty;
        public string EndCursor { get; set; } = string.Empty;

        public string SessionId { get; set; } = string.Empty;
        public int MaximumPagesToLoad { get; set; }
        public int PagesLoaded { get; set; } = 1;

        public static PaginationParameters Empty => MaxPagesToLoad(int.MaxValue);

        public static PaginationParameters MaxPagesToLoad(int maxPagesToLoad) =>
            new PaginationParameters { MaximumPagesToLoad = maxPagesToLoad };
    }
}