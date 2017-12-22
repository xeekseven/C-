using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleTOOL
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("各个项目的类为自用工具类！");

            ////string str ="-9.21E+18,-1494943961,北京,856728805,蒙自县,7,中兴,"蒙自市""泰和街-LZHQ",103.38182,23.36043,室外,蒙自市泰和街-LZHQ-003,460-00-37747-3,103.38182,23.360537,37747,3,34728,,38400,8,F,,270,13,16,52,";
            //string str = "扫频.bin";
            //Console.WriteLine(Path.GetFileNameWithoutExtension(str));

            //DateTime prevTime =DateTime.Parse(DateTime.Parse("2017-11-06 20:00:11.000").ToShortDateString());
            //DateTime nowTime = DateTime.Parse(DateTime.Parse("2017-11-07 2:00:11.000").ToShortDateString());
            //Console.WriteLine(nowTime.Subtract(prevTime).Days);
            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd"));
            //Console.WriteLine("20170507111524".Substring(0, 8));
            string longstr = "11-16 1、今日联系用户出国不在家中。2、青羊大道8号成都花园为多起2/4G信号投诉重灾区，多个重要客户曾经多次投诉，但是由于之前的物管阻扰，青羊移动始终无法入场施工新建基站（老基站被老物业干扰后拆除）。目前该小区物管已经更换，新物管愿意为小区业主解决信号问题包括:成都花园朗庭园AB区、成都花园翠雍华庭一、成都花园翠雍华庭二、成都花园望郡，前期与新物管方协商制度方案，进场时间估计在11月20号左右，待室分基站建设后将解决成都花园信号问题。3、在此期间烦请客服经理为用户开通VLOTE通话功能，提高用户感知";
            Console.WriteLine(Environment.ProcessorCount);
            ConcurrentDictionary<char, int> odic = new ConcurrentDictionary<char, int>();
            Dictionary<char, int> dic = new Dictionary<char, int>();
            dic['c'] = 2;
            Console.WriteLine(dic['c']);
            Console.ReadKey();
        }
    }
}
