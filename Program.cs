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

            //MSTime MSTimeInput = new MSTime(@"C:\Users\dangl\source\repos\Axis-ProdTimeDB\Data\MSTime.csv");

            //Ballast ballastInput = new Ballast(@"C:\Users\dangl\Source\repos\Axis-ProdTimeDB\Data\BallastFix.csv");
            //SubEndP subEndPInput = new SubEndP(@"C:\Users\dangl\source\repos\Axis-ProdTimeDB\Data\SubEndP.csv");
            //Optic opticInput = new Optic(@"C:\Users\dangl\source\repos\Axis-ProdTimeDB\Data\Optic.csv");
            //CartBoard cartBoardInput = new CartBoard(@"C:\Users\dangl\Source\repos\Axis-ProdTimeDB\Data\CartridgeBoard.csv");




            string input = "BBRLED-1000-80-35-FL-8'-CB315251-W-UNV-LT-1-TB15-B3-CP";
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
