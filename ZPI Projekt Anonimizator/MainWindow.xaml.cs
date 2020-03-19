using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZPI_Projekt_Anonimizator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            mojaTestowaFunkcja();


        }

        public void mojaTestowaFunkcja()
        {
            var jpg_gen = new ZPI_Projekt_Anonimizator.Generators.JPGGenerator();

            

            var jpg_parser = new ZPI_Projekt_Anonimizator.Parsers.JPGParser();

            var table = jpg_parser.parseDocument("path");

            

            var xml_gen = new ZPI_Projekt_Anonimizator.Generators.XMLGenerator(@"C:/Users/Artiom/Desktop/aninimizator/ZPI/ZPI Projekt Anonimizator/Generators/Files/XMLtest.txt");
            xml_gen.generateDocument("001");

            var xml_reader = new ZPI_Projekt_Anonimizator.Parsers.XMLParser();
            DataTable dt = xml_reader.parseDocument(@"C:/Users/Artiom/Desktop/aninimizator/ZPI/ZPI Projekt Anonimizator/Generators/Files/XMLtest001.txt");
            string s = "";
            foreach (DataRow dr in dt.Rows)
            {
                s = s + dr["id"].ToString() + ".  " + dr["Name"].ToString() + "  " +
                    dr["Surname"].ToString() + " |  " + dr["Address"].ToString() + "  (" +  dr["PhoneNumber"].ToString() + ")\n";

            }
            testTextBox1.Text = s;
        }

        private void testTextBox3_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void testTextBox4_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
