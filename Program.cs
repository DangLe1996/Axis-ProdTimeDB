using Axis_ProdTimeDB.DAL;
using System.Data;
using System.Linq;

namespace Axis_ProdTimeDB
{

    class Program
    {

        public static void Main(string[] args)
        {
            //Utilities util = new Utilities();
            //util.initializer();
            //MSTime mSTimeInput = new MSTime("MSTime1.csv");
            using (var db = new TimeContext())
            {

                var prod = db.Prod.Where(item => item.Code == "B6RLED" && item.WorkCenter == "Double Saw").ToList();






            }
            AXISAutomation.Tools.DBConnection.AXIS_AutomationEntities _AutomationEntities = new AXISAutomation.Tools.DBConnection.AXIS_AutomationEntities();

            bool exit = false;



            string fixtureCode = "B2SQDLED-1000-90-35-L-18-BLK-UNV-DP-1-CA(60)";
            fixtureCode = "STFC-NO-DSO(175-400)-80-35-AH(26)+AY(4)-W-UNV-MD-1+E(3)-CA(120)-RC";
            fixtureCode = "SP18429(2)-BBPRLED-1000-90-30-ASO-FL-5' 9\" - W - UNV - DP - 1 - DF";
            //string fxcode2 = "BBRLED-1000-90-40-FL-4-W-UNV-DP-1-TG15";
            AXISAutomation.Solvers.BOM._BOM bOM = new AXISAutomation.Solvers.BOM._BOM(fixtureCode, _AutomationEntities);

            ProductClass test = new ProductClass(bOM, 102228, 1, 1);

        }








    }












}
