using Frattina.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Interfaces
{
    public interface IProdutoEstoqueApplication : IDisposable
    {
        Task<List<ProdutoEstoqueSapViewModel>> ObterPorProduto(string produtoId);
    }
}
