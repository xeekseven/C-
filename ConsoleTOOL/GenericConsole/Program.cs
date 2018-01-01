using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            LinkList<string> list = new LinkList<string>();
            //list.Init();
            ListInvoke<LinkList<string>, string> L = new ListInvoke<LinkList<string>, string>(list);
            //L.Init();
            for (int index = 1; index < 101; index++)
            {
                L.Add(index.ToString());
            }

            L.Update("2", "4");
            int location_4 = L.Find("4");
            int location_2 = L.Find("20");
            L.Remove("4");
            L.Remove("4");
            L.vistList((e) =>
            {
                Console.WriteLine(e);
            });
            //list.Init();
            //for(int index =1;index<101;index++)
            //{
            //    list.AddNode(index);
            //}

            //list.AddNode(2);
            //list.AddNode(2);
            //list.UpdateNode(2, 4);
            //int location_4 = list.FindNode(4);
            //int location_2 = list.FindNode(2);
            //list.RemoveNode(4);
            Console.ReadKey();
        }
    }
}
