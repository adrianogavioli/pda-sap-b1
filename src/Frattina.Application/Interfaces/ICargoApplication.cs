using Frattina.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Interfaces
{
    public interface ICargoApplication : IDisposable
    {
        Task<CargoViewModel> Adicionar(CargoViewModel cargoViewModel);

        Task Atualizar(CargoViewModel cargoViewModel);

        Task Remover(Guid id);

        Task<CargoViewModel> ObterCargo(Guid id);

        Task<CargoViewModel> ObterCargoUsuarios(Guid id);

        Task<List<CargoViewModel>> ObterTodos();
    }
}
