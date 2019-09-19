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

namespace Axis_ProdTimeDB
{

    class Program
    {
        static void Main(string[] args)
        {
            string currentDir = Directory.GetCurrentDirectory();
            string dataDir = currentDir + @"\Data\";
            //MSTime MSTimeInput = new MSTime(dataDir + @"MSTime.csv");
            //Driver ballastInput = new Driver(dataDir + @"BallastFix.csv");
            //SubEndP subEndPInput = new SubEndP(dataDir + @"SubEndP.csv");
            //Optic opticInput = new Optic(dataDir + @"Optic.csv");
            //CartBoard cartBoardInput = new CartBoard(dataDir + @"CartridgeBoard.csv");
            //Pack packInput = new Pack(dataDir + @"Pack.csv");
            //ExitW exitInput = new ExitW(dataDir + @"ExitW.csv");
            //PowerC powerCInput = new PowerC(dataDir + @"PowerC.csv");
            //ScrewRef screwInput = new ScrewRef(dataDir + @"ScrewRef.csv");
            //Housing housingInput = new Housing(dataDir + @"Housing.csv");
            //Battery batteryInput = new Battery(dataDir + "Battery.csv");
            //BackC backCInput = new BackC(dataDir + "BackC.csv");
            //CB cBInput = new CB(dataDir + "CB.csv");
            //ChicPle chicPleInput = new ChicPle(dataDir + "ChicPle.csv");
            //CounterW counterInputer = new CounterW(dataDir + "CounterW.csv");
            //Length lengthInput = new Length(dataDir + "Length.csv");





            string input = "BBRLED-1000-80-35-FL-6'-CB315251-W-UNV-LT-1-TB15-B3-CP";
            ProductClass prod1 = new ProductClass(input);











        }



        public DataTable ConvertCSVtoDataTable(string strFilePath)
        {
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
