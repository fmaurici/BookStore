using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IBusiness
{
    public interface IBookManager
    {
        Task<int> Rent(Guid id);
        Task<int> Return(Guid id);
    }
}
