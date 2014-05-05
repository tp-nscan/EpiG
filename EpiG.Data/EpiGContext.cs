using System.Data.Entity;

namespace EpiG.Data
{
    public class EpiGContext : DbContext
    {
        public EpiGContext() : base("Ralph") { }
        public IDbSet<Sorter> Sorters { get; set; }
    }
}
