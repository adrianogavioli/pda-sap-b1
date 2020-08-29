using Frattina.Business.Interfaces;
using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Interfaces.Services;
using Frattina.Business.Models;
using Frattina.Business.Models.Validations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Frattina.Business.Services
{
    public class CargoService : BaseService, ICargoService
    {
        private readonly ICargoRepository _cargoRepository;

        public CargoService(ICargoRepository cargoRepository, INotificador notificador) : base(notificador)
        {
            _cargoRepository = cargoRepository;
        }

        public async Task Adicionar(Cargo cargo)
        {
            if (!ExecutarValidacao(new CargoValidation(), cargo)) return;

            if (cargo.Descricao != null && _cargoRepository.Buscar(c => c.Descricao == cargo.Descricao).Result.Any())
            {
                Notificar("O cargo informado já está cadastrado.");
                return;
            }

            await _cargoRepository.Adicionar(cargo);
        }

        public async Task Atualizar(Cargo cargo)
        {
            var cargoDb = await _cargoRepository.ObterPorId(cargo.Id);

            if (cargoDb == null)
            {
                Notificar("Não foi possível obter as informações do cargo.");
                return;
            }

            if (!ExecutarValidacao(new CargoValidation(), cargo)) return;

            if (cargo.Descricao != null && _cargoRepository.Buscar(c => c.Descricao == cargo.Descricao && c.Id != cargo.Id).Result.Any())
            {
                Notificar("O cargo informado já está cadastrado.");
                return;
            }

            cargoDb.Descricao = cargo.Descricao;

            await _cargoRepository.Atualizar(cargoDb);
        }

        public async Task Remover(Guid id)
        {
            if (_cargoRepository.ObterCargoUsuarios(id).Result.Usuarios.Any())
            {
                Notificar("Este cargo possui vínculos com usuários, portanto não pode ser removido.");
                return;
            }

            await _cargoRepository.Remover(id);
        }

        public void Dispose()
        {
            _cargoRepository?.Dispose();
        }
    }
}
