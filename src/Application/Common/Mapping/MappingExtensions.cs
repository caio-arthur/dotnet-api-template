using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gridify;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Mapping
{
    public static class MappingExtensions
    {
        public static Task<ListaPaginada<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize)
            => ListaPaginada<TDestination>.CreateAsync(queryable, pageNumber, pageSize);

        public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration)
            => queryable.ProjectTo<TDestination>(configuration).ToListAsync();

        public static async Task<ListaPaginada<TDestination>> ProjectToListaPaginadaAsync<TSource, TDestination>(
                    this QueryablePaging<TSource> gridifyQueryable,
                    IConfigurationProvider configuration,
                    int pageNumber,
                    int pageSize,
                    CancellationToken cancellationToken = default)
                    where TSource : class 
        {
            var items = await gridifyQueryable.Query
                .AsNoTracking() 
                .ProjectTo<TDestination>(configuration)
                .ToListAsync(cancellationToken);

            return new ListaPaginada<TDestination>(items, gridifyQueryable.Count, pageNumber, pageSize);
        }
    }
}
