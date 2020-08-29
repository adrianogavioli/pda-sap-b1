using Frattina.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Interfaces
{
    public interface IVendaApplication : IDisposable
    {
        Task<VendaSapViewModel> Adicionar(VendaSapViewModel venda);

        Task<VendaSapViewModel> ObterVenda(int id);

        Task<VendaSapViewModel> ObterPorNumeroNf(int numeroNf);

        Task<List<VendaSapViewModel>> ObterPorCliente(string clienteId);

        Task<List<VendaSapViewModel>> ObterPorVendedor(int vendedorId);

        Task<List<VendaSapViewModel>> ObterPorData(DateTime dataEmissaoIni, DateTime dataEmissaoFim);

        Task<bool> ClienteEstaAptoParaComprar(ClienteSapViewModel cliente);
    }
}
