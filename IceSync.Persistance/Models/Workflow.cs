using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceSync.Persistance.Models
{
    public class Workflow
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsRunning { get; set; }
        public string? MultiExecBehavior { get; set; }
    }
}
