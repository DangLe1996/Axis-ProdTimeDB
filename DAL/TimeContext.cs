using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis_ProdTimeDB.DAL
{
    class TimeContext: DbContext
    {
        //public DbSet<ProdFamTB> ProdFam { get; set; }
        public DbSet<ProdTB> Prod { get; set; }
        public DbSet<OptionTB> Options { get; set; }
        public DbSet<ParametersTB> Params { get; set; }

        //public DbSet<FixtureTB> Fixtures { get; set; }

    }
}
