using Microsoft.EntityFrameworkCore;

namespace StudentSystem.DataAccess.EntityFramework
{
    public class StudentSystemDbContextFactory : IDbContextFactory<StudentSystemDbContext>
    {
        private readonly IDbContextFactory<StudentSystemDbContext> _pooledFactory;
        
        public StudentSystemDbContextFactory(IDbContextFactory<StudentSystemDbContext> pooledFactory)
        {
            _pooledFactory = pooledFactory;
        }
        
        public StudentSystemDbContext CreateDbContext()
        {
            return _pooledFactory.CreateDbContext();
        }
    }
}