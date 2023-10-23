using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceSync.Infrastructure.Configuration
{
    public class UniLoaderApiConfiguration
    {
        public string Url { get; set; }
        public string CompanyId { get; set; }
        public string UserId { get; set; }
        public string UserSecret { get; set; }
    };
}
