using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ZPI_Projekt_Anonimizator.Parsers
{
    interface DocumentParser
    {
        public DataRow parseDocument(String path); //bierze sciezke do dokumentu i zwraca dane w nim zawarte w formie rekordu tabeli,
                                                   //ktory mozna potem wstawic do DataTable, ktore bedzie anonimizowane
    }
}
