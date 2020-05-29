using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Xml.Linq;
using Microsoft.Win32;
using ZPI_Projekt_Anonimizator.Algorithm;
using ZPI_Projekt_Anonimizator.entity;
using ZPI_Projekt_Anonimizator.Generators;
using System.Linq;



namespace ZPI_Projekt_Anonimizator
{
    public partial class MainWindow : Window
    {
        DataTable patientDataGenerated = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void bindXMLBeforeGridData()
        {
            string filePath = "";
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Filter = "XML file|*.xml";
            fileDialog.DefaultExt = ".xml";
            Nullable<bool> dialogOK = fileDialog.ShowDialog();
            if (dialogOK == true)
            {
                filePath = fileDialog.FileName;
            }
            try
            {
                BrowseTextBlockPath.Text = filePath.Substring(0,3) + "(...)" + filePath.Substring(filePath.Length-20, 20);
                BrowseTextBlockPath.Visibility = Visibility.Visible;
                var xml_reader = new ZPI_Projekt_Anonimizator.Parsers.XMLParser();

                patientDataGenerated = xml_reader.parseDocument(filePath);

                XMLbeforeDataGrid.DataContext = patientDataGenerated.DefaultView;
                XMLbeforeDataGrid.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                promptUser("Unable to retrieve patient data. Please try again.");
            }
        }

        private void Browse_Button_Click(object sender, RoutedEventArgs e)
        {
            bindXMLBeforeGridData();
        }

        private void Generate_Button_Click(object sender, RoutedEventArgs e)
        {
            var filename = OutputFileNameInput.Text;
            int patientNumber = 0;
            if (filename == null || filename.Length == 0)
            {
                promptUser("File name is incorrect. Name has to be at least one character.");
            }
            else
            {
                Int32.TryParse(PatientNumberInput.Text, out patientNumber);
                if (patientNumber != 0)
                {
                    try
                    {
                        var xml_gen = new ZPI_Projekt_Anonimizator.Generators.XMLGenerator();
                        xml_gen.generateDocument(filename, patientNumber);

                        OutputFileNameInput.Text = "";
                        PatientNumberInput.Text = "";

                        promptUser("Successfully generated patient data file: " + filename + ".xml");
                    }
                    catch (Exception ex)
                    {
                        promptUser("Unable to generate patient data. An error occured.");
                    }
                }
                else
                {
                    promptUser("Patient number is incorrect. Enter an integer.");
                }
            }
        }

        private void RunAlgorithm_Button_Click(object sender, RoutedEventArgs e)
        {
            bool k_anonimization = RButton1KAnoAlgorithm.IsChecked == null ? false : RButton1KAnoAlgorithm.IsChecked == true ? true : false;
            bool k_alfa_anonimization = RButton1KAlfaAnoAlgorithm.IsChecked == null ? false : RButton1KAlfaAnoAlgorithm.IsChecked == true ? true : false;

            if(patientDataGenerated == null || patientDataGenerated.Rows.Count == 0)
            {
                promptUser("Patient data is empty.");

            }
            else if (k_anonimization)
            {
                try
                {
                    int k = 0;
                    var k_text = kValue.Text;
                    Int32.TryParse(k_text, out k);
                    kValue.Text = "";
                    if (k > 2 && k < 21)
                    {
                        var anonimizator = new Anonymization();
                        var anonymized = anonimizator.AnonymizeData(patientDataGenerated, k);

                        DocumentGenerator JPG_gen = new JPGGenerator();
                        DocumentGenerator DICOM_gen = new DICOMGenerator();
                        DocumentGenerator DOCX_gen = new DOCXGenerator();
                        List<ZPI_Projekt_Anonimizator.entity.Patient> dataBasePatients = new List<ZPI_Projekt_Anonimizator.entity.Patient>();
                        List<string> pathes = new List<string>();


                        foreach (DataRow row in anonymized.Rows)
                        {
                            var values = row.ItemArray;
                            Patient p = new Patient(values[0] + "", values[1] + "", values[2] + "", values[8] + "", values[7] + "", values[3] + "", values[5] + "", values[6] + "", values[4] + "");
                            dataBasePatients.Add(p);

                            if (!values[9].Equals(""))
                            {
                                string s = JPG_gen.generateDocument(p) + ";" + DICOM_gen.generateDocument(p) + ";" + DOCX_gen.generateDocument(p);
                                row.BeginEdit();
                                row[9] = s;
                                pathes.Add(s);
                            }
                            else
                            {
                                pathes.Add("");
                            }
                        }
                        anonymized.AcceptChanges();


                        XDocument xdoc = new XDocument(
                        new XDeclaration("1.0", "utf-8", "yes"),

                        new XElement("Patients",
                        from patient in dataBasePatients
                            select
                                new XElement("Patient", new XElement("Id", patient.Id),
                                new XElement("Name", patient.Name),
                                new XElement("Surname", patient.SurName),
                                new XElement("Gender", patient.Gender),
                                new XElement("DateOfBirth", patient.DateOfBirth),
                                new XElement("Profession", patient.Profession),
                                new XElement("City", patient.City),
                                new XElement("Address", patient.Address),
                                new XElement("PhoneNumber", patient.PhoneNumber),
                                new XElement("PathForFiles", pathes[Int32.Parse(patient.Id) - 1])
                        )));

                        string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName+@"/resource/XML_files/"+ "anonymized_data" + ".xml";
                        xdoc.Save(path);
                        
                        XMLAfterAnonimizationGrid.DataContext = anonymized.DefaultView;
                        XMLAfterAnonimizationGrid.Visibility = Visibility.Visible;
                    }
                    else promptUser("K has to be in range <3,20>");
                }
                catch(Exception ex)
                {
                    promptUser("Algoritm execution resulted in an error.");
                }
            }
            else if(k_alfa_anonimization)
            {
                try
                {
                    int k = 0;
                    var k_text = kValue.Text;
                    Int32.TryParse(k_text, out k);
                    kValue.Text = "";
                    if (k > 2 && k < 21)
                    {
                        var anonimizator = new KAnonymization();
                        anonimizator.add(patientDataGenerated, k);
                        var anonymized = anonimizator.normalize();
                        XMLAfterAnonimizationGrid.DataContext = anonymized.DefaultView;
                        XMLAfterAnonimizationGrid.Visibility = Visibility.Visible;
                    }
                    else promptUser("K has to be in range <3,20>");
                }
                catch(Exception ex)
                {
                    promptUser("Algoritm execution resulted in an error.");
                }
            }
            else
            {
                promptUser("One of algorithms must be selected.");
            }
            
        }    

