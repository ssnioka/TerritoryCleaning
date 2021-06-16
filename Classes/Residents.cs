using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LD_5.Classes
{
    public class Residents : IComparable<Residents>
    {
        public string Surname { get; set; }
        public int AdultsCount { get; set; }
        public int KidsCount { get; set; }
        public double SquareMeters { get; set; }
       
  


        public Residents(string surname, int adultscount, int kidscount, double squaremeters)
        {
            this.Surname = surname;
            this.AdultsCount = adultscount;
            this.KidsCount = kidscount;
            this.SquareMeters = squaremeters;
        }

        public Residents() { }

        public bool Equals(Residents other)
        {
            return other != null && Surname == other.Surname
                && AdultsCount == other.AdultsCount && KidsCount == other.KidsCount && SquareMeters == other.SquareMeters;
        }


        public static string ToStringHeader()
        {
            return String.Format("| {0, -15} | {1, -5} | {2, -5} | {3, -5} |", "Pavardė", "Suaugusiųjų skaičius", "Vaikų skaičius", "Buto plotas");
        }
        public override string ToString()
        {
            return String.Format("| {0, -15} | {1, -20} | {2, -14} | {3, -11} |", Surname, AdultsCount, KidsCount, SquareMeters);
        }

        public int CompareTo(Residents other)
        {
            return Surname.CompareTo(other.Surname);
        }
    }
}