using Frattina.Business.Notificacoes;
using System.Collections.Generic;

namespace Frattina.Business.Interfaces
{
    public interface INotificador
    {
        bool TemNotificacao();

        List<Notificacao> ObterNotificacoes();

        void Handle(Notificacao notificacao);
    }
}
