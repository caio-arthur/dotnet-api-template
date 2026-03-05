using Microsoft.EntityFrameworkCore;

namespace Application.Common.Models
{
    public class ListaPaginada<T>
    {
        public List<T> Itens { get; }
        public int PaginaAtual { get; }
        public int TotalPaginas { get; }
        public int TotalRegistros { get; }

        public ListaPaginada(List<T> itens, int totalRegistros, int paginaAtual, int tamanhoPagina)
        {
            PaginaAtual = paginaAtual;
            TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)tamanhoPagina);
            TotalRegistros = totalRegistros;
            Itens = itens;
        }

        public bool TemPaginaAnterior => PaginaAtual > 1;

        public bool TemProximaPagina => PaginaAtual < TotalPaginas;

        public static async Task<ListaPaginada<T>> CreateAsync(IQueryable<T> source, int paginaAtual, int tamanhoPagina)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((paginaAtual - 1) * tamanhoPagina).Take(tamanhoPagina).ToListAsync();

            return new ListaPaginada<T>(items, count, paginaAtual, tamanhoPagina);
        }
    }
}
