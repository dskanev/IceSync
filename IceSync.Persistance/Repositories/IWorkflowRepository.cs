using IceSync.Persistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceSync.Persistance.Repositories
{
    public interface IWorkflowRepository
    {
        Task<IList<Workflow>> GetAll();
        Task<IList<int>> GetAllIds();
        Task InsertMany(IList<Workflow> workflows);
        Task DeleteManyById(IList<int> workflowIds);
    }
}
