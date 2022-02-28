using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.IO;
using System.Text.RegularExpressions;
namespace WpfApp1
{
    /// <summary>
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            fill_box();
        }
        private void fill_box() {
            string path = @"konten.csv";
            List<string> list = new List<string>();
            List<String> lines = new List<String>();
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {

                    String line = reader.ReadLine();
                    if (line != null)
                    {
                        string[] parts = line.Split("; ");
                        for (int i = 0; i < parts.Length; i++)
                        {
                            list.Add(parts[i]);
                        }
                    }
                }
            }
            for(int i=0; i<list.Count; i++)
            {
                Haben.Items.Add(list[i]);
                Soll.Items.Add(list[i]);
            }
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            //koppelung Konten muss vollzogen werden...
            string path = @"journal.csv";
            string datum = Datum.Text;
            string buchung = Buchung.Text;
            string soll = Soll.Text;
            string soll_datei = @soll + ".csv";
            string haben = Haben.Text;
            string haben_datei = @haben + ".csv";
            string beschreibung = Beschreibung.Text;
            string betrag = Betrag.Text;
            if(datum=="" || buchung=="" ||soll == ""||haben == ""||betrag == "")
            {
                //alternative zu datum==""||buchung=="" ist (datum==""&&buchung=="") wenn nur eines der Beiden ausgefüllt ein muss anstatt beide
                MessageBox.Show("Bitte füllen Sie Datum/Betrag, Soll, Haben und Betrag aus!!", "Bitte alles ausfüllen.");
            }
            else
            {
                List<String> lines = new List<String>();
                if (File.Exists(path))
                {
                    using (StreamReader reader = new StreamReader(path))
                    {
                        String line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            lines.Add(line);
                        }
                    }
                }
                if (!File.Exists(path)) 
                { 
                    FileInfo FI = new FileInfo(path);
                    FileStream FS = FI.Create();
                    FS.Close();
                }
                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    lines.Add(datum + ";" + buchung + ";" + soll + ";" + haben + ";" + beschreibung + ";" + betrag + ";");
                    foreach (String line in lines)
                        writer.WriteLine(line);
                }
                if(File.Exists(soll_datei)&&File.Exists(haben_datei))
                {
                    //Soll
                    List<String> soll_lines = new List<String>(); 
                    using (StreamReader soll_reader = new StreamReader(soll_datei))
                    {
                        String soll_line;
                        while ((soll_line = soll_reader.ReadLine()) != null)
                        {
                            soll_lines.Add(soll_line);
                        }
                    }
                    using (StreamWriter soll_writer = new StreamWriter(soll_datei, false))
                    {
                        soll_lines.Add(datum + ";" + buchung + ";" + betrag + ";");
                        foreach (String soll_line in soll_lines)
                            soll_writer.WriteLine(soll_line);
                    }

                    //Haben
                    List<String> haben_lines = new List<String>();
                    using (StreamReader haben_reader = new StreamReader(haben_datei))
                    {
                        String haben_line;
                        while ((haben_line = haben_reader.ReadLine()) != null)
                        {
                            haben_lines.Add(haben_line);
                        }
                    }
                    using (StreamWriter haben_writer = new StreamWriter(haben_datei, false))
                    {
                        haben_lines.Add(datum + ";" + buchung + ";"+" ;" + betrag + ";");
                        foreach (String haben_line in haben_lines)
                            haben_writer.WriteLine(haben_line);
                    }
                }
                else 
                {
                    MessageBox.Show("Error: Es geb einen Fehler mit den Konten"+soll_datei+"&"+haben_datei+"!");
                }
                
            }
            Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
