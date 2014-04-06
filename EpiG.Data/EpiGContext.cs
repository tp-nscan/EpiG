using System.Data.Entity;

namespace EpiG.Data
{
    public class EpiGContext : DbContext
    {
        public IDbSet<Sorter> Sorters { get; set; }
    }
}
