using System;
using Laboris.DAL;
using Laboris.Models;
using Microsoft.EntityFrameworkCore;

namespace Laboris.Services;

public class LayoutService
{
    private readonly AppDbContext _context;

    public LayoutService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<BasketItem>> GetBasketItemsAsync(string userId)
    {
        var basketItems = await _context.BasketItems.Where(x => x.UserId == userId).Include(x => x.Product).ThenInclude(x => x.ProductImages).ToListAsync();

        return basketItems;
    }
}
