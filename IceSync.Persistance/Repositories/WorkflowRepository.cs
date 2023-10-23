using IceSync.Persistance.Database;
using IceSync.Persistance.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceSync.Persistance.Repositories
{
    public class WorkflowRepository : IWorkflowRepository
    {
        private readonly IceSyncDbContext _dbContext;

        public WorkflowRepository(IceSyncDbContext dbContext)
        {
            _dbContext = dbContext;
        }        

        public async Task<IList<Workflow>> GetAll()
        {
            return await _dbContext
                .Workflows
                .ToListAsync();
        }

        public async Task<IList<int>> GetAllIds()
        {
            return await _dbContext
                .Workflows
                .Select(x => x.Id)
                .ToListAsync();
        }

        public async Task InsertMany(IList<Workflow> workflows)
        {
            await _dbContext.AddRangeAsync(workflows);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteManyById(IList<int> workflowIds)
        {
            var workflowsToDelete = await _dbContext
                .Workflows
                .Where(x => workflowIds.Contains(x.Id))
                .ToListAsync();

            _dbContext
                .RemoveRange(workflowsToDelete);

            await _dbContext
                .SaveChangesAsync();
        }
    }
}
