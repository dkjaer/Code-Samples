using System.Data.Entity;

namespace Web
{
    /// <summary>
    /// Simple class to simulate an Entity Framework database schema.
    /// </summary>
    internal class AzureSqlEntities : DbContext
    {
        public AzureSqlEntities()
        {
        }

        public DbSet<MvcModel> Users { get; internal set; }
    }
}