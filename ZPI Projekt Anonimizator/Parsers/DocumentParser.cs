using System;
using System.Data;

namespace ZPI_Projekt_Anonimizator.Parsers
{
    interface DocumentParser
    {
        public DataTable parseDocument(String path);
    }
}
