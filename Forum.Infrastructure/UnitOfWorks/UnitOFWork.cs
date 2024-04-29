using Forum.Application.IUser.IRepository;
using Forum.Application.UOW;
using Forum.Persistence.Context;

namespace Forum.Infrastructure.UnitOfWorks
{
    public class UnitOFWork
    {
        protected readonly DataContext _dbContext;

        public UnitOFWork(DataContext dbContext) 
        {
            _dbContext = dbContext;
        }


        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
