using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Interfaces.Persistance;
using Template.Application.DatabaseContext;
using Template.Domain.Entities;

namespace Template.Infrastructure.Persistance
{
    public class ItemRepository : IItemRepository
    {
        private readonly TemplateContext _context;

        public ItemRepository(TemplateContext context)
        {
            _context = context;
        }

        public async Task<List<Item>> GetAllItems()
        {
            return await _context.Items.ToListAsync();
        }
    }
}
