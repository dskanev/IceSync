using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IceSync.Infrastructure.Authentication
{
    public record AuthenticateInputModel
    (
        string ApiCompanyId,
        string ApiUserId,
        string ApiUserSecret
    );
}