using escafandra.services.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace escafandra.services.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        IPdfRepository PdfRepository { get; }
        Task<int> CompleteAsync();
    }
}
