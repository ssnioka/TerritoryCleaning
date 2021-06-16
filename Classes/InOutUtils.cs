using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Linq;

namespace LD_5.Classes
{
    public static class InOutUtils
    {

        /// <summary>
        /// Deletes old data files
        /// </summary>
        public static void DeleteOldFiles()
        {
            string results = HttpContext.Current.Server.MapPath("App_Data/Results.txt");
            File.Delete(results);
        }

        /// <summary>
        /// Method used to locate required files
        /// </summary>
        /// <param name="variation"></param>
        /// <returns>Files</returns>
        private static string[] FindFiles(string variation)
        {
            string[] Files = Directory.GetFiles(HttpContext.Current.Server.MapPath("App_Data/"), variation + "*.csv");
            return Files;
        }
        /// <summary>
        /// method used to put all of the data in different files into one register list
        /// </summary>
        /// <param name="variation"></param>
        /// <returns>All Data </returns>
        public static List<Register> ReadFiles(string variation)
        {
            string[] Files = FindFiles(variation);
            List<Register> AllData = new List<Register>();
            for (int i = 0; i < Files.Length; i++)
            {
                AllData.Add(ReadFile(Files[i]));
            }
            return AllData;
        }
        /// <summary>
        /// Reads data from one single file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static Register ReadFile(string fileName)
        {
            using (StreamReader reader = new StreamReader(fileName, Encoding.UTF8))
            {
                string line;
                if ((line = reader.ReadLine()) == null)
                {
                    throw new FormatException("No data available");
                }
                else
                {
                    string street = line;
                    List<Residents> Residents = new List<Residents>();
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] Values = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        string surname = Values[0];
                        int adultscount = int.Parse(Values[1]);
                        int kidscount = int.Parse(Values[2]);
                        double squaremeters = double.Parse(Values[3]);
                        Residents Resident = new Residents(surname, adultscount, kidscount, squaremeters);
                        Residents.Add(Resident);
                    }
                    return new Register(street, Residents);
                }
            }
        }

        /// <summary>
        /// Reads Valuations data
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<Valuation> ReadValuations(string fileName)
        {
            List<Valuation> Valuations = new List<Valuation>();
            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Values = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    int adultscountvalue = int.Parse(Values[0]);
                    int kidscountvalue = int.Parse(Values[1]);
                    double priceforsqrmeter = double.Parse(Values[2]);
                    Valuation valuation = new Valuation(adultscountvalue, kidscountvalue, priceforsqrmeter);
                    Valuations.Add(valuation);
                }

                if (Valuations == null)
                {
                    throw new FormatException("No data available");
                }
                return Valuations;

            }

        }

        /// <summary>
        /// Residents Header data table
        /// </summary>
        /// <param name="table"></param>
        public static void GenerateTableHeader(Table table)
        {
            TableHeaderCell cell1 = new TableHeaderCell();
            TableHeaderCell cell2 = new TableHeaderCell();
            TableHeaderCell cell3 = new TableHeaderCell();
            TableHeaderCell cell4 = new TableHeaderCell();
            TableHeaderCell cell5 = new TableHeaderCell();

            cell1.Text = "Gatvė";
            cell2.Text = "Pavardė";
            cell3.Text = "Suaugusiųjų žmonių skaičius";
            cell4.Text = "Vaikų skaičius";
            cell5.Text = "Buto plotas kv.m";

            TableHeaderRow row = new TableHeaderRow();

            row.Cells.Add(cell1);
            row.Cells.Add(cell2);
            row.Cells.Add(cell3);
            row.Cells.Add(cell4);
            row.Cells.Add(cell5);
            table.Rows.Add(row);
        }
        /// <summary>
        /// Generates table header for valuations data
        /// </summary>
        /// <param name="table"></param>
        public static void GenerateTableHeaderForValuations(Table table)
        {
            TableHeaderCell cell1 = new TableHeaderCell();
            TableHeaderCell cell2 = new TableHeaderCell();
            TableHeaderCell cell3 = new TableHeaderCell();
         

            cell1.Text = "Suagusiųjų skaičius";
            cell2.Text = "Vaikų skaičius";
            cell3.Text = "Kaina už kv.m";
       

            TableHeaderRow row = new TableHeaderRow();

            row.Cells.Add(cell1);
            row.Cells.Add(cell2);
            row.Cells.Add(cell3);
   
            table.Rows.Add(row);
        }
        /// <summary>
        /// generates table for  valuations data
        /// </summary>
        /// <param name="table"></param>
        /// <param name="Valuations"></param>
        public static void GenerateTableforValuations(Table table, List<Valuation> Valuations)
        {
            GenerateTableHeaderForValuations(table);
            foreach (var (valuation, adultscount, kidscount, priceforsqrmeter, row) in from Valuation valuation in Valuations
                                                                                       let adultscount = new TableCell()
                                                                                       let kidscount = new TableCell()
                                                                                       let priceforsqrmeter = new TableCell()
                                                                                       let row = new TableRow()
                                                                                       select (valuation, adultscount, kidscount, priceforsqrmeter, row))
            {
                adultscount.Text = valuation.AdultsCountValue.ToString();
                kidscount.Text = valuation.KidsCountValue.ToString();
                priceforsqrmeter.Text = valuation.PriceForSqrMeter.ToString();
                row.Cells.Add(adultscount);
                row.Cells.Add(kidscount);
                row.Cells.Add(priceforsqrmeter);
                table.Rows.Add(row);
            }
        }
        /// <summary>
        /// Generates Table for residents
        /// </summary>
        /// <param name="table"></param>
        /// <param name="AllData"></param>

        public static void GenerateTableForResidents(Table table, List<Register> AllData)
        {
            GenerateTableHeader(table);
            foreach (Register register in AllData)
            {
    
                for (int i = 0; i < register.Residents.Count; i++)
                {
                    TableCell street = new TableCell();
                    TableCell surname = new TableCell();
                    TableCell adultscount = new TableCell();
                    TableCell kidscount = new TableCell();
                    TableCell squaremeters = new TableCell();
                    TableRow row = new TableRow();
                    surname.Text = register.Residents[i].Surname;
                    adultscount.Text = register.Residents[i].AdultsCount.ToString();
                    kidscount.Text = register.Residents[i].KidsCount.ToString();
                    squaremeters.Text = register.Residents[i].SquareMeters.ToString();
                    street.Text = register.Street;
                    row.Cells.Add(street);
                    row.Cells.Add(surname);
                    row.Cells.Add(adultscount);
                    row.Cells.Add(kidscount);
                    row.Cells.Add(squaremeters);

                    table.Rows.Add(row);

                }
                
            }
        }
        public static void GenerateTableHeaderForNewList(Table table)
        {
            TableHeaderCell cell1 = new TableHeaderCell();
            TableHeaderCell cell2 = new TableHeaderCell();
            TableHeaderCell cell3 = new TableHeaderCell();
      

            cell1.Text = "Gatvė";
            cell2.Text = "Pavardė";
            cell3.Text = "Žmonių skaičius";

            TableHeaderRow row = new TableHeaderRow();

            row.Cells.Add(cell1);
            row.Cells.Add(cell2);
            row.Cells.Add(cell3);
            table.Rows.Add(row);
        }
        public static void GenerateTableForResidentsList(Table table, List<Register> AllData)
        {
            GenerateTableHeaderForNewList(table);
            foreach (Register register in AllData)
            {

                for (int i = 0; i < register.Residents.Count; i++)
                {
                    
                    TableCell street = new TableCell();
                    TableCell surname = new TableCell();
                    TableCell PeopleCount = new TableCell();
                    TableRow row = new TableRow();
                   // int People = register.Residents[i].AdultsCount + register.Residents[i].KidsCount;
                    surname.Text = register.Residents[i].Surname;
                    PeopleCount.Text = Register.PeopleCount(register.Residents[i]).ToString();
                    street.Text = register.Street;
                    row.Cells.Add(street);
                    row.Cells.Add(surname);
                    row.Cells.Add(PeopleCount);
                 

                    table.Rows.Add(row);

                }

            }
        }
        /// <summary>
        /// Prints initial data to txt file:)
        /// </summary>
        /// <param name="AllData"></param>

        public static void PrintToTxt(List<Register> AllData)
        {
            using (StreamWriter writer = new StreamWriter(HttpContext.Current.Server.MapPath("App_Data/Results.txt")))
            {
                writer.WriteLine(new string('-', Residents.ToStringHeader().Length));
                writer.WriteLine("INITIAL RESIDENTS DATA: ");
                writer.WriteLine(new string('-', Residents.ToStringHeader().Length));

                foreach (Register register in AllData)
                {
                    writer.WriteLine(register.Street);
                    writer.WriteLine(new string('-', Residents.ToStringHeader().Length));
                    writer.WriteLine(Residents.ToStringHeader());
                    writer.WriteLine(new string('-', Residents.ToStringHeader().Length));
                    for ( int i =0 ; i < register.Residents.Count; i++)
                    {
                        writer.WriteLine(register.Residents[i].ToString());
                        writer.WriteLine(new string('-', Residents.ToStringHeader().Length));

                    }
                    writer.WriteLine();
                }
            }

        }
        /// <summary>
        /// Prints valuations data to txt
        /// </summary>
        /// <param name="Valuations"></param>
        public static void PrintToTxtValuations(List<Valuation> Valuations)
        {
            using (StreamWriter writer = new StreamWriter(HttpContext.Current.Server.MapPath("App_Data/Results1.txt")))
            {
                writer.WriteLine(new string('-', Valuation.ToStringHeader().Length));
                writer.WriteLine("INITIAL VALUATIONS DATA: ");
                writer.WriteLine(new string('-', Valuation.ToStringHeader().Length));
                writer.WriteLine(new string('-', Valuation.ToStringHeader().Length));
                writer.WriteLine(Valuation.ToStringHeader());
                writer.WriteLine(new string('-', Valuation.ToStringHeader().Length));
                foreach (Valuation valuation in Valuations)
                {
                    writer.WriteLine(valuation.ToString());
                    writer.WriteLine(new string('-', Valuation.ToStringHeader().Length));


                }
                writer.WriteLine();
            }
        }
    }
}