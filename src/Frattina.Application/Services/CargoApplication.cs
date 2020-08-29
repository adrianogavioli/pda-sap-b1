using AutoMapper;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces;
using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Interfaces.Services;
using Frattina.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Services
{
    public class CargoApplication : BaseApplication, ICargoApplication
    {
        private readonly ICargoService _cargoService;
        private readonly ICargoRepository _cargoRepository;
        private readonly IUsuarioApplication _usuarioApplication;
        private readonly IMapper _mapper;

        public CargoApplication(ICargoService cargoService,
            ICargoRepository cargoRepository,
            IUsuarioApplication usuarioApplication,
            IMapper mapper,
            INotificador notificador) : base(notificador)
        {
            _cargoService = cargoService;
            _cargoRepository = cargoRepository;
            _usuarioApplication = usuarioApplication;
            _mapper = mapper;
        }

        public async Task<CargoViewModel> Adicionar(CargoViewModel cargoViewModel)
        {
            var cargo = _mapper.Map<Cargo>(cargoViewModel);

            await _cargoService.Adicionar(cargo);

            if (!OperacaoValida()) return null;

            cargoViewModel = await ObterCargo(cargo.Id);

            if (cargoViewModel == null)
            {
                Notificar("Não foi possível adicionar o cargo.");
                return null;
            }

            return cargoViewModel;
        }

        public async Task Atualizar(CargoViewModel cargoViewModel)
        {
            await _cargoService.Atualizar(_mapper.Map<Cargo>(cargoViewModel));
        }

        public async Task Remover(Guid id)
        {
            await _cargoService.Remover(id);
        }

        public async Task<CargoViewModel> ObterCargo(Guid id)
        {
            return _mapper.Map<CargoViewModel>(await _cargoRepository.ObterCargo(id));
        }

        public async Task<CargoViewModel> ObterCargoUsuarios(Guid id)
        {
            return _mapper.Map<CargoViewModel>(await _cargoRepository.ObterCargoUsuarios(id));
        }

        public async Task<List<CargoViewModel>> ObterTodos()
        {
            return _mapper.Map<List<CargoViewModel>>(await _cargoRepository.ObterTodos());
        }

        public void Dispose()
        {
            _cargoService?.Dispose();
            _cargoRepository?.Dispose();
            _usuarioApplication?.Dispose();
        }
    }
}
