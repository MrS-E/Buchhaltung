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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Globalization;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class MyItem
    {
        public string Datum { get; set; }
        public string Buchungsnummer { get; set; }
        public string Soll { get; set; }
        public string Haben { get; set; }
        public string Beschreibung { get; set; }
        public string Betrag { get; set; }
        public string Aktiv { get; set; }
        public double AktivBetrag { get; set; }

        public string Passiv { get; set; }
        public double PassivBetrag { get; set; }
        public string Aufwand { get; set; }
        public double AufwandBetrag { get; set; }
        public string Ertrag { get; set; }
        public double ErtragBetrag { get; set; }
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window1 neuBuchung = new Window1();
            neuBuchung.Show();
        }
        private void KontoNeu_Click(object sender, RoutedEventArgs e)
        {
            KontoNeu Konto = new KontoNeu();
            Konto.Show();
        }
        private void ExportPDF_Click(object sender, RoutedEventArgs e)
        {
            //vieleicht nur txt
            Export();
        }
        private void NeueRechnungsberiode_Click(object sender, RoutedEventArgs e)
        {
            //Schlussbilanz 1
            Schlussbilanz1();
            //Erfolgsrechnung
            Erfolg_berechnung();
            //Schlussbilanz 2
            Schlussbilanz2();
            //export pdf
            Export();
            //Eröffnungsbilanz ist SB2 
            string schlussbilanz2 = @"sb2.csv";
            string eröffnungsbilanz = @"eroeffnung.csv";
            if (File.Exists(eröffnungsbilanz))
            { 
                File.Delete(eröffnungsbilanz);
            }
            System.IO.File.Move(schlussbilanz2, eröffnungsbilanz);
            //löschung aller dateien -> txt-datei als Inhaltsverzeichnis
            string[] filePathsTXT = Directory.GetFiles(Environment.CurrentDirectory, "*.txt", SearchOption.TopDirectoryOnly);
            foreach (string path in filePathsTXT)
            {
                File.Delete(path);
            }
            string sb2 = @"sb2.csv";
            string journal = @"journal.csv";
            File.Delete(sb2);
            File.Delete(journal);
            //Konten (Aktiv&Pasiv) mit start summe versehen
            List<string> lines = new List<string>();
            using(StreamReader reader = new StreamReader(eröffnungsbilanz))
            {
                String line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
            foreach(string line in lines)
            {
                string[] konten_beträge = line.Split(';');
                string konto1 = @konten_beträge[0] + ".csv";
                string betrag1 = @konten_beträge[1];
                string konto2 = @konten_beträge[2] + ".csv";
                string betrag2 = @konten_beträge[3];
                if(konto1 == "gewinn.csv.csv") 
                {
                    using (StreamWriter writer = new StreamWriter(@"gewinn.csv"))
                    {
                        string input = DateTime.Now.ToString("dd-MM-yyyy") + ";" + "Eröffnung" + ";" + betrag1 + ";";
                        writer.WriteLine(input);
                    }
                }
                if (konto1 != "----.csv")
                {
                    using (StreamWriter writer = new StreamWriter(konto1))
                    {
                        string input = DateTime.Now.ToString("dd-MM-yyyy") + ";" + "Eröffnung" + ";" + betrag1 + ";";
                        writer.WriteLine(input);
                    }
                }
                if (konto2 == "gewinn.csv.csv")
                {
                    using (StreamWriter writer = new StreamWriter(@"gewinn.csv"))
                    {
                        string input = DateTime.Now.ToString("dd-MM-yyyy") + ";" + "Eröffnung" + ";" + ";" + betrag2;
                        writer.WriteLine(input);
                    }
                }
                if (konto2 != "----.csv")
                {
                    using (StreamWriter writer = new StreamWriter(konto2))
                    {
                        string input = DateTime.Now.ToString("dd-MM-yyyy") + ";" + "Eröffnung" + ";" + ";" + betrag2;
                        writer.WriteLine(input);
                    }
                }
            }
            //setzt konto (Ertrag&Aufwand) auf null 
            string konten_liste = @"konten.csv";
            using (StreamReader reader = new StreamReader(konten_liste))
            {
                string line = reader.ReadLine();
                string line2 = reader.ReadLine();
                if (line != null)
                {
                    string[] parts = line.Split("; ");
                    string[] parts2 = line2.Split("; ");
                    for (int i = 0; i < parts.Length; i++)
                    {
                        if (parts2[i] == "Aktiv")
                        {}
                        else if (parts2[i] == "Passiv")
                        {}
                        else if (parts2[i] == "Ertrag")
                        {
                            File.Delete(parts[i]+".csv");
                            File.Create(parts[i] +".csv");
                        }
                        else if (parts2[i] == "Aufwand")
                        {
                            File.Delete(parts[i] + ".csv");
                            File.Create(parts[i] + ".csv");
                        }
                    }
                }

            }
            MessageBox.Show("Beendet");
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Aktualisieren bei Journal
            string path = @"journal.csv";
            List<String> datum = new List<String>();
            List<String> buchung = new List<String>();
            List<String> soll_id = new List<String>();
            List<String> haben = new List<String>();
            List<String> beschreibung = new List<String>();
            List<String> betrag = new List<String>();
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    String line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        String[] parts = line.Split(';');
                        datum.Add(parts[0]);
                        buchung.Add(parts[1]);
                        soll_id.Add(parts[2]);
                        haben.Add(parts[3]);
                        beschreibung.Add(parts[4]);
                        betrag.Add(parts[5]);
                    }
                }

            }
            JournalListView.Items.Clear();
            for (int i = 0; i < datum.Count; i++)
            {
                JournalListView.Items.Add(new MyItem { Datum = datum[i], Buchungsnummer = buchung[i], Soll = soll_id[i], Haben = haben[i], Beschreibung = beschreibung[i], Betrag = betrag[i] });
            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Aktualisierung der Erfolgsrechnung
            Erfolg_berechnung();
        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            //eröffnungsbilanz
            string path = @"eroeffnung.csv";
            try
            {
                List<string> lines = new List<string>();
                using(StreamReader reader = new StreamReader(path))
                {
                    String line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }
                BilanzListView.Items.Clear();
                foreach(string line in lines)
                {
                    string[] parts = line.Split(";");
                    string konto1 = parts[0];
                    string betrag1 = parts[1];
                    string konto2 = parts[2];
                    string betrag2 = parts[3];
                    BilanzListView.Items.Add(new MyItem { Aktiv = konto1, AktivBetrag = Convert.ToDouble(betrag1),Passiv=konto2, PassivBetrag = Convert.ToDouble(betrag2) });
                }    
            }
            catch
            {
                MessageBox.Show("Die Erfolgsrechnung konnte nicht geladen werden. Überprüfen Sie ob es eine Erfolgsrechnung gibt oder beginnen Sie einen neue Rechnungsperiode.");
            }
        }
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Schlussbilanz1();
        }
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            Schlussbilanz2();
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //Aktualisierung der Konten
            //Aktiv.Items.Add("Kasse");
            string path = @"konten.csv";
            Aktiv.Items.Clear();
            Passiv.Items.Clear();
            Ertrag.Items.Clear();
            Aufwand.Items.Clear();
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string line = reader.ReadLine();
                    string line2 = reader.ReadLine();
                    if (line != null)
                    {
                        string[] parts = line.Split("; ");
                        string[] parts2 = line2.Split("; ");
                        for (int i = 0; i < parts.Length; i++)
                        {
                            if (parts2[i] == "Aktiv")
                            {
                                Aktiv.Items.Add(parts[i]);
                            }
                            else if (parts2[i] == "Passiv")
                            {
                                Passiv.Items.Add(parts[i]);
                            }
                            else if (parts2[i] == "Ertrag")
                            {
                                Ertrag.Items.Add(parts[i]);
                            }
                            else if (parts2[i] == "Aufwand")
                            {
                                Aufwand.Items.Add(parts[i]);
                            }
                        }
                    }

                }
            }
        }
        private void SelectionChanged(object sender, RoutedPropertyChangedEventArgs<Object> e)
        {
            //Perform actions when SelectedItem changes
            //TreeViewItem selectedTVI = (TreeViewItem)myTreeView.SelectedItem;
            try
            {
                string value = (string)KontoListe.SelectedItem;
                Konto_Printout(value);
            }
            catch
            {
                MessageBox.Show("Bitte nur Konten auswählen");
            }
        }
        public void Konto_Printout(string name)
        {
            string path = @name + ".csv";
            double soll_summe = 0;
            double haben_summe = 0;
            double saldo = 0;
            bool soll_saldo;
            List<String> lines = new List<String>();
            List<String> datum = new List<String>();
            List<String> nummer = new List<String>();
            List<String> soll = new List<String>();
            List<String> haben = new List<String>();
            List<String> result = new List<String>();
            //reading
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    String line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line);
                        string[] line_splitt = line.Split(';');
                        datum.Add(line_splitt[0]);
                        nummer.Add(line_splitt[1]);
                        soll.Add(line_splitt[2]);
                        haben.Add(line_splitt[3]);
                    }
                }
            }
            else
            {
                MessageBox.Show("Error: das Konto \"" + name + "\" konnte nicht gefunden werden.");
            }
            //converting and addition
            foreach (string line in soll)
            {
                try
                {
                    soll_summe += Convert.ToDouble(line);
                }
                catch
                { }
            }
            foreach (string line in haben)
            {
                try
                {
                    haben_summe += Convert.ToDouble(line);
                }
                catch
                { }
            }
            //saldo calculation
            if (haben_summe < soll_summe)
            {
                saldo = soll_summe - haben_summe;
                soll_saldo = true;
            }
            else
            {
                saldo = haben_summe - soll_summe;
                soll_saldo = false;
            }
            //creating messagbox output
            result.Add("Datum\t\t| Buchungsnummer \t| Soll \t\t| Haben");
            for (int i = 0; i < lines.Count; i++)
            {
                result.Add(datum[0] + "\t| " + nummer[i] + "\t\t| " + soll[i] + "\t\t| " + haben[i]);
            }

            string specifier = "G";
            CultureInfo culture = CultureInfo.CreateSpecificCulture("eu-ES");
            result.Add("Summen\t\t| ----\t\t| " + soll_summe.ToString(specifier, CultureInfo.InvariantCulture) + "\t\t| " + haben_summe.ToString(specifier, CultureInfo.InvariantCulture));
            if (soll_saldo)
            {
                result.Add("Saldo\t\t| ----\t\t| \t\t| " + saldo.ToString(specifier, CultureInfo.InvariantCulture));
            }
            else
            {
                result.Add("Saldo\t\t| ----\t\t| " + saldo.ToString(specifier, CultureInfo.InvariantCulture) + "\t\t| ");
            }
            string output = String.Join("\n", result.ToArray());
            MessageBox.Show(output, name);
        }
        public void Erfolg_berechnung()
        {
            string path = @"konten.csv";
            string gewinn_data = @"gewinn.csv";
            string date = DateTime.UtcNow.ToString("d");
            string[] attribute;
            string[] konten_namen;
            List<string> aufwand_konto = new List<string>();
            List<double> aufwand_summen = new List<double>();
            List<string> ertrag_konto = new List<string>();
            List<double> ertrag_summen = new List<double>();
            double aufwand = 0;
            double ertrag = 0;
            double erfolg;
            bool gewinn;
            //searching for konten
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    //achtung 0 ist nicht zu vewenden
                    konten_namen = reader.ReadLine().Split("; ");
                    attribute = reader.ReadLine().Split(";");
                }
                for (int i = 0; i < attribute.Length; i++)
                {
                    if (attribute[i] == " Ertrag")
                    {
                        ertrag_konto.Add(konten_namen[i]);
                    }
                    else if (attribute[i] == " Aufwand")
                    {
                        aufwand_konto.Add(konten_namen[i]);
                    }
                }
            }
            //extract data from the erfolgs-konten
            for (int i = 0; i < ertrag_konto.Count; i++)
            {
                string name = @ertrag_konto[i] + ".csv";

                if (File.Exists(name))
                {
                    List<String> soll = new List<String>();
                    List<String> haben = new List<String>();
                    double soll_summe = 0;
                    double haben_summe = 0;
                    using (StreamReader reader = new StreamReader(name))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] line_splitt = line.Split(';');
                            soll.Add(line_splitt[2]);
                            haben.Add(line_splitt[3]);
                        }
                    }
                    for (int x = 0; x < soll.Count; x++)
                    {
                        try
                        {
                            soll_summe += Convert.ToDouble(soll[x]);
                        }
                        catch
                        { }
                    }
                    for (int x = 0; x < haben.Count; x++)
                    {
                        try
                        {
                            haben_summe += Convert.ToDouble(haben[x]);
                        }
                        catch
                        { }
                    }
                    ertrag_summen.Add(haben_summe - soll_summe);
                }
            }
            //extract data from the aufwand-konten
            for (int i = 0; i < aufwand_konto.Count; i++)
            {
                string name = @aufwand_konto[i] + ".csv";

                if (File.Exists(name))
                {
                    List<String> soll = new List<String>();
                    List<String> haben = new List<String>();
                    double soll_summe = 0;
                    double haben_summe = 0;
                    using (StreamReader reader = new StreamReader(name))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] line_splitt = line.Split(';');
                            soll.Add(line_splitt[2]);
                            haben.Add(line_splitt[3]);
                        }
                    }
                    for (int x = 0; x < soll.Count; x++)
                    {
                        try
                        {
                            soll_summe += Convert.ToDouble(soll[x]);
                        }
                        catch
                        { }
                    }
                    for (int x = 0; x < haben.Count; x++)
                    {
                        try
                        {
                            haben_summe += Convert.ToDouble(haben[x]);
                        }
                        catch
                        { }
                    }
                    aufwand_summen.Add(soll_summe - haben_summe);
                }
            }
            //calculate the Erfolg and decide if it is Gewinn or Verlust
            for (int i = 0; i < aufwand_summen.Count; i++)
            {
                aufwand += aufwand_summen[i];
            }
            for (int i = 0; i < ertrag_summen.Count; i++)
            {
                ertrag += ertrag_summen[i];
            }
            if (aufwand < ertrag)
            {
                erfolg = ertrag - aufwand;
                gewinn = true;
            }
            else
            {
                erfolg = aufwand - ertrag;
                gewinn = false;
            }
            //print out
            ErfolgsListView.Items.Clear();
            for (int i = 0; (i < aufwand_konto.Count) || (i < ertrag_konto.Count); i++)
            {
                if (i < aufwand_konto.Count && i < ertrag_konto.Count)
                {
                    ErfolgsListView.Items.Add(new MyItem { Aufwand = aufwand_konto[i], AufwandBetrag = aufwand_summen[i], Ertrag = ertrag_konto[i], ErtragBetrag = ertrag_summen[i] });
                }
                else if (i < aufwand_konto.Count)
                {
                    ErfolgsListView.Items.Add(new MyItem { Aufwand = aufwand_konto[i], AufwandBetrag = aufwand_summen[i], Ertrag = "----", ErtragBetrag = 0.0 });
                }
                else if (i < ertrag_konto.Count)
                {
                    ErfolgsListView.Items.Add(new MyItem { Aufwand = "----", AufwandBetrag = 0.0, Ertrag = ertrag_konto[i], ErtragBetrag = ertrag_summen[i] });
                }
                else
                {
                    ErfolgsListView.Items.Add(new MyItem { Aufwand = "----", AufwandBetrag = 0.0, Ertrag = "----", ErtragBetrag = 0.0 });
                }
            }
            ErfolgsListView.Items.Add(new MyItem { Aufwand = "Aufwand Summe", AufwandBetrag = aufwand, Ertrag = "Ertrag Summe", ErtragBetrag = ertrag });
            if (gewinn == true)
            {
                ErfolgsListView.Items.Add(new MyItem { Aufwand = "Saldo", AufwandBetrag = erfolg, Ertrag = "----", ErtragBetrag = 0.0 });
            }
            else
            {
                ErfolgsListView.Items.Add(new MyItem { Aufwand = "----", AufwandBetrag = 0.0, Ertrag = "Saldo", ErtragBetrag = erfolg });
            }
            //write geinn to file
            if (!File.Exists(gewinn_data))
            {
                addKonto("gewinn", "");
            }
            using (StreamWriter writer = new StreamWriter(gewinn_data, false))
            {
                if(gewinn == true)
                {
                    //gewinn auf haben
                    writer.WriteLine(date + ";" + ";" + ";" + erfolg);
                }
                else
                {
                    //gewinn auf soll
                    writer.WriteLine(date + ";" + ";" + erfolg + ";");
                }
            }
            //write to txt file
            string er = @"erfolgsrechnung.txt";
            if (!File.Exists(er))
            {
                FileInfo FI = new FileInfo(er);
                FileStream FS = FI.Create();
                FS.Close();
            }
            using (StreamWriter writer = new StreamWriter(er, false))
            {
                writer.WriteLine("Erfolgsrechnung:");
                writer.WriteLine("Aufwand\t\tBetrag\t\t|Ertrag\t\tBetrag");
                writer.WriteLine("--------------------------------|-----------------------------");

                for (int i = 0; (i < aufwand_konto.Count) || (i < ertrag_konto.Count); i++)
                {
                    if (i < aufwand_konto.Count && i < ertrag_konto.Count)
                    {
                        writer.WriteLine(aufwand_konto[i] + "\t\t" + aufwand_summen[i] + "\t|" + ertrag_konto[i] + "\t\t" + ertrag_summen[i]);
                    }
                    else if (i < aufwand_konto.Count)
                    {
                        writer.WriteLine(aufwand_konto[i] + "\t\t" + aufwand_summen[i] + "\t|----\t\t\t----");
                    }
                    else if (i < ertrag_konto.Count)
                    {
                        writer.WriteLine("----\t\t\t----\t|" + ertrag_konto[i] + "\t\t" + ertrag_summen[i]);
                    }
                    else
                    {
                        writer.WriteLine("----\t\t\t----\t|----\t\t\t----");
                    }
                }
                writer.WriteLine("Aufwand Summe\t\t" + aufwand + "\t|Ertrag Summe\t\t" + ertrag);
                if (gewinn == true)
                {
                    writer.WriteLine("Saldo\t\t\t" + erfolg + "\t|----\t\t\t----");
                }
                else
                {
                    writer.WriteLine("----\t\t\t----\t|Saldo\t\t\t" + erfolg);
                }
            }
        }
        public void Schlussbilanz1()
        {
            string path = @"konten.csv";
            string[] attribute;
            string[] konten_namen;
            List<string> aktiv_konto = new List<string>();
            List<double> aktiv_summen = new List<double>();
            List<string> passiv_konto = new List<string>();
            List<double> passiv_summen = new List<double>();
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    //achtung 0 ist nicht zu vewenden
                    konten_namen = reader.ReadLine().Split("; ");
                    attribute = reader.ReadLine().Split(";");
                }
                for (int i = 0; i < attribute.Length; i++)
                {
                    if (attribute[i] == " Aktiv")
                    {
                        aktiv_konto.Add(konten_namen[i]);
                    }
                    else if (attribute[i] == " Passiv")
                    {
                        passiv_konto.Add(konten_namen[i]);
                    }
                }
            }
            //date extraction
            //aktiv
            for (int i = 0; i < aktiv_konto.Count; i++)
            {
                string name = @aktiv_konto[i] + ".csv";

                if (File.Exists(name))
                {
                    List<String> soll = new List<String>();
                    List<String> haben = new List<String>();
                    double soll_summe = 0;
                    double haben_summe = 0;
                    using (StreamReader reader = new StreamReader(name))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] line_splitt = line.Split(';');
                            soll.Add(line_splitt[2]);
                            haben.Add(line_splitt[3]);
                        }
                    }
                    for (int x = 0; x < soll.Count; x++)
                    {
                        try
                        {
                            soll_summe += Convert.ToDouble(soll[x]);
                        }
                        catch
                        { }
                    }
                    for (int x = 0; x < haben.Count; x++)
                    {
                        try
                        {
                            haben_summe += Convert.ToDouble(haben[x]);
                        }
                        catch
                        { }
                    }
                    aktiv_summen.Add(soll_summe - haben_summe);
                }
            }
            //passiv
            for (int i = 0; i < passiv_konto.Count; i++)
            {
                string name = @passiv_konto[i] + ".csv";

                if (File.Exists(name))
                {
                    List<String> soll = new List<String>();
                    List<String> haben = new List<String>();
                    double soll_summe = 0;
                    double haben_summe = 0;
                    using (StreamReader reader = new StreamReader(name))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] line_splitt = line.Split(';');
                            soll.Add(line_splitt[2]);
                            haben.Add(line_splitt[3]);
                        }
                    }
                    for (int x = 0; x < soll.Count; x++)
                    {
                        try
                        {
                            soll_summe += Convert.ToDouble(soll[x]);
                        }
                        catch
                        { }
                    }
                    for (int x = 0; x < haben.Count; x++)
                    {
                        try
                        {
                            haben_summe += Convert.ToDouble(haben[x]);
                        }
                        catch
                        { }
                    }
                    passiv_summen.Add(soll_summe - haben_summe);
                }
            }
            //write to list view
            BilanzListView.Items.Clear();
            for (int i = 0; (i < aktiv_konto.Count) || (i < passiv_konto.Count); i++)
            {
                if (i < aktiv_konto.Count && i < passiv_konto.Count)
                {
                    BilanzListView.Items.Add(new MyItem { Aktiv = aktiv_konto[i], AktivBetrag = aktiv_summen[i], Passiv = passiv_konto[i], PassivBetrag = passiv_summen[i] });
                }
                else if (i < aktiv_konto.Count)
                {
                    BilanzListView.Items.Add(new MyItem { Aktiv = aktiv_konto[i], AktivBetrag = aktiv_summen[i], Passiv = "----", PassivBetrag = 0.0 });
                }
                else if (i < passiv_konto.Count)
                {
                    BilanzListView.Items.Add(new MyItem { Aktiv = "----", AktivBetrag = 0.0, Passiv = passiv_konto[i], PassivBetrag = passiv_summen[i] });
                }
                else
                {
                    BilanzListView.Items.Add(new MyItem { Aktiv = "----", AktivBetrag = 0.0, Passiv = "----", PassivBetrag = 0.0 });
                }
            }
            //write to txt file
            string sb1 = @"schlussbilanz1.txt";
            if (!File.Exists(sb1))
            {
                FileInfo FI = new FileInfo(sb1);
                FileStream FS = FI.Create();
                FS.Close();
            }
            using (StreamWriter writer = new StreamWriter(sb1, false))
            {
                writer.WriteLine("Schlussbilanz1:");
                writer.WriteLine("Aktiv\t\tBetrag\t\t|Passiv\t\t\tBetrag");
                writer.WriteLine("--------------------------------|-----------------------------");
                for (int i = 0; (i < aktiv_konto.Count) || (i < passiv_konto.Count); i++)
                {
                    if (i < aktiv_konto.Count && i < passiv_konto.Count)
                    {
                        writer.WriteLine(aktiv_konto[i] + "\t\t"+ aktiv_summen[i]+"\t\t|"+ passiv_konto[i]+"\t\t"+ passiv_summen[i]);
                    }
                    else if (i < aktiv_konto.Count)
                    {
                        writer.WriteLine(aktiv_konto[i] + "\t\t" + aktiv_summen[i] + "\t\t|----\t\t\t----");
                    }
                    else if (i < passiv_konto.Count)
                    {
                        writer.WriteLine("----\t\t\t----\t\t|" + passiv_konto[i] + "\t\t" + passiv_summen[i]);
                    }
                    else
                    {
                        writer.WriteLine("----\t\t\t----\t\t|----\t\t\t----");

                    }
                }
            }
        }
        public void addKonto(string name, string katigorie)
        {
            string path = @"konten.csv";
            bool enthalten = false;
            if (!File.Exists(path))
            {
                FileInfo FI = new FileInfo(path);
                FileStream FS = FI.Create();
                FS.Close();
            }
            else
            {
                List<String> lines = new List<String>();
                if (File.Exists(path))
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        string linecheck1 = sr.ReadLine();
                        if (linecheck1 != null)
                        {
                            string[] linecheck2 = linecheck1.Split("; ");
                            string compare;
                            for (int i = 0; i < linecheck2.Length; i++)
                            {
                                compare = linecheck2[i];
                                if (compare == name)
                                {
                                    enthalten = true;
                                    break;
                                }
                                else
                                {
                                    enthalten = false;
                                }
                            }
                        }
                    }

                    if (enthalten == false)
                    {
                        using (StreamReader reader = new StreamReader(path))
                        {
                            string line1 = reader.ReadLine() + "; " + name;
                            string line2 = reader.ReadLine() + "; " + katigorie;
                            lines.Add(line1);
                            lines.Add(line2);
                        }

                        FileInfo Konto = new FileInfo(name + ".csv");
                        FileStream Konto_FS = Konto.Create();
                        Konto_FS.Close();

                        using (StreamWriter writer = new StreamWriter(path, false))
                        {
                            foreach (String line in lines)
                                writer.WriteLine(line);
                        }
                    }
                }
            }
        }
        public void Schlussbilanz2()
        {
            string path = @"konten.csv";
            string gewinn = @"gewinn.csv";
            string[] attribute;
            string[] konten_namen;
            List<string> aktiv_konto = new List<string>();
            List<double> aktiv_summen = new List<double>();
            List<string> passiv_konto = new List<string>();
            List<double> passiv_summen = new List<double>();
            //string sb2 = @"schlussbilanz2.csv";
            //List<string> sb2_contence = new List<string>();
            //if (!File.Exists(sb2))
            //{
            //    FileInfo FI = new FileInfo(sb1);
            //    FileStream FS = FI.Create();
            //    FS.Close();
            //}
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    //achtung 0 ist nicht zu vewenden
                    konten_namen = reader.ReadLine().Split("; ");
                    attribute = reader.ReadLine().Split(";");
                }
                for (int i = 0; i < attribute.Length; i++)
                {
                    if (attribute[i] == " Aktiv")
                    {
                        aktiv_konto.Add(konten_namen[i]);
                    }
                    else if (attribute[i] == " Passiv")
                    {
                        passiv_konto.Add(konten_namen[i]);
                    }
                }
            }
            //date extraction
            //aktiv
            for (int i = 0; i < aktiv_konto.Count; i++)
            {
                string name = @aktiv_konto[i] + ".csv";

                if (File.Exists(name))
                {
                    List<String> soll = new List<String>();
                    List<String> haben = new List<String>();
                    double soll_summe = 0;
                    double haben_summe = 0;
                    using (StreamReader reader = new StreamReader(name))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] line_splitt = line.Split(';');
                            soll.Add(line_splitt[2]);
                            haben.Add(line_splitt[3]);
                        }
                    }
                    for (int x = 0; x < soll.Count; x++)
                    {
                        try
                        {
                            soll_summe += Convert.ToDouble(soll[x]);
                        }
                        catch
                        { }
                    }
                    for (int x = 0; x < haben.Count; x++)
                    {
                        try
                        {
                            haben_summe += Convert.ToDouble(haben[x]);
                        }
                        catch
                        { }
                    }
                    aktiv_summen.Add(soll_summe - haben_summe);
                }
            }
            //passiv
            for (int i = 0; i < passiv_konto.Count; i++)
            {
                string name = @passiv_konto[i] + ".csv";

                if (File.Exists(name))
                {
                    List<String> soll = new List<String>();
                    List<String> haben = new List<String>();
                    double soll_summe = 0;
                    double haben_summe = 0;
                    using (StreamReader reader = new StreamReader(name))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] line_splitt = line.Split(';');
                            soll.Add(line_splitt[2]);
                            haben.Add(line_splitt[3]);
                        }
                    }
                    for (int x = 0; x < soll.Count; x++)
                    {
                        try
                        {
                            soll_summe += Convert.ToDouble(soll[x]);
                        }
                        catch
                        { }
                    }
                    for (int x = 0; x < haben.Count; x++)
                    {
                        try
                        {
                            haben_summe += Convert.ToDouble(haben[x]);
                        }
                        catch
                        { }
                    }
                    passiv_summen.Add(soll_summe - haben_summe);
                }
            }
            //search for file gewinn
            Erfolg_berechnung();
            using (StreamReader reader = new StreamReader(gewinn))
            {
                string line = reader.ReadLine();
                string[] line_splitt = line.Split(';');
                if (line_splitt[2] == "")
                {
                    //inhalt in haben -> passiv
                    passiv_konto.Add("gewinn");
                    passiv_summen.Add(Convert.ToDouble(line_splitt[3]));
                }
                else
                {
                    double betrag = Convert.ToDouble(line_splitt[2]);
                    betrag = betrag - (betrag * 2);
                    passiv_konto.Add(gewinn);
                    passiv_summen.Add(betrag);
                }
                
            }
            //write to list view
            BilanzListView.Items.Clear();
            for (int i = 0; (i < aktiv_konto.Count) || (i < passiv_konto.Count); i++)
            {
                if (i < aktiv_konto.Count && i < passiv_konto.Count)
                {
                    BilanzListView.Items.Add(new MyItem { Aktiv = aktiv_konto[i], AktivBetrag = aktiv_summen[i], Passiv = passiv_konto[i], PassivBetrag = passiv_summen[i] });
                }
                else if (i < aktiv_konto.Count)
                {
                    BilanzListView.Items.Add(new MyItem { Aktiv = aktiv_konto[i], AktivBetrag = aktiv_summen[i], Passiv = "----", PassivBetrag = 0.0 });
                }
                else if (i < passiv_konto.Count)
                {
                    BilanzListView.Items.Add(new MyItem { Aktiv = "----", AktivBetrag = 0.0, Passiv = passiv_konto[i], PassivBetrag = passiv_summen[i] });
                }
                else
                {
                    BilanzListView.Items.Add(new MyItem { Aktiv = "----", AktivBetrag = 0.0, Passiv = "----", PassivBetrag = 0.0 });
                }
            }
            //write to txt file
            string sb2 = @"schlussbilanz2.txt";
            if (!File.Exists(sb2))
            {
                FileInfo FI = new FileInfo(sb2);
                FileStream FS = FI.Create();
                FS.Close();
            }
            using (StreamWriter writer = new StreamWriter(sb2, false))
            {
                writer.WriteLine("Schlussbilanz2:");
                writer.WriteLine("Aktiv\t\tBetrag\t\t|Passiv\t\t\tBetrag");
                writer.WriteLine("--------------------------------|-----------------------------");
                for (int i = 0; (i < aktiv_konto.Count) || (i < passiv_konto.Count); i++)
                {
                    if (i < aktiv_konto.Count && i < passiv_konto.Count)
                    {
                        writer.WriteLine(aktiv_konto[i] + "\t\t" + aktiv_summen[i] + "\t\t|" + passiv_konto[i] + "\t\t" + passiv_summen[i]);
                    }
                    else if (i < aktiv_konto.Count)
                    {
                        writer.WriteLine(aktiv_konto[i] + "\t\t" + aktiv_summen[i] + "\t\t|----\t\t\t----");
                    }
                    else if (i < passiv_konto.Count)
                    {
                        writer.WriteLine("----\t\t\t----\t\t|" + passiv_konto[i] + "\t\t" + passiv_summen[i]);
                    }
                    else
                    {
                        writer.WriteLine("----\t\t\t----\t\t|----\t\t\t----");

                    }
                }
            }
            //write to csv file
            string sb = @"sb2.csv";
            if (!File.Exists(sb))
            {
                FileInfo FI = new FileInfo(sb);
                FileStream FS = FI.Create();
                FS.Close();
            }
            using (StreamWriter writer = new StreamWriter(sb, false))
            {
                for (int i = 0; (i < aktiv_konto.Count) || (i < passiv_konto.Count); i++)
                {
                    if (i < aktiv_konto.Count && i < passiv_konto.Count)
                    {
                        writer.WriteLine(aktiv_konto[i] + ";" + aktiv_summen[i] + ";" + passiv_konto[i] + ";" + passiv_summen[i]);
                    }
                    else if (i < aktiv_konto.Count)
                    {
                        writer.WriteLine(aktiv_konto[i] + ";" + aktiv_summen[i] + ";----;0.0");
                    }
                    else if (i < passiv_konto.Count)
                    {
                        writer.WriteLine("----;0.0;" + passiv_konto[i] + ";" + passiv_summen[i]);
                    }
                    else
                    {
                        writer.WriteLine("----;0.0;----;0.0");

                    }
                }
            }
        }
        public void Export()
        {
            string data_list = @"konten.csv";
            string journal = @"journal.csv";
            string er = @"erfolgsrechnung.txt";
            string eb = @"eroffnungsbilanz.txt";
            string sb1 = @"schlussbilanz1.txt";
            string sb2 = @"schlussbilanz2.txt";
            string[] liste_konten;
            string[] liste_attripute;
            List<string> liste = new List<string>();
            //Aktualisierung
            Erfolg_berechnung();
            Schlussbilanz1();
            Schlussbilanz2();
            //Inhalt zu Liste
            using (StreamReader reader = new StreamReader(data_list))
            {
                liste_konten = reader.ReadLine().Split("; ");
                liste_attripute = reader.ReadLine().Split("; ");
            }
            liste.Add("Konten:");
            liste.Add("--------------------");
            liste.Add("");
            liste.Add("");
            for (int i= 1; i< liste_konten.Length; i++)
            {
                try
                {
                    string konto = @liste_konten[i]+ ".csv";
                    liste.Add(liste_konten[i] + " (" + liste_attripute[i] + "): ");
                    liste.Add("Datum\t\t\t|Nummer\t\t|Soll  \t\t|Haben  \t\t|");
                    using (StreamReader reader = new StreamReader(konto))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] split = line.Split(";");
                            line = string.Join("\t\t|", split);
                            liste.Add(line);
                        }
                    }
                    //Saldo und summe evtl. einfügen
                }
                catch
                {
                    liste.Add("Es gab einen fehler beim auslesen des Kontos \"" + liste_konten[i] + "\".");
                }
                liste.Add("");
                liste.Add("");
            }
            liste.Add("Journal:");
            liste.Add("--------------------");
            liste.Add("");
            try
            {
                liste.Add("Datum\t\t\t|Nummer\t\t|Soll  \t\t|Haben  \t|Beschreibungen |Betrag\t\t|");
                using (StreamReader reader = new StreamReader(journal))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] split = line.Split(";");
                        line = string.Join("\t\t|", split);
                        liste.Add(line);
                    }
                }
            }
            catch
            {
                liste.Add("Es gab einen Fehler beim auslesen des Journals.");
            }
            liste.Add("");
            liste.Add("");
            liste.Add("Eroeffnungsrechnung:");
            liste.Add("--------------------");
            liste.Add("");
            try
            {
                using (StreamReader reader = new StreamReader(eb))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        liste.Add(line);
                    }
                }
            }
            catch
            {
                liste.Add("Es gab einen Fehler beim auslesen der Eroeffnungsbilanz.");
            }
            liste.Add("");
            liste.Add("");
            liste.Add("Schlussbilanz 1:");
            liste.Add("--------------------");
            liste.Add("");
            try
            {
                using (StreamReader reader = new StreamReader(sb1))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        liste.Add(line);
                    }
                }
            }
            catch
            {
                liste.Add("Es gab einen Fehler beim auslesen der Schlussbilanz 1.");
            }
            liste.Add("");
            liste.Add("");
            liste.Add("Erfolgsrechnung:");
            liste.Add("--------------------");
            liste.Add("");
            try
            {
                using (StreamReader reader = new StreamReader(er))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        liste.Add(line);
                    }
                }
            }
            catch
            {
                liste.Add("Es gab einen Fehler beim auslesen der Erfolgsrechnung.");
            }
            liste.Add("");
            liste.Add("");
            liste.Add("Schlussbilanz 2:");
            liste.Add("--------------------");
            liste.Add("");
            try
            {
                using (StreamReader reader = new StreamReader(sb2))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        liste.Add(line);
                    }
                }
            }
            catch
            {
                liste.Add("Es gab einen Fehler beim auslesen der Schlussbilanz 2.");
            }
            liste.Add("");
            liste.Add("");
            //file dialog
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "export"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text Dokument|*.txt|Alle Datein|*.*"; // Filter files by extension
            Nullable<bool> result = dlg.ShowDialog();
            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string export = dlg.FileName;
                //write to file
                if (!File.Exists(export))
                {
                    FileInfo FI = new FileInfo(export);
                    FileStream FS = FI.Create();
                    FS.Close();
                }
                using (StreamWriter writer = new StreamWriter(export))
                {
                    foreach (string line in liste)
                    {
                        writer.WriteLine(line);
                    }
                }
            }
            //MessageBox.Show("Fertig");
        }
    }
}