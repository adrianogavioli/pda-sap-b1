using Frattina.Business.Interfaces;
using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Interfaces.Services;
using Frattina.Business.Models;
using Frattina.Business.Models.Validations;
using System.Linq;
using System.Threading.Tasks;

namespace Frattina.Business.Services
{
    public class AtendimentoProdutoService : BaseService, IAtendimentoProdutoService
    {
        private readonly IAtendimentoProdutoRepository _atendimentoProdutoRepository;

        public AtendimentoProdutoService(IAtendimentoProdutoRepository atendimentoProdutoRepository,
            INotificador notificador) : base(notificador)
        {
            _atendimentoProdutoRepository = atendimentoProdutoRepository;
        }

        public async Task Adicionar(AtendimentoProduto atendimentoProduto)
        {
            atendimentoProduto.RemovidoAtendimento = false;
            atendimentoProduto.RemovidoVenda = true;
            atendimentoProduto.Atendimento = null;

            if (!ExecutarValidacao(new AtendimentoProdutoValidation(), atendimentoProduto)) return;

            if (_atendimentoProdutoRepository.ObterAtendimentoProdutosPorAtendimento(atendimentoProduto.AtendimentoId).Result.Any(p => p.ProdutoSapId == atendimentoProduto.ProdutoSapId && !p.RemovidoAtendimento))
            {
                Notificar("Este produto já foi adicionado ao atendimento.");
                return;
            }

            await _atendimentoProdutoRepository.Adicionar(atendimentoProduto);
        }

        public async Task AtualizarValorNegociado(AtendimentoProduto atendimentoProduto)
        {
            var atendimentoProdutoDb = await _atendimentoProdutoRepository.ObterPorId(atendimentoProduto.Id);

            if (atendimentoProdutoDb == null)
            {
                Notificar("Não foi possível obter as informações do produto.");
                return;
            }

            atendimentoProdutoDb.ValorNegociado = atendimentoProduto.ValorNegociado;

            await _atendimentoProdutoRepository.Atualizar(atendimentoProdutoDb);
        }

        public async Task AtualizarNivelInteresse(AtendimentoProduto atendimentoProduto)
        {
            var atendimentoProdutoDb = await _atendimentoProdutoRepository.ObterPorId(atendimentoProduto.Id);

            if (atendimentoProdutoDb == null)
            {
                Notificar("Não foi possível obter as informações do produto.");
                return;
            }

            atendimentoProdutoDb.NivelInteresse = atendimentoProduto.NivelInteresse;

            await _atendimentoProdutoRepository.Atualizar(atendimentoProdutoDb);
        }

        public async Task RemoverDoAtendimento(AtendimentoProduto atendimentoProduto)
        {
            var atendimentoProdutoDb = await _atendimentoProdutoRepository.ObterPorId(atendimentoProduto.Id);

            if (atendimentoProdutoDb == null)
            {
                Notificar("Não foi possível obter as informações do produto.");
                return;
            }

            atendimentoProdutoDb.RemovidoAtendimento = true;

            await _atendimentoProdutoRepository.Atualizar(atendimentoProdutoDb);
        }

        public async Task RemoverDaVenda(AtendimentoProduto atendimentoProduto)
        {
            var atendimentoProdutoDb = await _atendimentoProdutoRepository.ObterPorId(atendimentoProduto.Id);

            if (atendimentoProdutoDb == null)
            {
                Notificar("Não foi possível obter as informações do produto.");
                return;
            }

            atendimentoProdutoDb.RemovidoVenda = true;

            await _atendimentoProdutoRepository.Atualizar(atendimentoProdutoDb);
        }

        public async Task AdicionarAVenda(AtendimentoProduto atendimentoProduto)
        {
            var atendimentoProdutoDb = await _atendimentoProdutoRepository.ObterPorId(atendimentoProduto.Id);

            if (atendimentoProdutoDb == null)
            {
                Notificar("Não foi possível obter as informações do produto.");
                return;
            }

            atendimentoProdutoDb.RemovidoVenda = false;

            await _atendimentoProdutoRepository.Atualizar(atendimentoProdutoDb);
        }

        public void Dispose()
        {
            _atendimentoProdutoRepository?.Dispose();
        }
    }
}
