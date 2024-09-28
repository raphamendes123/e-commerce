using Core.Message;

namespace Core.Domain.Repository.DomainObjects
{

    public abstract class Entity
    {
        public Guid Id { get; set; }

        protected Entity() 
        { 
            Id = Guid.NewGuid();
        }

        private List<Event> _events;

        public IReadOnlyCollection<Event> Events => _events?.AsReadOnly();

        public void AddEvent(Event _event)
        {
            _events = _events ?? new List<Event>();
            _events.Add(_event);
        }
        public void RemoveEvent(Event _event)
        {
            _events?.Remove(_event);
        }
        public void ClearEvents()
        {
            _events?.Clear();
        }

        #region Comparacoes
        public static bool operator ==(Entity b1, Entity b2)
        {
            if (b1 is null)
                return b2 is null;

            return b1.Equals(b2);
        }

        public static bool operator !=(Entity b1, Entity b2)
        {
            return !(b1 == b2);
        }

        public override bool Equals(object? obj)
        {
            Entity? compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return true;

            return Id.Equals(compareTo.Id);
        }


        public override int GetHashCode()
        {
            return GetType().GetHashCode() * 907 + Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }
        #endregion

    }
}
