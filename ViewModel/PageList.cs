using System;
using System.Collections.Generic;
using System.Linq;

namespace ViewModels
{
    public class PageList<T>
    {
        public IEnumerable<T> DataSource { get; private set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; private set; }
        public bool HasPrev { get { return PageIndex > 1; } }
        public bool HasNext { get { return PageIndex < PageCount; } }
        public PageList(int pageSize, IEnumerable<T> dataSource)
        {
            this.PageSize = pageSize > 1 ? pageSize : 1;
            this.DataSource = dataSource;
            PageCount = (int)Math.Ceiling(dataSource.Count() / (double)pageSize);
            PageCount = (PageCount == 0) ? 1 : PageCount;
        }
        //获取当前页数据
        public IEnumerable<T> GetPagingData()
        {
            return DataSource.Skip((PageIndex - 1) * PageSize).Take(PageSize);
        }
    }
}
