using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis_ProdTimeDB.DAL
{
    public class TimeContext: DbContext
    {

        public TimeContext()
            //:base ("Epicorssrs")
            //: base("data source =dangl-wks;initial catalog=AXIS Automation;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework")
            :base(@"Data Source = EPICORSSRS\MSSQLAPPS;Initial Catalog = ProdTime; User ID = epicoradmin; Password=Ep1c0r4Life!;Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")



        {
        }
        //public DbSet<ProdFamTB> ProdFam { get; set; }
        public DbSet<ProdTB> Prod { get; set; }
        public DbSet<OptionTB> Options { get; set; }
        public DbSet<ParametersTB> Params { get; set; }

        //public DbSet<FixtureTB> Fixtures { get; set; }

    }
}