        public void showJPGMetadata(String path)
         {
            try
            {
                var jpg_parser = new ZPI_Projekt_Anonimizator.Parsers.JPGParser();
                
                var table = jpg_parser.parseDocument(path);
                if (table == null || path == "") promptUser("An error ocurred, unable to open the jpg file." + path);
                else
                {
                    var values = table.Rows[0].ItemArray;
                    TextLine0.Text = table.Columns[0].ColumnName + ": " + values[0];
                    TextLine1.Text = table.Columns[1].ColumnName + ": " + values[1];
                    TextLine2.Text = table.Columns[2].ColumnName + ": " + values[2];
                    TextLine3.Text = table.Columns[3].ColumnName + ": " + values[3];
                    TextLine4.Text = table.Columns[4].ColumnName + ": " + values[4];
                    TextLine5.Text = "PATH: " + path;
                    MetadataDocumentView.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                promptUser("An error ocurred, unable to open the jpg file." + path);
            }
        }
        public void showDICOMMetadata(String path)
        {
            try
            {
                var dicom_parser = new ZPI_Projekt_Anonimizator.Parsers.DICOMParser();
                var table = dicom_parser.parseDocument(path);
                if (table == null) promptUser("An error ocurred, unable to open the dicom file." + path);
                else
                {
                    var values = table.Rows[0].ItemArray;
                    TextLine0.Text = table.Columns[0].ColumnName + ": " + values[0];
                    TextLine1.Text = table.Columns[1].ColumnName + ": " + values[1];
                    TextLine2.Text = table.Columns[2].ColumnName + ": " + values[2];
                    TextLine3.Text = table.Columns[3].ColumnName + ": " + values[3];
                    TextLine5.Text = "PATH: " + path;
                    MetadataDocumentView.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                promptUser("An error ocurred, unable to open the dicom file.");
            }
        }
        public void showDOCXData(String path)
        {
            try
            {
                var docx_parser = new ZPI_Projekt_Anonimizator.Parsers.DOCXParser();
                var table = docx_parser.parseDocument(path);
                if (table == null) promptUser("An error ocurred, unable to open the docx file." + path);
                else
                {
                    var values = table.Rows[0].ItemArray;
                    TextLine0.Text = "Patient: " + table.Columns[0].ColumnName + " " + values[0] + ", " + values[1] + " " + values[2]; ;
                    TextLine1.Text = table.Columns[3].ColumnName + ", " + table.Columns[4].ColumnName + ": " + values[3] + ", " + values[4];
                    TextLine2.Text = table.Columns[5].ColumnName + ": " + values[5];
                    TextLine3.Text = table.Columns[6].ColumnName + ": " + values[6];
                    TextLine4.Text = table.Columns[7].ColumnName + ": " + values[7];
                    TextLine5.Text = table.Columns[8].ColumnName + ": " + values[8] + "\n\nPATH: " + path;
                    MetadataDocumentView.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                promptUser("An error ocurred, unable to open the docx file.");
            }
        }

        private void MetadataOpen_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                DataRowView context = (DataRowView)button.DataContext;
                var row = context.Row;
                var links = row["PathForFiles"].ToString().Split(";");
                switch (button.Name)
                {
                    case "BtnJPG":
                    case "BtnJPG2": showJPGMetadata(links[0]); break;
                    case "BtnDICOM":
                    case "BtnDICOM2": showDICOMMetadata(links[1]); break;
                    case "BtnDOCX":
                    case "BtnDOCX2": showDOCXData(links[2]); break;
                    default: promptUser("An error ocurred, no file can be opened."); break;
                }
            }
            catch (Exception ex)
            {
                promptUser("An error ocured while trying to open the file.");
            }
        }
        private void MetadataClose_Button_Click(object sender, RoutedEventArgs e)
        {
            TextLine0.Text = "";
            TextLine1.Text = "";
            TextLine2.Text = "";
            TextLine3.Text = "";
            TextLine4.Text = "";
            TextLine5.Text = "";
            MetadataDocumentView.Visibility = Visibility.Hidden;
        }

        private void promptUser(String msg)
        {
            InfoBoxPrompt.Text = msg;
            ((Storyboard)FindResource("animate")).Begin(Prompt);
        }
    }
}
