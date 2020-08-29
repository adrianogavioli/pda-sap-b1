using Frattina.Business.Interfaces;
using Frattina.Business.Notificacoes;

namespace Frattina.Application.Services
{
    public abstract class BaseApplication
    {
        private readonly INotificador _notificador;

        protected BaseApplication(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected void Notificar(string mensagem)
        {
            _notificador.Handle(new Notificacao(mensagem));
        }

        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }
    }
}
