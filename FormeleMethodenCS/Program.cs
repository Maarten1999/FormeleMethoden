using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormeleMethodenCS
{
    class Program
    {
        static void Main(string[] args)
        {
            char[] alphabet = { 'a', 'b' };

            DFA<string> dfa = new DFA<string>(alphabet);
            dfa.AddTransition(new Transition<string>("q0", 'a', "q1"));
            dfa.AddTransition(new Transition<string>("q0", 'b', "q3"));

            dfa.AddTransition(new Transition<string>("q1", 'a', "q5"));
            dfa.AddTransition(new Transition<string>("q1", 'b', "q2"));

            dfa.AddTransition(new Transition<string>("q2", 'a', "q5"));
            dfa.AddTransition(new Transition<string>("q2", 'b', "q6"));

            dfa.AddTransition(new Transition<string>("q3", 'a', "q5"));
            dfa.AddTransition(new Transition<string>("q3", 'b', "q4"));

            dfa.AddTransition(new Transition<string>("q4", 'a', "q6"));
            dfa.AddTransition(new Transition<string>("q4", 'b', "q5"));

            dfa.AddTransition(new Transition<string>("q5", 'a', "q5"));
            dfa.AddTransition(new Transition<string>("q5", 'b', "q5"));

            dfa.AddTransition(new Transition<string>("q6", 'a', "q6"));
            dfa.AddTransition(new Transition<string>("q6", 'b', "q6"));

            dfa.DefineAsStartState("q0");
            dfa.DefineAsFinalState("q6");
            
            List<string> words = dfa.GetLanguage(6, false).ToList();//dfa.GetLanguage(2);
            Console.WriteLine($"Number of combinations: {words.Count()}");
            words.ForEach(w => Console.WriteLine(w));
            Console.ReadLine();
            //do
            //{
            //    Console.Clear();
            //    Console.Write("Geef string: ");
            //    string s = Console.ReadLine();
            //    bool success = dfa.Accept(s);
            //    Console.WriteLine($"String geaccepteerd: {success}");
            //    Console.WriteLine("Druk op esc om te stoppen.");
            //} while (Console.ReadKey().Key != ConsoleKey.Escape);
            

            
            //var x = dfa.GetToStates("q4", 'a');
            //foreach (var i in x)
            //{
            //    Console.WriteLine(i);
            //}
            //Console.ReadLine();
        }
    }
}
