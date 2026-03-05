using Domain.Common.Interfaces;

namespace Domain.Common.Primitives
{
    public abstract class EntidadeComEventos
    {
        private readonly List<IEventoDeDominio> _eventosDeDominio = [];

        public IReadOnlyCollection<IEventoDeDominio> EventosDeDominio => _eventosDeDominio.AsReadOnly();

        public void AdicionarEventoDeDominio(IEventoDeDominio evento)
        {
            _eventosDeDominio.Add(evento);
        }

        public void LimparEventosDeDominio()
        {
            _eventosDeDominio.Clear();
        }
    }
}
