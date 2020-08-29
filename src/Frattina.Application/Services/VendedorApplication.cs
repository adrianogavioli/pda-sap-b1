using AutoMapper;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces;
using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Models.Enums;
using Frattina.SapService.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frattina.Application.Services
{
    public class VendedorApplication : BaseApplication, IVendedorApplication
    {
        private readonly IVendedorSapService _vendedorSapService;
        private readonly IAtendimentoRepository _atendimentoRepository;
        private readonly IAtendimentoProdutoRepository _atendimentoProdutoRepository;
        private readonly IMapper _mapper;

        public VendedorApplication(
            IVendedorSapService vendedorSapService,
            IAtendimentoRepository atendimentoRepository,
            IAtendimentoProdutoRepository atendimentoProdutoRepository,
            IMapper mapper,
            INotificador notificador) : base(notificador)
        {
            _vendedorSapService = vendedorSapService;
            _atendimentoRepository = atendimentoRepository;
            _atendimentoProdutoRepository = atendimentoProdutoRepository;
            _mapper = mapper;
        }

        public async Task<VendedorSapViewModel> ObterVendedor(int id)
        {
            return _mapper.Map<VendedorSapViewModel>(await _vendedorSapService.ObterVendedor(id));
        }

        public async Task<List<VendedorSapViewModel>> ObterTodos()
        {
            return _mapper.Map<List<VendedorSapViewModel>>(await _vendedorSapService.ObterTodos());
        }

        public async Task<VendedorVisaoViewModel> ObterVisao(int id)
        {
            var vendedorVisaoViewModel = new VendedorVisaoViewModel
            {
                VendedorId = id
            };

            await PopularVisaoAtendimentos(vendedorVisaoViewModel);

            await PopularVisaoPoderCompra(vendedorVisaoViewModel);

            return vendedorVisaoViewModel;
        }

        private async Task<VendedorVisaoViewModel> PopularVisaoAtendimentos(VendedorVisaoViewModel vendedorVisaoViewModel)
        {
            var atendimentos = await _atendimentoRepository.Buscar(a => a.VendedorId == vendedorVisaoViewModel.VendedorId);

            if (atendimentos == null || atendimentos.Count() == 0) return vendedorVisaoViewModel;

            vendedorVisaoViewModel.QuantidadeAtendimentos = atendimentos.Count();
            vendedorVisaoViewModel.QuantidadeVendas = atendimentos.Count(a => a.Etapa == AtendimentoEtapa.Vendido);
            vendedorVisaoViewModel.TaxaConversaoVenda = (vendedorVisaoViewModel.QuantidadeVendas / vendedorVisaoViewModel.QuantidadeAtendimentos) * 100;

            var ultimoAtendimento = atendimentos.OrderByDescending(a => a.Data).FirstOrDefault();
            vendedorVisaoViewModel.DataUltimoAtendimento = ultimoAtendimento.Data;
            vendedorVisaoViewModel.ClienteUltimoAtendimento = ultimoAtendimento.ClienteNome;

            return vendedorVisaoViewModel;
        }

        private async Task<VendedorVisaoViewModel> PopularVisaoPoderCompra(VendedorVisaoViewModel vendedorVisaoViewModel)
        {
            var atendimentoProdutos = await _atendimentoProdutoRepository.Buscar(p => p.Atendimento.VendedorId == vendedorVisaoViewModel.VendedorId
                                                                                && p.Atendimento.Etapa == AtendimentoEtapa.Vendido
                                                                                && !p.RemovidoVenda);

            if (atendimentoProdutos == null || atendimentoProdutos.Count() == 0) return vendedorVisaoViewModel;

            vendedorVisaoViewModel.ValorVendas = atendimentoProdutos.Sum(p => p.ValorNegociado);

            vendedorVisaoViewModel.TicketMedio = vendedorVisaoViewModel.ValorVendas / atendimentoProdutos.Count();

            return vendedorVisaoViewModel;
        }

        public void Dispose()
        {
            _vendedorSapService?.Dispose();
        }
    }
}
