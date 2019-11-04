using Axis_ProdTimeDB.DAL;
using System.Data;
using System.Linq;
using System.Data.Entity;
using System;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;
using Ice.Core;
using Ice.Lib.Framework;
using Ice.Proxy.BO;
using AXISAutomation.FixtureConfiguration;
namespace Axis_ProdTimeDB
{

    class Program
    {
        private static void getProdInfo(_Fixture fixture, ref string productCode, ref string fixtureCode, ref string ProdFamCode )
        {
            Session epiSession = new Session("Dang", "Lebaodang96!", "net.tcp://EPICORERP/Epicor10Test", Session.LicenseType.Default, @"C:\Epicor\ERP10.1Client\Client\Config\Epicor10Test.sysconfig");
            UD03Impl UD30BO = WCFServiceSupport.CreateImpl<UD03Impl>((Ice.Core.Session)epiSession, "Ice/BO/UD03");


            bool morepage;

            string productID = fixture.Selection.ProductID.SelectionBaseValue;
            if (productID == null)
            {
                throw new System.Exception("This product cannot be configured");

            }
            var BO = UD30BO.GetRows(string.Format("Key1 = 'PRODUCTID' and key2 = '{0}'", productID), "", 0, 0, out morepage);

            productCode = BO.UD03[0]["Key2"].ToString();
            fixtureCode = BO.UD03[0]["Character03"].ToString();
            ProdFamCode = BO.UD03[0]["Key3"].ToString();
        }

        public static int Main(string[] args)
        {
            try
            {
                string fixtureCode = "BBPRLED-400-80-35-RG3-5'-W-UNV-DP-1-DS";
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

                string DWConnectiontring = "metadata = res://*/DBConnection.csdl|res://*/DBConnection.ssdl|res://*/DBConnection.msl;provider=System.Data.SqlClient;provider connection string='Data Source=VAULT\\DRIVEWORKS;Initial Catalog=\"AXIS Automation\";Integrated Security=True;MultipleActiveResultSets=True'";
                AXISAutomation.Tools.DBConnection.AXIS_AutomationEntities _AutomationEntities = new AXISAutomation.Tools.DBConnection.AXIS_AutomationEntities(DWConnectiontring);
                _Fixture fixture = new AXISAutomation.FixtureConfiguration._Fixture(fixtureCode, _AutomationEntities);
               
                fixture.SPM.ConfigureAll();
                string productCode = string.Empty;
                string fxCode = string.Empty;
                string ProdFamCode = string.Empty;
                getProdInfo(fixture, ref productCode, ref fxCode, ref ProdFamCode);

                ProductClass test = new ProductClass(fixture, productCode, fxCode, ProdFamCode, 3);
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
