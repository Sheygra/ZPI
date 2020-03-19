using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ZPI_Projekt_Anonimizator.Parsers
{
    class XMLParser : DocumentParser
    {
        public DataTable parseDocument(string path)
        {
            DataTable objDataTable;
            DataSet ds;
            try
            {
                objDataTable = new DataTable();
                ds = new DataSet();
                String s  ="";
                ds.ReadXml(path);
                objDataTable = ds.Tables[0];
                
                
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return objDataTable;
        }
    }
}
