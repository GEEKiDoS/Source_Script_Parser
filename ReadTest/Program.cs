using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSP;


namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleColor ori1 = Console.ForegroundColor;
            ConsoleColor ori2 = Console.BackgroundColor;
            SourceScriptParser SourceScriptParser = new SourceScriptParser();
            Console.WriteLine("Plz drag source engine script to here and enter");
            try
            {
                string file = Console.ReadLine();
                if (file.StartsWith("\"")&&file.EndsWith("\""))
                {
                    file = file.Substring(1, file.Length - 2);
                }
                SourceScriptParser.Parser(file);
            }
            catch(Exception a)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("Error");
                Console.ForegroundColor = ori1;
                Console.BackgroundColor = ori2;
                Console.Write(":" + a);
                Console.ReadLine();
                return;
            }
        redo:
            {
                Console.WriteLine("");
                Console.WriteLine("input a Section Name and press any key to next");
                Console.WriteLine("");
                string s = Console.ReadLine();
                Console.WriteLine("");
                Console.WriteLine("input a Setting Name and press any key to get Key");
                Console.WriteLine("");
                string s1 = Console.ReadLine();
                Console.WriteLine("");
                Console.WriteLine("The Key of " + s1 + " is " + SourceScriptParser.GetSetting(s, s1));
                Console.ReadLine();
            }
            Console.WriteLine("continue? yes or no");
            if (Console.ReadLine().ToString().ToLower() == "yes")
                goto redo;
        }
    }
}
