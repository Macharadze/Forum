using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Application.IRepository
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetById(string ID, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default);
        Task Delete(string ID, CancellationToken cancellationToken = default);
    }
}
