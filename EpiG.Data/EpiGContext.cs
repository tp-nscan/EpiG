using System.Data.Entity;

namespace EpiG.Data
{
    public class EpiGContext : DbContext
    {
        public EpiGContext() : base("punky") {}
        public IDbSet<Sorter> Sorters { get; set; }
    }
}
