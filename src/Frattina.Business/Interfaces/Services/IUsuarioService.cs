using Frattina.Business.Models;
using System;
using System.Threading.Tasks;

namespace Frattina.Business.Interfaces.Services
{
    public interface IUsuarioService : IDisposable
    {
        Task Adicionar(Usuario usuario);

        Task Atualizar(Usuario usuario);

        Task Remover(Usuario usuario);
    }
}
