using ERP.Api.Models;

namespace ERP.Api.Repositories;

public interface IPlanillaRepository
{
    Task<List<Planilla>> GetByPeriodoAsync(string periodo);
    Task<Planilla?> GetByIdWithDetailAsync(int id);
}
