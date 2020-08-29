using Frattina.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Interfaces
{
    public interface IProdutoPrecoApplication : IDisposable
    {
        Task<List<ProdutoPrecoSapViewModel>> ObterPorProduto(string produtoId);
    }
}
