using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceSync.Infrastructure.Authentication
{
    public interface IUniLoaderAuthApi
    {
        [Post("/authenticate")]
        Task<string> Authenticate([Body] AuthenticateInputModel input);
    }
}
