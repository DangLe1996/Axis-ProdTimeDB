using Axis_ProdTimeDB.InputClasses;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace Axis_ProdTimeDB
{
    public class Utilities
    {
        static string currentDir = Directory.GetCurrentDirectory();
        static string dataDir = currentDir + @"\Data\";
        public static string prodtype = "ProdID";
        public static string prodfamtype = "ProdFamID";
        public static string fixturetype = "FixtureID";
        public DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            strFilePath = dataDir + strFilePath;
            StreamReader sr = new StreamReader(strFilePath);

            string[] headers = sr.ReadLine().Split(',');

            DataTable dt = new DataTable();
            foreach (string header in headers)
            {

                dt.Columns.Add(header.Trim());
            }
            while (!sr.EndOfStream)
            {
                string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    dr[i] = rows[i].Trim();
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public void initializer()
        {
            MKits mKitsInput = new MKits("Mkits.csv");
            MR mRInput = new MR("MR.csv");
            MSTime MSTimeInput = new MSTime(@"MSTime.csv");
            Driver ballastInput = new Driver(@"BallastFix.csv");
            SubEndP subEndPInput = new SubEndP(@"SubEndP.csv");
            Optic opticInput = new Optic(@"Optic.csv");
            CartBoard cartBoardInput = new CartBoard(@"CartridgeBoard.csv");
            Pack packInput = new Pack(@"Pack.csv");
            ExitW exitInput = new ExitW(@"ExitW.csv");
            PowerC powerCInput = new PowerC(@"PowerC.csv");
            ScrewRef screwInput = new ScrewRef(@"ScrewRef.csv");
            Housing housingInput = new Housing(@"Housing.csv");
            Battery batteryInput = new Battery("Battery.csv");
            BackC backCInput = new BackC("BackC.csv");
            CB cBInput = new CB("CB.csv");
            ChicPle chicPleInput = new ChicPle("ChicPle.csv");
            CounterW counterInputer = new CounterW("CounterW.csv");
            Length lengthInput = new Length("Length.csv");
            Emergency emergencyInput = new Emergency("Emergency.csv");
            EndC endCInput = new EndC("EndC.csv");
            EndFeed endFeedInput = new EndFeed("EndFeed.csv");
            EndP endPInput = new EndP("EndP.csv");
            Nightlight nightlightInput = new Nightlight("Nightlight.csv");
            FlexWhip flexWhipInput = new FlexWhip("FlexWhip.csv");
            Fuse fuseInput = new Fuse("Fuse.csv");
            Hanger hangerInput = new Hanger("Hanger.csv");
            Inspection inspectionInput = new Inspection("Inspection.csv");
            ITS iTSInput = new ITS("ITS.csv");
            LightC lightCInput = new LightC("LightC.csv");
            OpTest opTestInput = new OpTest("OpTest.csv");
            PowerCord powerCordinput = new PowerCord("PowerCord.csv");
            Remo remoInput = new Remo("Remo.csv");
            SubEndC subEndCInput = new SubEndC("SubEndC.csv");
            SubOp subOpInput = new SubOp("SubOp.csv");
        }
    }
}
