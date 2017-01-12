using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Centaline.Framework.Kernel.DTO.Entity;

namespace SqlExport.Entity
{
    public class PageResult<TData>: Result<TData>
    {
        public PageInfo PageInfo { get; set; }

        public PageResult()
        {
            PageInfo=new PageInfo(0,0,0);
        }
    }
}
