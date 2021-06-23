using System;

namespace Contracts
{
    /// <summary>
    /// MobileFoodFacilityPermit is a single record of a food truck 
    /// </summary>
    public class MobileFoodFacilityPermit
    {
        public int locationid { get; set; }
        public string Applicant { get; set; }
        public string FacilityType { get; set; }
        public int cnn { get; set; }
        public string LocationDescription { get; set; }
        public string Address { get; set; }
        public int blocklot { get; set; }
        public int block { get; set; }
        public int lot { get; set; }
        public string permit { get; set; }
        public string Status { get; set; }
        public string FoodItems { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Schedule { get; set; }
        public string dayshours { get; set; }
        public string NOISent { get; set; }
        public DateTime Approved { get; set; }
        public int Received { get; set; }
        public int PriorPermit { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Location { get; set; }
        public int FirePreventionDistricts { get; set; }
        public int PoliceDistricts { get; set; }
        public int SupervisorDistricts { get; set; }
        public int ZipCodes { get; set; }
        public int Neighborhoodsold { get; set; }
    }
}