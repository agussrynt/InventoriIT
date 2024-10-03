using PlanCorp.Areas.Master.Models;

namespace PlanCorp.Areas.Master.Interface
{
    public interface IFungsiService
    {
        List<Fungsi> GetAll();

        List<Fungsi> Gets();
        Fungsi Get(string id);
    }
}
