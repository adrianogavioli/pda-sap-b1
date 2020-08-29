﻿using Frattina.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Frattina.Business.Interfaces.Repositories
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task Adicionar(TEntity entity);

        Task Atualizar(TEntity entity);

        Task Remover(Guid id);

        Task<TEntity> ObterPorId(Guid id);

        Task<List<TEntity>> ObterTodos();

        Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate);

        Task<int> SaveChanges();
    }
}
