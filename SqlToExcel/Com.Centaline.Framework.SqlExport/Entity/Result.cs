using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlExport.Entity
{
    public class Result<TData>
    {
        public long TimeConsume { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public TData Data { get; set; }
        //public List<Column> Columns { get; set; }

        public Result()
        {
            Code = 0;
            Message = "ok";
        }
    }
}
