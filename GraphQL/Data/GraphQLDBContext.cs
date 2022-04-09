using GraphQL.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Data
{
    public class GraphQLDBContext : DbContext
    {
        public GraphQLDBContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Platform> Platforms { get; set; }
    }   
}