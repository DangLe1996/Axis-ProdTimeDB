using Axis_ProdTimeDB.DAL;
using System.Data;
using System.Linq;
using System.Data.Entity;
using System;

namespace Axis_ProdTimeDB
{

    class Program
    {

        public static int Main(string[] args)
        {
            try
            {
                string fixtureCode ;
                int quoteNum ;
                int quoteLine ;
                if (args.Length > 0)
                {
                    fixtureCode = args[0];
                    quoteNum = Int32.Parse(args[1]);
                    quoteLine = Int32.Parse(args[2]);

                }
                else
                {
                    
                    Console.Write("Enter a fixtureCode  - ");
                    fixtureCode = Console.ReadLine();
                    Console.Write("Enter a QuoteNum  - ");
                    quoteNum = Int32.Parse(Console.ReadLine());
                    Console.Write("Enter a QuoteLine  - ");
                    quoteLine = Int32.Parse(Console.ReadLine());
                  
                }

                AXISAutomation.Tools.DBConnection.AXIS_AutomationEntities _AutomationEntities = new AXISAutomation.Tools.DBConnection.AXIS_AutomationEntities();



                using (var db = new TimeContext())
                {
                    var listprod = db.Prod.Where(r => r.Code == "bbrled").ToList();
                }
                
                
                AXISAutomation.FixtureConfiguration._Fixture fixture = new AXISAutomation.FixtureConfiguration._Fixture(fixtureCode, _AutomationEntities);
               
                fixture.SPM.ConfigureAll();
                //fixture.SPM.ConfigureMountingKits();

                ProductClass test = new ProductClass(fixture, quoteNum, quoteLine);
                Console.WriteLine("Code executed!!!");
                return 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                var name = Console.ReadLine();
                return 0;
            }
            

        }








    }












}
