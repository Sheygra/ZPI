using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ZPI_Projekt_Anonimizator.Parsers
{
    interface DocumentParser
    {
        public DataTable parseDocument(String path); //bierze sciezke do dokumentu i zwraca dane w nim zawarte jako rekord tabeli
    }
}
