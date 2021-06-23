using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace FoodTruckNearMe
{
    public class RawDataContainer
    {
        public byte[] RawBytes { get; set; }
    }

    public static class MobileFoodFacilityPermitLoader
    {
        private static List<List<MobileFoodFacilityPermit>> DataSets = new List<List<MobileFoodFacilityPermit>>()
        {
            new List<MobileFoodFacilityPermit>(),new List<MobileFoodFacilityPermit>()
        };
        private static int CurrentDataSetIndex = -1;

        public static List<MobileFoodFacilityPermit> GetCurrentDataSet()
        {
            if (CurrentDataSetIndex == -1)
            {
                return null;
            }

            return DataSets[CurrentDataSetIndex];

        }

        public static List<MobileFoodFacilityPermit> LoadMobileFoodFacilityPermitsByBytes(byte[] bytes)
        {
            using Stream stream = new MemoryStream(bytes);
            using StreamReader streamReader = new StreamReader(stream);
            streamReader.ReadLine(); // discard first line
            List<string> rows = new List<string>();
            while (!streamReader.EndOfStream)
            {
                rows.Add(streamReader.ReadLine());
            }
            var data = rows
                .Select(x => x.Split(','));
            return LoadMobileFoodFacilityPermits(data.ToList());
        }

        public static List<MobileFoodFacilityPermit> LoadMobileFoodFacilityPermitsByFile(string fileLocation)
        {
           
            var buffer = File.ReadAllLines(fileLocation).Skip(1);
            var data = File.ReadAllLines(fileLocation).Skip(1)
                .Select(x => x.Split(','));
            var dataSet = LoadMobileFoodFacilityPermits(data.ToList());
            if (CurrentDataSetIndex == -1)
            {
                DataSets[0] = dataSet;
                CurrentDataSetIndex = 0;
            }
            else
            {
                if (CurrentDataSetIndex == 0)
                {
                    DataSets[1] = dataSet;
                    CurrentDataSetIndex = 1;
                }
                else
                {
                    DataSets[0] = dataSet;
                    CurrentDataSetIndex = 0;
                }
            }

            return GetCurrentDataSet();
        }

        public static List<MobileFoodFacilityPermit> LoadMobileFoodFacilityPermits(List<string[]> permitRecords)
        {
            // TODO: Need a better CSV parser.

            // NOTE: There are a few parsing issues with the data set;
            // 1. location is a string that contains a comma: "(123.343,-112.334)", which this linq query doesn't account for.  Fortunately its at the end of the record and we currently
            // don't need that data
            // 2. A few int.Parse(set[x]) threw exceptions and we need to find out what data did that.

            var query = from set in permitRecords
                where set[10] == "APPROVED"
                let c = new MobileFoodFacilityPermit
                {
                    locationid = int.Parse(set[0]),
                    Applicant = set[1],
                    FacilityType = set[2],
                    LocationDescription = set[4],
                    Address = set[5],
                    Status = set[10],
                    FoodItems = set[11],
                    Latitude = double.Parse(set[14]),
                    Longitude = double.Parse(set[15]),
                    Schedule = set[16],
                    dayshours = set[17],
                }
                select c;
            var dataSet = query.ToList();
            if (CurrentDataSetIndex == -1)
            {
                DataSets[0] = dataSet;
                CurrentDataSetIndex = 0;
            }
            else
            {
                if (CurrentDataSetIndex == 0)
                {
                    DataSets[1] = dataSet;
                    CurrentDataSetIndex = 1;
                }
                else
                {
                    DataSets[0] = dataSet;
                    CurrentDataSetIndex = 0;
                }
            }

            return GetCurrentDataSet();

        }

    }
}
