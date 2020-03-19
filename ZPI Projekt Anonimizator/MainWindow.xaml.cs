using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using Microsoft.Win32;

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

            
        }
        private void btnOpenClick(Object sender, RoutedEventArgs rea)
        {
            string filePath ="";
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Filter = "XML file|*.xml";
            fileDialog.DefaultExt = ".xml";
            Nullable<bool> dialogOK = fileDialog.ShowDialog();
            if(dialogOK == true)
            {
                filePath = fileDialog.FileName;
            }
            try
            {
                /*var xml_gen = new ZPI_Projekt_Anonimizator.Generators.XMLGenerator(@"C:/Users/Artiom/Desktop/aninimizator/ZPI/ZPI Projekt Anonimizator/Generators/Files/XMLtest.txt");
                xml_gen.generateDocument("001");*/
                var xml_reader = new ZPI_Projekt_Anonimizator.Parsers.XMLParser();

                DataTable dt = xml_reader.parseDocument(filePath);
                string s = "";
                foreach (DataRow dr in dt.Rows)
                {
                    s = s + dr["id"].ToString() + ".  " + dr["Name"].ToString() + "  " +
                        dr["Surname"].ToString() + " |  " + dr["Address"].ToString() + "  (" + dr["PhoneNumber"].ToString() + ")\n";
                }
                testTextBox1.Text = s;
            }
            catch (Exception ex)
            {
                testTextBox1.Text = "Blędna sciezka!!\n" + ex;
            }

        }

        private void testTextBox3_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void testTextBox4_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void testTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
