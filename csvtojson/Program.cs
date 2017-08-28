using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace ConsoleApp4
{
    public class first
    {
        public string IndicatorName { get; set; } // for first list
        public string year { get; set; }
        public string value { get; set; }

    }
    public class lastpart // for second list
    {
        public string year { get; set; }             //4th index
        public string IndicatorName { get; set; }    //2nd index
        public string Countrycode { get; set; }      //1st index
        public string countryname { get; set; }     //0th index
        public double value { get; set; }           //5th index
    }
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader streamReader = new StreamReader(new FileStream("C:/Users/Training/Desktop/Folder/Indicators.csv", FileMode.OpenOrCreate));
            StreamWriter obj = new StreamWriter(new FileStream("C:/Users/Training/Desktop/Folder/Content1.json", FileMode.OpenOrCreate));
            StreamWriter obj1 = new StreamWriter(new FileStream("C:/Users/Training/Desktop/Folder/Content2.json", FileMode.OpenOrCreate));
            StreamWriter obj2 = new StreamWriter(new FileStream("C:/Users/Training/Desktop/Folder/Content3.json", FileMode.OpenOrCreate));
            string[] country = { "AFG", "ARM", "AZE", "BHR", "BGD", "BTN", "BRN", "KHM", "CHN", "CXR", "CCK", "IOT", "GEO", "HKG", "IND", "IDN", "IRN", "IRQ", "ISR", "JPN", "JOR", "KAZ", "KWT", "KGZ", "LAO", "LBN", "MAC", "MYS", "MDV", "MNG", "MMR", "NPL", "PRK", "OMN", "PAK", "PHL", "QAT", "SAU", "SGP", "KOR", "LKA", "SYR", "TWN", "TJK", "THA", "TUR", "TKM", "ARE", "UZB", "VNM", "YEM" }; //asian country
            List<lastpart> list = new List<lastpart>(); //barchart
            List<lastpart> list1 = new List<lastpart>();
            List<lastpart> list2 = new List<lastpart>();
            var result = streamReader.ReadLine(); // line ko variable result mai daaldia 
            string[] str = result.Split(','); //comma se pehle vaale array mai jaaenge string type ka
            obj.Write("[" + "\n");
            obj1.Write("[" + "\n");
            while (!streamReader.EndOfStream) // till full file
            {
                var result1 = streamReader.ReadLine();
                string[] str1 = result1.Split(','); // columns ke data
                if (str1[0] == "India")
                {
                    if (str1[2] == "Rural population (% of total population)" || str1[2] == "Urban population (% of total)") // start 1st conditon
                    {
                        obj.WriteLine("{" + "\n" + "\"" + str[4] + "\"" + ":" + str1[4] + "," + "\n" + "\"" + str1[2] + "\"" + ":" + str1[5] + "\n" + "}");
                        // "\"" for """"
                        string y = (str1[4] == "2014" && str1[2] == "Urban population (% of total)") ? "]" : "," + "\n";  // jaise hi 2014 aaega vo ] lgadega
                        obj.Write(y);
                        obj.Flush(); // buffer mai data delete krne ke liye taaki new data aajae
                    }
                    if (str1[2] == "Urban population growth (annual %)") // start 2nd condition
                    {
                        obj1.WriteLine("{" + "\n" + "\"" + str[4] + "\"" + ":" + str1[4] + "," + "\n" + "\"" + str1[2] + "\"" + ":" + str1[5] + "\n" + "}"); // "\"" for """"
                        string x = (str1[4] == "2014" && str1[2] == "Urban population growth (annual %)") ? "]" : "," + "\n";
                        obj1.Write(x);
                        obj1.Flush(); // buffer mai data delete krne ke liye taaki new data aajae
                    }
                }
                for (int i = 0; i < country.Length; i++)
                {
                    if ((country[i] == str1[1]) && (str1[2] == "Urban population" || str1[2] == "Rural population"))
                    {
                        double temp;
                        double.TryParse(str1[5], out temp);
                        list.Add(new lastpart() { year = str1[4], IndicatorName = str1[2], value = temp, Countrycode = str1[1], countryname = str1[0] });
                    }
                }
            }
            var stackroute = from m in list group new { m.value, m.IndicatorName } by m.countryname into NewG from n in (from m in NewG group new { m.value } by m.IndicatorName into xyz select new { xyz.Key, sum = xyz.Sum(o => o.value) }) group n by NewG.Key;
            obj2.WriteLine("[" + "\n");

            foreach (var i in stackroute)
            {
                obj2.WriteLine("{" + "\n" + "\"" + "CountryCode" + "\"" + ":" + "\"" + i.Key + "\"" + ",\n"); // for all data
                foreach (var j in i) // for filtering
                {
                    var rp = j.Key == "Urban population" ? "\"" + "Urban" + "\"" + ":" + j.sum : "\"" + "Rural" + "\"" + ":" + j.sum + ","; // j key is matched with urban and rural
                    obj2.WriteLine(rp);
                }
                var r = i.Key == "Vietnam" ? "}" : "},"; // if vietnam then } else ,
                obj2.WriteLine(r);
            }
            obj2.WriteLine("]"); // end
            obj2.Flush();
        }
    }
}
