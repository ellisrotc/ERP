using ERP.Api.Data;
using ERP.Api.Models;
using ERP.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ERP.Api.Services;

public interface IComprobanteService
{
    Task<List<ComprobanteDto>> GetAllAsync(DateTime? desde, DateTime? hasta);
    Task<ComprobanteDto> CreateAsync(ComprobanteCreateDto dto);
    Task<ComprobanteDto> GetByIdAsync(int id);
}

public class ComprobanteService(ErpDbContext db) : IComprobanteService
{
    public async Task<List<ComprobanteDto>> GetAllAsync(DateTime? desde, DateTime? hasta)
    {
        var query = db.Comprobantes
            .Include(c => c.TipoComprobante)
            .Where(c => c.Activo);

        if (desde.HasValue)
        {
            var desdeDate = DateOnly.FromDateTime(desde.Value);
            query = query.Where(c => c.Fecha >= desdeDate);
        }
        if (hasta.HasValue)
        {
            var hastaDate = DateOnly.FromDateTime(hasta.Value);
            query = query.Where(c => c.Fecha <= hastaDate);
        }

        var list = await query.OrderByDescending(c => c.Fecha).ToListAsync();
        return list.Select(ToDto).ToList();
    }

    public async Task<ComprobanteDto> CreateAsync(ComprobanteCreateDto dto)
    {
        if (!EsRucValido(dto.Ruc))
            throw new ArgumentException("RUC debe tener exactamente 11 dígitos numéricos");

        if (dto.Monto <= 0)
            throw new ArgumentException("El monto debe ser mayor a cero");

        var igv = Math.Round(dto.Monto / 1.18m * 0.18m, 2);

        var comp = new Comprobante
        {
            IdTipoComprobante = dto.IdTipo,
            Numero = dto.Numero,
            Serie = dto.Serie,
            Ruc = dto.Ruc,
            RazonSocial = dto.RazonSocial,
            Fecha = DateOnly.FromDateTime(dto.Fecha),
            Monto = dto.Monto,
            Igv = igv
        };

        db.Comprobantes.Add(comp);
        await db.SaveChangesAsync();
        return await GetByIdAsync(comp.IdComprobante);
    }

    public async Task<ComprobanteDto> GetByIdAsync(int id)
    {
        var comp = await db.Comprobantes.Include(c => c.TipoComprobante)
            .FirstOrDefaultAsync(c => c.IdComprobante == id)
            ?? throw new KeyNotFoundException($"Comprobante {id} no encontrado");
        return ToDto(comp);
    }

    public static bool EsRucValido(string ruc) =>
        ruc.Length == 11 && ruc.All(char.IsDigit);

    private static ComprobanteDto ToDto(Comprobante c) => new(
        c.IdComprobante,
        c.TipoComprobante?.Nombre ?? string.Empty,
        c.Numero,
        c.Serie ?? string.Empty,
        c.Ruc,
        c.RazonSocial ?? string.Empty,
        c.Fecha.ToDateTime(TimeOnly.MinValue),
        c.Monto,
        c.Igv,
        c.Activo);
}
