using escafandra.services.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace escafandra.services.Infrastructure.Repositories.Interfaces
{
    public interface IPdfRepository
    {
        Task<PDFDocument> GetByIdAsync(int id);
        Task<IEnumerable<PDFDocument>> GetAllAsync();
        Task AddAsync(PDFDocument document);
        Task UpdateAsync(PDFDocument document);
        Task DeleteAsync(int id);
    }
}
