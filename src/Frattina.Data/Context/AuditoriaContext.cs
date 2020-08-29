using Frattina.Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Frattina.Data.Context
{
    public static class AuditoriaContext
    {
        private static readonly List<EntityState> entityStates = new List<EntityState>() { EntityState.Added, EntityState.Modified, EntityState.Deleted };

        public static void LogarAuditoria(this FrattinaDbContext context, IHttpContextAccessor accessor)
        {
            var userId = Guid.Parse(accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var entries = context.ChangeTracker.Entries()
                .Where(x => entityStates.Contains(x.State) && x.Entity.GetType().IsSubclassOf(typeof(Entity)))
                .ToList();

            foreach (var entry in entries)
            {
                var newValue = string.Empty;
                var oldValue = string.Empty;

                switch (entry.State)
                {
                    case EntityState.Added:
                        newValue = JsonConvert.SerializeObject(entry.CurrentValues.Properties.ToDictionary(p => p.Name, p => entry.CurrentValues[p]));
                        break;
                    case EntityState.Modified:
                        oldValue = JsonConvert.SerializeObject(entry.OriginalValues.Properties.ToDictionary(p => p.Name, p => entry.OriginalValues[p]));
                        newValue = JsonConvert.SerializeObject(entry.CurrentValues.Properties.ToDictionary(p => p.Name, p => entry.CurrentValues[p]));
                        break;
                }

                var auditoria = new Auditoria()
                {
                    Data = DateTime.Now,
                    Tabela = entry.Entity.GetType().Name,
                    Evento = entry.State.ToString(),
                    Chave = Guid.Parse(entry.CurrentValues["Id"].ToString()),
                    ValorAntigo = oldValue,
                    ValorAtual = newValue,
                    UsuarioId = userId
                };

                context.Auditorias.Add(auditoria);
            }
        }
    }
}
