using Axis_ProdTimeDB.DAL;
using Axis_ProdTimeDB.InputClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AXISAutomation;
namespace Axis_ProdTimeDB
{

    class Program
    {
        static string currentDir = Directory.GetCurrentDirectory();
        static string dataDir = currentDir + @"\Data\";
        static void Main(string[] args)
        {

            //MSTime MSTimeInput = new MSTime(@"MSTime.csv");
            //Driver ballastInput = new Driver(@"BallastFix.csv");
            //SubEndP subEndPInput = new SubEndP(@"SubEndP.csv");
            //Optic opticInput = new Optic(@"Optic.csv");
            //CartBoard cartBoardInput = new CartBoard(@"CartridgeBoard.csv");
            //Pack packInput = new Pack(@"Pack.csv");
            //ExitW exitInput = new ExitW(@"ExitW.csv");
            //PowerC powerCInput = new PowerC(@"PowerC.csv");
            //ScrewRef screwInput = new ScrewRef(@"ScrewRef.csv");
            //Housing housingInput = new Housing(@"Housing.csv");
            //Battery batteryInput = new Battery("Battery.csv");
            //BackC backCInput = new BackC("BackC.csv");
            //CB cBInput = new CB("CB.csv");
            //ChicPle chicPleInput = new ChicPle("ChicPle.csv");
            //CounterW counterInputer = new CounterW("CounterW.csv");
            //Length lengthInput = new Length("Length.csv");
            //Emergency emergencyInput = new Emergency("Emergency.csv");
            //EndC endCInput = new EndC("EndC.csv");
            //EndFeed endFeedInput = new EndFeed("EndFeed.csv");
            //EndP endPInput = new EndP("EndP.csv");
            //Nightlight nightlightInput = new Nightlight("Nightlight.csv");
            //FlexWhip flexWhipInput = new FlexWhip("FlexWhip.csv");
            //Fuse fuseInput = new Fuse("Fuse.csv");
            //Hanger hangerInput = new Hanger("Hanger.csv");
            //Inspection inspectionInput = new Inspection("Inspection.csv");
            //ITS iTSInput = new ITS("ITS.csv");
            //LightC lightCInput = new LightC("LightC.csv");
            //OpTest opTestInput = new OpTest("OpTest.csv");
            //PowerCord powerCordinput = new PowerCord("PowerCord.csv");
            //Remo remoInput = new Remo("Remo.csv");
            //SubEndC subEndCInput = new SubEndC("SubEndC.csv");
            //SubOp subOpInput = new SubOp("SubOp.csv");

            //Dictionary<string, List<string>> MapParamValue = new Dictionary<string, List<string>>();
            //Dictionary<string, List<string>> badParams = new Dictionary<string,List<string>>();

            //Dictionary<string, List<string>> MapParamName = new Dictionary<string, List<string>>();
            //List<string> badParamName = new List<string>();

            //using (var db = new AxisCustom())
            //{
            //    var parameters = db.Parameters.ToList();
            //    var category = db.CategoryTypes.ToList();

            //    using (var customdb = new TimeContext())
            //    {

            //        foreach(var param in customdb.Params.ToList()){
            //            var paramValue = db.Parameters.Where(r => r.Code == param.ParamValue).FirstOrDefault();
            //            var paramName = db.CategoryTypes.Where(r => r.Description == param.ParamName).FirstOrDefault();
            //            if (paramValue != null)
            //            {
            //                try
            //                {
            //                    MapParamValue[paramValue.Code.ToString()].Add(param.ParamValue.ToString());

            //                }
            //                catch
            //                {
            //                    MapParamValue[paramValue.Code.ToString()] = new List<string>();
            //                    MapParamValue[paramValue.Code.ToString()].Add(param.ParamValue.ToString());

            //                }
            //            }
            //            else
            //            {
            //                try
            //                {
            //                    badParams[param.ParamName.ToString()].Add(param.ParamValue.ToString());

            //                }
            //                catch
            //                {
            //                    badParams[param.ParamName.ToString()] = new List<string>();
            //                    badParams[param.ParamName.ToString()].Add(param.ParamValue.ToString());

            //                }
            //            }



            //            if (paramName != null)
            //            {
            //                try
            //                {
            //                    MapParamName[paramName.Description.ToString()].Add(param.ParamName);
            //                }
            //                catch
            //                {
            //                    MapParamName[paramName.Description.ToString()] = new List<string>();
            //                    MapParamName[paramName.Description.ToString()].Add(param.ParamName);
            //                }
            //            }

            //            else
            //            {
            //                badParamName.Add(param.ParamName.ToString());
            //            }




            //        }

            //    }





            //}
            //using (var writer = new StreamWriter(currentDir + @"\badParams.csv"))
            //{
            //    foreach (var pair in badParams)
            //        foreach(var value in pair.Value)
            //        {
            //            writer.WriteLine("{0},{1}", pair.Key, value);
            //        }



            //}

            //WiresQty wiresInput = new WiresQty();

            using (var db = new TimeContext())
            {
                var MSTime = db.Options.Where(r => r.OptionName == "MSTime").ToList();
              

            }


            string input = "BBRLED-500-80-35-FL-55'-W-UNV-DP-1-TG9";
            ProductClass prod1 = new ProductClass(input);











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
