using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<AuditoriaRegistro> AuditoriaRegistros { get; set; }
        IModel Model { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        DatabaseFacade Database { get; }
        EntityEntry Add(object entity);
    }
}
