using Core.Domain.Repository.DomainObjects;
using Core.Mediator;
using Microsoft.EntityFrameworkCore;

namespace Core.Extensions
{
    public static class MediatorHandlerExtension
    {
        public static async Task PublishEvents<T>(this IMediatorHandler mediator, T ctx) where T : DbContext
        {
            var entities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Events != null && x.Entity.Events.Any());

            var domainEvents = entities
                .SelectMany(x => x.Entity.Events)
                .ToList();

            entities.ToList()
                .ForEach(entity => entity.Entity.ClearEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.PublishEvent(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
