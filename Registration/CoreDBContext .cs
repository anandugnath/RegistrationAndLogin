using Microsoft.EntityFrameworkCore;
using Registration.Models;

namespace Registration
{
    public partial class CoreDBContext: DbContext
    {
        public CoreDBContext()
        { }
        public CoreDBContext(DbContextOptions<CoreDBContext> options)
            : base(options)
        { }
        public virtual DbSet<Users> Users { get; set; }
    }
}
