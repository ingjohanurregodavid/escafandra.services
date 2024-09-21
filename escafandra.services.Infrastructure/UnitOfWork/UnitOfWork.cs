using escafandra.services.Infrastructure.Repositories.Interfaces;
using escafandra.services.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using escafandra.services.Infrastructure.Data;

namespace escafandra.services.Infrastructure.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly MainDbContext _context;
        private IPdfRepository _pdfRepository;

        public UnitOfWork(MainDbContext context)
        {
            _context = context;
        }

        public IPdfRepository PdfRepository => _pdfRepository ??= new PdfRepository(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
