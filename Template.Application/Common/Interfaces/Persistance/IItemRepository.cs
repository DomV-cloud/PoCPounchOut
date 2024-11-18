using Template.Domain.Entities;

namespace Template.Application.Common.Interfaces.Persistance
{
    public interface IItemRepository
    {
        public Task<List<Item>> GetAllItems();
    }
}
