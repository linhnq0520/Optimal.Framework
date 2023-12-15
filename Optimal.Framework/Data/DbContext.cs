using Microsoft.EntityFrameworkCore;
using Optimal.Framework.Data.Extensions;

namespace Optimal.Framework.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

    }
}
