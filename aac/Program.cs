using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace aac
{
    class Program
    {
        static void Main(string[] args)
        {
            ArrayList lib = new ArrayList();
            lib.Add("张三");
            lib.Add("李四");
            lib.Add("王五");
            lib.Add("大忽悠");
            lib.Add("老陈");
            foreach (string a in lib)
            {                
                Console.WriteLine(a);
            }
        }
    }
}
