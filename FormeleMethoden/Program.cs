using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormeleMethoden
{
    class Program
    {
        static void Main(string[] args)
        {
            char[] alphabet = { 'a', 'b' };
            Automata<string> a = new Automata<string>(alphabet);
            //a.AddTransition(new Transition<string>("q0", 'a', "q1"));
            //a.AddTransition(new Transition<string>("q0", 'b', "q4"));

            //a.AddTransition(new Transition<string>("q1", 'a', "q4"));
            //a.AddTransition(new Transition<string>("q1", 'b', "q2"));

            //a.AddTransition(new Transition<string>("q2", 'a', "q3"));
            //a.AddTransition(new Transition<string>("q2", 'b', "q4"));

            //a.AddTransition(new Transition<string>("q3", 'a', "q1"));
            //a.AddTransition(new Transition<string>("q3", 'b', "q2"));

            //a.AddTransition(new Transition<string>("q4", 'a'));
            //a.AddTransition(new Transition<string>("q4", 'b'));

            //a.DefineAsStartState("q0");

            //a.DefineAsFinalState("q2");
            //a.DefineAsFinalState("q3");
            a.AddTransition(new Transition<string>("q0", 'a', "q0"));
            a.AddTransition(new Transition<string>("q0", 'b', "q1"));
            a.AddTransition(new Transition<string>("q1", 'a', "q1"));
            a.AddTransition(new Transition<string>("q1", 'b', "q0"));




            a.PrintTransitions();
            Console.ReadLine();
        }

    }
}
