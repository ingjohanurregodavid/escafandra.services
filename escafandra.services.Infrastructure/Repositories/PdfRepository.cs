using escafandra.services.Domain.Entities;
using escafandra.services.Infrastructure.Data;
using escafandra.services.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace escafandra.services.Infrastructure.Repositories
{
    public class PdfRepository: IPdfRepository
    {
        //TODO: Implementación de repository
        private readonly MainDbContext _context;

        public PdfRepository(MainDbContext context)
        {
            _context = context;
        }

        public async Task<PDFDocument> GetByIdAsync(int id)
        {
            return await _context.PDFDocuments.FindAsync(id);
        }

        public async Task<IEnumerable<PDFDocument>> GetAllAsync()
        {
            return await _context.PDFDocuments.ToListAsync();
        }

        public async Task AddAsync(PDFDocument document)
        {
            await _context.PDFDocuments.AddAsync(document);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PDFDocument document)
        {
            _context.PDFDocuments.Update(document);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var document = await GetByIdAsync(id);
            if (document != null)
            {
                _context.PDFDocuments.Remove(document);
                await _context.SaveChangesAsync();
            }
        }
    }
}
