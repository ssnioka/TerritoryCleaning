using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LD_5.Classes
{
    public class Valuation
    {
        public int AdultsCountValue { get; set; }
        public int KidsCountValue { get; set; }
        public double PriceForSqrMeter { get; set; }


        public Valuation(int adultscountvalue, int kidscountvalue, double priceforsqrmeter)
        {
            this.AdultsCountValue = adultscountvalue;
            this.KidsCountValue = kidscountvalue;
            this.PriceForSqrMeter = priceforsqrmeter;
        }
        public Valuation() { }

        public bool Equals(Valuation other)
        {
            return other != null && AdultsCountValue == other.AdultsCountValue
                && KidsCountValue == other.KidsCountValue && PriceForSqrMeter == other.PriceForSqrMeter;
        }

        public static string ToStringHeader()
        {
            return String.Format("| {0, -5} | {1, -5} | {2, 5} |", "Suagusiųjų kiekis", "Vaikų kiekis", "Kaina už kv.m");

        }
        public override string ToString()
        {
            return String.Format("| {0, -17} | {1, -12} | {2,-13} |", AdultsCountValue, KidsCountValue, PriceForSqrMeter);
        }



    }
}