using Mon_resto_backend.Models.Repository;
using Mon_resto_backend.Models;
using Microsoft.EntityFrameworkCore;

public class OffreRepository : IOffreRepository
{
    private readonly MonRestoDbContext _context;

    public OffreRepository(MonRestoDbContext context)
    {
        _context = context;
    }

    public async Task<Offre> CreerOffreAsync(Offre offre)
    {
        _context.Offres.Add(offre);
        await _context.SaveChangesAsync();
        return offre;
    }

    public async Task<Offre?> ObtenirOffreParIdAsync(int id)
    {
        return await _context.Offres.Include(o => o.Article).FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Offre>> ObtenirToutesLesOffresAsync()
    {
        return await _context.Offres.Include(o => o.Article).ToListAsync();
    }

    public async Task SupprimerOffreAsync(int id)
    {
        var offre = await ObtenirOffreParIdAsync(id);
        if (offre != null)
        {
            _context.Offres.Remove(offre);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<List<Offre>> ObtenirOffresActivesAsync()
    {
        var maintenant = DateTime.Now;
        return await _context.Offres
            .Include(o => o.Article)
            .Where(o => o.DateDebut <= maintenant && o.DateFin >= maintenant)
            .ToListAsync();
    }
}
