using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Globalization;
using CsvHelper.Configuration;
using CsvHelper;
using LiteDB;
using Microsoft.VisualBasic.FileIO;

namespace Interact
{
    internal class PrepareData
    {
        LiteDatabase db = new LiteDatabase(@"DrugInter.db");

        public void Raw2Merged()
        {
            string[] paths = { @"A.csv", @"B.csv", @"D.csv", @"H.csv", @"L.csv", @"P.csv", @"R.csv", @"V.csv" };

            List<String> drugA = new List<String>();
            List<String> drugB = new List<String>();
            List<String> interaction = new List<String>();

            for (int i = 0; i < 8; i++)
            {
                using (TextFieldParser csvParser = new TextFieldParser(paths[i]))
                {
                    csvParser.CommentTokens = new string[] { "#" };
                    csvParser.SetDelimiters(new string[] { "," });
                    csvParser.HasFieldsEnclosedInQuotes = false;
                    char[] chars = { '\"', ' ', ';' };
                    while (!csvParser.EndOfData)
                    {
                        string[] fields = csvParser.ReadFields();
                        if (!(fields[1].Contains("DDInter") || fields[3].Contains("DDInter")))  //Mistake in files, in name fields sometimes identificators are present. Omiting these fields required!
                        {
                            drugA.Add(fields[1].Trim(chars));
                            drugB.Add(fields[3].Trim(chars));
                            interaction.Add(fields[4].Trim(chars)); ;
                        }
                    }
                }
            }

            List<List<String>> merged = new List<List<String>>
            {
                drugA,
                drugB,
                interaction
            };

            
            WriteMaster_DB(merged);
            Conv2Dictionary(merged);
        }

        public void Conv2Dictionary(List<List<String>> merged)
        {

            string[] drugA_array = merged[0].ToArray();
            string[] drugB_array = merged[1].ToArray();
            string[] interaction_array = merged[2].ToArray();

            var drugs = drugA_array.Concat(drugB_array).Distinct().ToArray();
            Dictionary<string, List<List<int>>> drugs_dict = new Dictionary<string, List<List<int>>>();

            for (int i = 0; i < drugs.Length; i++)
            {
                List<int> indexesA = new List<int>();
                List<int> indexesB = new List<int>();
                List<List<int>> indexes = new List<List<int>>();

                for (int x = 0; x < drugA_array.Length; x++)
                {
                    if (drugs[i] == drugA_array[x])
                    {
                        indexesA.Add(x);
                    }
                    if (drugs[i] == drugB_array[x])
                    {
                        indexesB.Add(x);
                    }
                }

                indexes.Add(indexesA);
                indexes.Add(indexesB);
                drugs_dict.Add(drugs[i], indexes);
            }

            Dict2Ordered(drugs_dict);
        }

        public void Dict2Ordered(Dictionary<string, List<List<int>>> drugs_dict)
        {
            var drugs_dict_ordered = new SortedDictionary<string, List<List<int>>>(drugs_dict);

            SepAlphabeticaly_DB(drugs_dict_ordered);
        }

        public void SepAlphabeticaly_DB(SortedDictionary<string, List<List<int>>> dictionary)
        {

            List<char> chars = new List<char>();
            for (int i = 0; i < dictionary.Count(); i++)
            {
                char first = dictionary.ElementAt(i).Key.ToUpper().Trim().ToCharArray()[0];

                var col = db.GetCollection<DrugInterSorted>($"{first}");

                var drug_sorted = new DrugInterSorted
                {
                    drug = dictionary.Keys.ElementAt(i),
                    a = dictionary.Values.ElementAt(i)[0],
                    b = dictionary.Values.ElementAt(i)[1],
                };

                chars.Add(first);
                col.Insert(drug_sorted);
            }
            chars = chars.Distinct().ToList();
            TableNamesWr(chars);
        }

        public void WriteMaster_DB(List<List<String>> merged)
        {
            var col = db.GetCollection<DrugInter>("Master");
            for (int i = 0; i < merged.ElementAt(0).Count(); i++)
            {
                var drug = new DrugInter
                {
                    drugA = merged.ElementAt(0)[i],
                    drugB = merged.ElementAt(1)[i],
                    inter = merged.ElementAt(2)[i]
                };
                col.Insert(drug);
            }
        }

        public void TableNamesWr(List<char> chars)
        {
            StreamWriter wr = new StreamWriter("TableNames.txt");
            foreach (var item in chars)
            {
                wr.WriteLine(item);
            }
            wr.Close();
            db.Dispose();
            StreamWriter fin = new StreamWriter("fin.txt");
            fin.Close();
        } 

        public string[] TableNamesRead()
        {
            StreamReader re = new StreamReader("TableNames.txt");
            string all = re.ReadToEnd();
            string[] str = all.Split('\n');
            return str;
        }
    }
}
