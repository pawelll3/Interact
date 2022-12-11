using LiteDB;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interact
{
    internal class DataRead
    {
        LiteDatabase db = new LiteDatabase(@"DrugInter.db");

        //Get result from database using embeded searching algorythm
        public string[] Get(string drugA, string drugB)
        {
           string[] result = {null,null,null};

           var col = db.GetCollection<DrugInter>("Master");
           var inter_a = col.Find(x => x.drugA.Contains(drugA)).ToList();

            if (inter_a.Count != 0)
            {
                result[1] = "T";
                foreach (var item in inter_a)
                {
                    if (item.drugB == drugB)
                    {
                        result[0] = item.inter;
                        result[2] = "T";
                        return result;
                    }
                }
            }

            var inter_b = col.Find(x => x.drugA.Contains(drugB)).ToList();
            if (inter_b.Count != 0)
            {
                result[2] = "T";
                foreach (var item in inter_b)
                {
                    if (item.drugB == drugA)
                    {
                        result[0] = item.inter;
                        result[1] = "T";    
                        return result;
                    }
                }
            }
            
            return result;
        }

        //Get result from database using sorted tables
        //ALSO TEXT AUTOCOMPLETION!!!!!

        //public string Get_(string drugA, string drugB)
        //{
        //    int byA = -1;
        //    int byB = -1;

        //    var col_a = db.GetCollection<DrugInterSorted>(drugA);
        //    var col_b = db.GetCollection<DrugInterSorted>(drugB);

        //    List<DrugInterSorted> drug_a_L;
        //    List<DrugInterSorted> drug_b_L;

        //    try
        //    {
        //        drug_a_L = (List<DrugInterSorted>)col_a;
        //        drug_b_L = (List<DrugInterSorted>)col_b;
        //    }
        //    catch (Exception)
        //    {
        //        return "wrong";
        //    }

        //    DrugInterSorted drug_a = drug_a_L[0];
        //    DrugInterSorted drug_b = drug_b_L[0];

        //    List<int> A_list_drugs_a = new List<int>();
        //    List<int> A_list_drugs_b = new List<int>();

        //    List<int> B_list_drugs_a = new List<int>();
        //    List<int> B_list_drugs_b = new List<int>();

        //    var byA_L = drug_a.b.ToList().Intersect(drug_b.a.ToList()).ToList();
        //    byA = byA_L[0];
        //    var byB_L = drug_b.b.ToList().Intersect(drug_a.a.ToList()).ToList();
        //    byB = byB_L[0];

        //    return null;
        //}  //Get using tables from sorted dictionary

    }
}
