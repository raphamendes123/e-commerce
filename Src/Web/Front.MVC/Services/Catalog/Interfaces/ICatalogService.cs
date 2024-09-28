using Front.MVC.Models;

namespace Front.MVC.Services.Catalog
{
    public interface ICatalogService
    {
        Task<PagedViewModel<ProductViewModel>> GetAll(int pageSize, int pageIndex, string query = null);

        Task<ProductViewModel> GetById(Guid id);

    }
}
