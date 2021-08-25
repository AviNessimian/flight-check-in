using Britannica.Application.Bases;
using Britannica.Domain.Bases;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Britannica.Infrastructure.Common
{
    public static class QueyableExtensions
    {
        public static async Task<PaginatedResponse<T>> PaginateAsync<T>(
            this IQueryable<T> source,
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
             where T : AuditableEntity
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
            return new PaginatedResponse<T>(items, count, pageIndex, pageSize);
        }
    }
}
