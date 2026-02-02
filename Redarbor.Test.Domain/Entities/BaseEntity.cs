namespace Redarbor.Test.Domain.Entities
{
    /// <summary>
    /// Base entity for all domain entities
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int Id { get; set; }
    }
}
