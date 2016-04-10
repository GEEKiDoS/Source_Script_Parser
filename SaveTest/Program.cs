using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSP;

namespace Test1
{
    class Program
    {
        static void Main(string[] args)
        {
            SourceScriptParser SourceScriptParser = new SourceScriptParser();
        redo:
            {
                Console.WriteLine("section");
                string s = Console.ReadLine();
                Console.WriteLine("");
                Console.WriteLine("setting");
                string s1 = Console.ReadLine();
                Console.WriteLine("");
                Console.WriteLine("key");
                string k = Console.ReadLine();
                Console.WriteLine("");
                SourceScriptParser.AddSetting(s, s1, k);
            }
            Console.WriteLine("");
            Console.WriteLine("continue? yes or no");
            if (Console.ReadLine().ToString().ToLower() == "yes")
                goto redo;
            else
            {
                Console.WriteLine("filename");
                SourceScriptParser.SaveSetting(Console.ReadLine().ToString());
            }
        }
    }
}
