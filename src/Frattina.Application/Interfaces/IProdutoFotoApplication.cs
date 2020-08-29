using Frattina.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Interfaces
{
    public interface IProdutoFotoApplication : IDisposable
    {
        Task<ProdutoFotoSapViewModel> ObterFoto(int id);

        Task<List<ProdutoFotoSapViewModel>> ObterPorProduto(string produtoId);
    }
}
