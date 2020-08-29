using Frattina.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Interfaces
{
    public interface IProdutoFichaTecnicaApplication : IDisposable
    {
        Task<List<ProdutoFichaTecnicaSapViewModel>> ObterPorProduto(string produtoId);
    }
}
