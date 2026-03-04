using AgendaPro.Domain.Interfaces;
using AgendaPro.Infrastucture.Data.Context;

namespace AgendaPro.Infrastucture.Data.Repositories
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly AgendaProDbContext _context;

        public UnitOfWork(AgendaProDbContext context)
        {
            _context = context;
        }
        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
               => await _context.SaveChangesAsync(cancellationToken);
    }
}
