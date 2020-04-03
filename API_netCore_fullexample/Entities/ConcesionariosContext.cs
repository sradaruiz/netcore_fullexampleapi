using Microsoft.EntityFrameworkCore;

namespace UniriojaREST.Entities
{
    public class ConcesionariosContext : DbContext
    {
        public ConcesionariosContext(DbContextOptions<ConcesionariosContext> options)
           : base(options)
        {
            
        }

        public DbSet<Presupuesto> Presupuestos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
    }
}