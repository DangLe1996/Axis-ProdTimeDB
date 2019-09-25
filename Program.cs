using Axis_ProdTimeDB.InputClasses;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace Axis_ProdTimeDB
{

    class Program
    {
        static string currentDir = Directory.GetCurrentDirectory();
        static string dataDir = currentDir + @"\Data\";
        public static string prodtype = "ProdID";
        public static string prodfamtype = "ProdFamID";
        public static string fixturetype = "FixtureID";
        static void Main(string[] args)
        {
            int quotenum = 102229;
            int quoteline = 1;
            
            EpicorInteract.AxisProdStdCalculator testquote = new EpicorInteract.AxisProdStdCalculator();
            testquote.getProdStd(quotenum, quoteline);

            //initializer();
            //using (var tdb = new TimeContext())
            //{
            //    foreach (var row in tdb.Params.ToList())
            //    {
            //        int i = 2;

            //    }
            //}

            //using (var db = new ParamContext())
            //{
            //    var list = db.Parameters.ToList();
            //    using (var tdb = new TimeContext())
            //    {
            //        foreach (var row in tdb.Params.ToList())
            //        {
            //            var paramref = db.Parameters.Where(r => r.Code == row.ParamValue).FirstOrDefault();
            //            if (paramref != null)
            //            {
            //                int i = 1;
            //                row.ParameterRef = paramref;
            //                try
            //                {
            //                    tdb.SaveChanges();
            //                }
            //                catch (DbEntityValidationException dbEx)
            //                {
            //                    // Iterate over the errors and write the trace information to make it a little easier to debug
            //                    // We coudl also wrap this up with our RuleException for any modeldb issues that we don't catch with our 
            //                    // domain rules.
            //                    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //                    {
            //                        foreach (var validationError in validationErrors.ValidationErrors)
            //                        {
            //                            Trace.TraceInformation("Property: {0}, Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
            //                        }
            //                    }

            //                    throw;
            //                }

            //            }
            //        }
            //    }

            //}

            //using (var db = new AxisCustom())
            //{
            //    var parameters = db.Parameters.ToList();
            //    var category = db.CategoryTypes.ToList();

            //    using (var customdb = new TimeContext())
            //    {

            //    }
            //}
            //using (var customdb = new TimeContext())
            //{
            //    foreach (ParametersTB row in customdb.Params)
            //    {
            //        using (var db = new AxisCustom())
            //        {
            //            var paramindex = db.Parameters.Where(r => r.Code == row.ParamValue).FirstOrDefault();


            //        }
            //        customdb.SaveChanges();
            //    }


            //}





        }

        public static void initializer()
        {
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



        

    }


    
       

        
    





}
