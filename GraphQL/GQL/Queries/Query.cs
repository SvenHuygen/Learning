using GraphQL.Data;
using GraphQL.Models;
using HotChocolate;

namespace GraphQL.GQL.Queries
{
    public class Query
    {
        public IQueryable<Platform> Platforms([Service] GraphQLDbContext context)
        {
            return context.Platforms;
        }
    }
}