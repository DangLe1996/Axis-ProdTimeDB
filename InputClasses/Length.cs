using System;
using System.Collections.Generic;
using System.Data;

namespace Axis_ProdTimeDB.InputClasses
{
    class Length : Utilities
    {
        public Length(string paramFilePath)
        {
            string optionName = this.GetType().Name;
            var dt = ConvertCSVtoDataTable(paramFilePath);


            Dictionary<string, List<string>> SectionParam = new Dictionary<string, List<string>>();






            foreach (DataRow row in dt.Rows)
            {
                SectionParam["SR1"] = new List<string>
            {

                row["SR1"].ToString(),
                row["Cartridge 1 SR1"].ToString(),
                row["Cartridge 2 SR1"].ToString(),
                row["Cartridge 3 SR1"].ToString()
            };
                SectionParam["SRM"] = new List<string>
            {

                row["SRM"].ToString(),
                row["Cartridge 1 SRM"].ToString(),
                row["Cartridge 2 SRM"].ToString(),
                row["Cartridge 3 SRM"].ToString()
            };

                SectionParam["SRE"] = new List<string>
            {
                row["SRE"].ToString(),
                row["Cartridge 1 SRE"].ToString(),
                row["Cartridge 2 SRE"].ToString(),
                row["Cartridge 3 SRE"].ToString()
            };

                int length = Int32.Parse(row["Length (feet)"].ToString());
                OptionTB.AddInstance(optionName, 0.0, length);

                foreach (KeyValuePair<string, List<string>> section in SectionParam)
                {
                    foreach (var x in section.Value)
                    {
                        if (!string.IsNullOrEmpty(x))
                        {
                            ParametersTB.AddInstance(section.Key, x);
                            OptionTB.AddParam(optionName, 0.0, section.Key, x, length);
                        }
                    }
                }

            }


        }
    }
}
