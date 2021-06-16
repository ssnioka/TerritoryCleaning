using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LD_5.Classes
{
    public class Register
    {
        public string Street { get; set; }
        public List<Residents> Residents { get; set; }

        public Register(string street, List<Residents> residents)
        {
            this.Street = street;
            this.Residents = residents;
        }


        /// <summary>
        /// Counts people
        /// </summary>
        /// <param name="resident"></param>
        /// <returns></returns>
        public static int PeopleCount(Residents resident)
        {
            return resident.AdultsCount + resident.KidsCount;
        }

        /// <summary>
        /// Counts how much does the resident pay for the rent 
        /// </summary>
        /// <param name="resident"></param>
        /// <param name="AllValuations"></param>
        /// <returns></returns>
        public static double ResidentFee(Residents resident, List<Valuation> AllValuations)
        {
            foreach (var valuation in from Valuation valuation in AllValuations
                                      where valuation.AdultsCountValue == resident.AdultsCount && valuation.KidsCountValue == resident.KidsCount
                                      select valuation)
            {
                return valuation.PriceForSqrMeter * resident.SquareMeters;
            }

            return 0;
        }

        /// <summary>
        /// method used to find average price of valuation
        /// </summary>
        /// <param name="AllValuations"></param>
        /// <returns></returns>
        public static double AverageValuation(List<Valuation> AllValuations)
        {
            int peoplecount = 0;
            double pricecount = 0;
   
            foreach(Valuation valuation in AllValuations)
            {
                peoplecount += valuation.KidsCountValue + valuation.AdultsCountValue;
                pricecount += valuation.PriceForSqrMeter;
            }
           return  pricecount/peoplecount;
        }
        /// <summary>
        /// method to find how much resident is paying on average
        /// </summary>
        /// <param name="resident"></param>
        /// <param name="AllValuations"></param>
        /// <returns></returns>
        public static double AverageResidentFee(Residents resident, List<Valuation> AllValuations)
        {

             double avg = ResidentFee(resident, AllValuations) / (resident.AdultsCount + resident.KidsCount);
            return avg;
        }
        /// <summary>
        /// method to remove residents who paid less than avg
        /// </summary>
        /// <param name="AllData"></param>
        /// <param name="AllValuations"></param>
        /// <returns></returns>
        /// 

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static List<Register> RemoveResidents(List<Register> AllData, List<Valuation> AllValuations)
        {
            foreach (Register register in AllData)
            {
                for (int i = 0; i < register.Residents.Count; i++)
                {
                    if (AverageValuation(AllValuations) > AverageResidentFee(register.Residents[i], AllValuations))
                    {
                        register.Residents.Remove(register.Residents[i]);
                    }
                }
            }
            return AllData;
        }

        //public static List<Register> RemoveResidentsLINQ(List<Register> AllData, List<Valuation> AllValuations)
        //{
        //    var residents = from Reg in AllData from res in Reg.Residents where
        //                    AverageValuation(AllValuations) > AverageResidentFee(res, AllValuations) select Reg.Residents.Remove;

        //}

        /// <summary>
        /// Finds residents who pay more than k amount
        /// </summary>
        /// <param name="Residents"></param>
        /// <param name="AllValuations"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Residents MoreThanK(Residents resident, List<Valuation> AllValuations, double k)
        {
            if (ResidentFee(resident, AllValuations) > k)
                return resident;
            else
            {
                return null;
            }
        }


        /// <summary>
        /// method used to find residents who paid more than k
        /// </summary>
        /// <param name="AllData"></param>
        /// <param name="AllValuations"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static List<Register> ResidentsWhoPaidMore(List<Register> AllData, List<Valuation> AllValuations, double k)
        {
            foreach (Register register in AllData)
            {
                for (int i = 0; i < register.Residents.Count; i++)
                {
                    if (MoreThanK(register.Residents[i], AllValuations, k) == null)
                    {
                        register.Residents.Remove(register.Residents[i]);
                        i = i - 1;
                    }
                }
            }
            return AllData;
        }
        /// <summary>
        /// method to sort list
        /// </summary>
        /// <param name="Register"></param>
        /// <returns></returns>
        public static List<Register> Sort(List<Register> Register)
        {
            foreach (Register res in Register)
            {
                res.Residents = res.Residents.OrderBy(x => x.Surname).ToList();
            }
            Register = Register.OrderBy(x => x.Street).ToList();
            return Register;
        }
    


    }
}