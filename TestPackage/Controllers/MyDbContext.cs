using Microsoft.EntityFrameworkCore;

namespace TestPackage.Controllers
{
    public class MyDbContext: DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            
        }
    }
}
