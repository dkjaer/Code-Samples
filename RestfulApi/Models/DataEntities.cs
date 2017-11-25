using System.Data.Entity;

namespace RestfulApi.Models
{
    /// <summary>
    /// Simple class to simulate an Entity Framework database schema.
    /// </summary>
    internal class DataEntities : DbContext
    {
        public DataEntities()
        {
        }

        public DbSet<UserModel> Users { get; internal set; }
    }
}