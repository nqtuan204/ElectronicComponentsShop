using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicComponentsShop.Models
{
    public class PaginatorVM
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public int Total { get; set; }
        public int LastPage { get; set; }
        public string Path { get; set; }
        public int Displays { get; set; } = 4;

        public PaginatorVM(int page, int pageSize, int total, string path)
        {
            Page = page;
            PageSize = pageSize;
            Total = total;
            From = pageSize * (page - 1) + 1;
            To = From + PageSize < Total ? (page*pageSize) : Total;
            LastPage = (int)Math.Ceiling((double)total / pageSize);
            Path = path;
        }
    }
}
