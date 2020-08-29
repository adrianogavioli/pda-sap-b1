using Frattina.Business.Models;
using System;
using System.Threading.Tasks;

namespace Frattina.Business.Interfaces.Services
{
    public interface IUsuarioProdutoConsultaService : IDisposable
    {
        Task Adicionar(UsuarioProdutoConsulta usuarioProdutoConsulta);
    }
}
