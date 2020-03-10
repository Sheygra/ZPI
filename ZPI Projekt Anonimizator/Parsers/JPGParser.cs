using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ZPI_Projekt_Anonimizator.Parsers
{
    class JPGParser : DocumentParser
    {
        public DataTable parseDocument(String path)
        {
            DataTable table = new DataTable();

            table.Columns.Add("dane1", typeof(String));
            table.Columns.Add("dane2", typeof(int));
            // itd...

            var row = table.NewRow();
            row["dane1"] = "DANE1";
            row["dane2"] = 2;

            table.Rows.Add(row);

            return table;
        }
    }
}
