using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.Database;
using Microsoft.EntityFrameworkCore;

namespace ElectronicComponentsShop.Services.Stat
{
    public class StatService : IStatService
    {
        private readonly ECSDbContext _db;
        public StatService(ECSDbContext db)
        {
            _db = db;
        }
        public decimal GetRevenue(DateTime from, DateTime to)
        {
            return _db.Orders.Where(o => o.OrderStateId == 4 && o.ModifiedAt >= from && o.ModifiedAt <= to).Sum(o => o.Items.Sum(i => i.Price * i.Quantity));
        }

        public IEnumerable<object> GetRevenueStat(DateTime from, DateTime to)
        {
            var orders = _db.Orders.Include(o => o.Items).Where(o => o.OrderStateId == 4 && from <= o.ModifiedAt && to >= o.ModifiedAt).AsEnumerable();

            return orders.GroupBy(o => new DateTime(o.ModifiedAt.Value.Year, o.ModifiedAt.Value.Month, o.ModifiedAt.Value.Day)).Select(g => new { x = g.Key, y = g.Sum(o => o.Items.Sum(i => i.Price * i.Quantity)) });
        }

        public IEnumerable<object> GetRevenueStatByLocal(DateTime from, DateTime to)
        {
            var orders = _db.Orders.AsSplitQuery().Include(o => o.Province).Include(o => o.Items).Where(o => o.OrderStateId == 4 && from <= o.ModifiedAt && to >= o.ModifiedAt).AsEnumerable();
            return orders.GroupBy(o => o.Province).Select(g => new { label = g.Key.Name, y = g.Key.Orders.Sum(o => o.Items.Sum(i => i.Price * i.Quantity)) });
        }

        public IDictionary<string, decimal> GetTopCustomers(DateTime from, DateTime to)
        {
            return _db.Users.AsSplitQuery().Include(u => u.Orders).ThenInclude(o => o.Items).AsEnumerable().GroupBy(u => u).Select(g => new { user = g.Key, paid = g.Key.Orders.Where(o => o.OrderStateId == 4 && o.ModifiedAt >= from && o.ModifiedAt <= to).Sum(o => o.Items.Sum(i => i.Price * i.Quantity)) }).OrderByDescending(g => g.paid).Take(5).ToDictionary(g => $"{g.user.LastName} {g.user.FirstName}", g => g.paid);
        }

        public IDictionary<string, decimal> GetTopProducts(DateTime from, DateTime to)
        {
            return _db.Products.AsSplitQuery().Include(p => p.OrderItems).ThenInclude(i => i.Order).AsEnumerable().Select(p => new { product = p, cost = p.OrderItems.Where(i => i.Order.OrderStateId == 4).Sum(i => i.Price * i.Quantity) }).GroupBy(p => p.product.Name).ToDictionary(g => g.Key, g => g.Sum(p => p.cost)).OrderByDescending(p => p.Value).Take(5).ToDictionary(p => p.Key, p => p.Value);
        }

        public IEnumerable<object> GetCategoriesStat(DateTime from, DateTime to)
        {
            var stat= _db.Categories.AsSplitQuery().Include(c => c.Products).ThenInclude(p => p.OrderItems).ThenInclude(i => i.Order).AsEnumerable().GroupBy(c => c).Select(g => new { label = g.Key.Name, legendText = g.Key.Name, y = g.Sum(c => c.Products.Sum(p => p.OrderItems.Where(i => i.Order.OrderStateId == 4 && i.Order.ModifiedAt >= from && i.Order.ModifiedAt <= to).Sum(i => i.Price * i.Quantity))) });
            var sum = stat.Sum(e => e.y);
            return stat.Select(e => new { y = Math.Round(e.y*100 / sum,2), label = e.label, legendText = e.legendText });
        }
    }
}
