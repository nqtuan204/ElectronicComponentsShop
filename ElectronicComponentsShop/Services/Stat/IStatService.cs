using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicComponentsShop.Services.Stat
{
    public interface IStatService
    {
        decimal GetRevenue(DateTime from, DateTime to);
        IEnumerable<object> GetRevenueStat(DateTime from, DateTime to);
        IEnumerable<object> GetRevenueStatByLocal(DateTime from, DateTime to);
        IDictionary<string, decimal> GetTopCustomers(DateTime from, DateTime to);
        IDictionary<string, decimal> GetTopProducts(DateTime from, DateTime to);
        IEnumerable<object> GetCategoriesStat(DateTime from, DateTime to);
    }
}
