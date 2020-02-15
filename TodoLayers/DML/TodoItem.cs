using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoLayers.DML
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int IsCompleted { get; set; }
    }
}
