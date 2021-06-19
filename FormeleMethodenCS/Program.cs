﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormeleMethodenCS.Converters;

namespace FormeleMethodenCS
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestToMinimalDFA();
            //            TestRegex();
            TestDFAOperations();
            Console.ReadLine();
            
            //var x = dfa.GetToStates("q4", 'a');
            //foreach (var i in x)
            //{
            //    Console.WriteLine(i);
            //}
            //Console.ReadLine();
        }

        static void TestDFAOperations()
        {
            char[] alphabet = { 'a', 'b' };

            DFA<string> dfa = new DFA<string>(alphabet);
            dfa.AddTransition(new Transition<string>("1", 'a', "2"));
            dfa.AddTransition(new Transition<string>("1", 'b', "1"));

            dfa.AddTransition(new Transition<string>("2", 'a', "2"));
            dfa.AddTransition(new Transition<string>("2", 'b', "3"));

            dfa.AddTransition(new Transition<string>("3", 'a', "4"));
            dfa.AddTransition(new Transition<string>("3", 'b', "1"));

            dfa.AddTransition(new Transition<string>("4", 'a', "4"));
            dfa.AddTransition(new Transition<string>("4", 'b', "4"));

            dfa.DefineAsStartState("1");
            dfa.DefineAsFinalState("4");


            DFA<string> other = new DFA<string>(alphabet);
            other.AddTransition(new Transition<string>("1", 'a', "1"));
            other.AddTransition(new Transition<string>("1", 'b', "2"));

            other.AddTransition(new Transition<string>("2", 'a', "3"));
            other.AddTransition(new Transition<string>("2", 'b', "2"));

            other.AddTransition(new Transition<string>("3", 'a', "1"));
            other.AddTransition(new Transition<string>("3", 'b', "4"));

            other.AddTransition(new Transition<string>("4", 'a', "3"));
            other.AddTransition(new Transition<string>("4", 'b', "2"));

            other.DefineAsStartState("1");
            other.DefineAsFinalState("1");
            other.DefineAsFinalState("2");
            other.DefineAsFinalState("3");

            var newDfa = dfa.Or(other);
            Graphiz<string> graphiz = new Graphiz<string>(newDfa);
            graphiz.PrintGraph();
           
            return;
            dfa = dfa.Not();
            do
            {
                Console.Clear();
                Console.Write("Geef string: ");
                string s = Console.ReadLine();
                bool success = dfa.Accept(s);
                Console.WriteLine($"String geaccepteerd: {success}");
                Console.WriteLine("Druk op esc om te stoppen.");
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }
        static void TestDFA()
        {
            char[] alphabet = { 'a', 'b' };

            Automata<string> dfa = new DFA<string>(alphabet);
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

            Graphiz<string> graphiz = new Graphiz<string>(dfa);
            graphiz.PrintGraph();

            List<string> words = dfa.GetLanguage(6, true).ToList();//dfa.GetLanguage(2);
            Console.WriteLine($"Number of combinations: {words.Count()}");
            words.ForEach(w => Console.WriteLine(w));
            Console.ReadLine();
            do
            {
                Console.Clear();
                Console.Write("Geef string: ");
                string s = Console.ReadLine();
                bool success = dfa.Accept(s);
                Console.WriteLine($"String geaccepteerd: {success}");
                Console.WriteLine("Druk op esc om te stoppen.");
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }
        static void TestRegex()
        {
            RegExp a = new RegExp("a");
            RegExp b = new RegExp("b");

            // Expr1: "baa"
            RegExp expr1 = new RegExp("baa");
            // Expr2: "bb"
            RegExp expr2 = new RegExp("bb");
            // Expr3: "baa | bb"
            RegExp expr3 = expr1.Or(expr2);

            // All: " (a|b) *"
            RegExp all = (a.Or(b)).Star();
            // Expr4: " (baa | bb)+"
            RegExp expr4 = expr3.Plus();
            // Expr5: "(baa | bb)+ (a|b)*"
            RegExp expr5 = expr4.Dot(all);

            //Console.WriteLine("Taal van (baa):");
            //expr1.GetLanguage(5).ToList().ForEach(s => Console.WriteLine(s));
            //Console.WriteLine("");
            //Console.WriteLine("Taal van (bb):");
            //expr2.GetLanguage(5).ToList().ForEach(s => Console.WriteLine(s));
            //Console.WriteLine("");

            Console.WriteLine("Taal van (baa | bb):");
            expr3.GetLanguage(5).ToList().ForEach(s => Console.WriteLine(s));
            Console.WriteLine("");

            //Console.WriteLine("Taal van (a|b)*:");
            //all.GetLanguage(5).ToList().ForEach(s => Console.WriteLine(s));
            //Console.WriteLine("");

            //Console.WriteLine("Taal van (baa | bb)+:");
            //expr4.GetLanguage(5).ToList().ForEach(s => Console.WriteLine(s));
            //Console.WriteLine("");

            //Console.WriteLine("Taal van (baa | bb) + (a|b) *:");
            //expr5.GetLanguage(6).ToList().ForEach(s => Console.WriteLine(s));
            //Console.WriteLine("");
            string str;
            do
            {
                Console.Write("Input: ");
                str = Console.ReadLine();
                Console.WriteLine($"Accepts {expr3.ToString()} = {expr3.Accept(str)}");
            } while (str != "exit");
            
        }

        static void TestDFAGenerator()
        {
            string str = "bbabba";
            DFA<string> dfa = DFAGenerator.EndsWith(str);

            Graphiz<string> graphiz = new Graphiz<string>(dfa);
            graphiz.PrintGraph();

            Console.WriteLine("DFA Generated -> EndsWith: " + str);
        }

        static void TestThompsonConstruction()
        {
            RegExp a = new RegExp("a");
            RegExp b = new RegExp("b");

            // Expr1: "baa"
            RegExp expr1 = new RegExp("baa");
            // Expr2: "bb"
            RegExp expr2 = new RegExp("bb");
            // Expr3: "baa | bb"
            RegExp expr3 = expr1.Or(expr2);

            // All: " (a|b) *"
            RegExp all = (a.Or(b)).Star();
            // Expr4: " (baa | bb)+"
            RegExp expr4 = expr3.Plus();
            // Expr5: "(baa | bb)+ (a|b)*"
            RegExp expr5 = expr4.Dot(all);

            NDFA<string> ndfa = AutomataConverter.RegexToNDFA(expr5);

            Graphiz<string> graphiz = new Graphiz<string>(ndfa);
            graphiz.PrintGraph();

            Console.WriteLine("Regex to NDFA: " + expr5.ToString());
        }

        static void TestNDFAToDFA()
        {
            char[] alphabet = { 'a', 'b' };

            // Example from lesson 4:
            NDFA<string> ndfa = new NDFA<string>(alphabet);
            ndfa.AddTransition(new Transition<string>("q0", 'a', "q0"));
            ndfa.AddTransition(new Transition<string>("q0", 'a', "q1"));
            ndfa.AddTransition(new Transition<string>("q0", 'b', "q1"));

            ndfa.AddTransition(new Transition<string>("q1", 'a', "q2"));
            ndfa.AddTransition(new Transition<string>("q1", 'b', "q2"));
            
            ndfa.AddTransition(new Transition<string>("q2", 'a', "q2"));
            ndfa.AddTransition(new Transition<string>("q2", 'a', "q0"));
            
            ndfa.DefineAsStartState("q0");
            ndfa.DefineAsFinalState("q0");

            DFA<string> dfa = AutomataConverter.NDFAToDFA(ndfa);

            Graphiz<string> graphiz = new Graphiz<string>(dfa);
            graphiz.PrintGraph();

            Console.WriteLine("NDFA to DFA" );
        }

        static void TestToMinimalDFA()
        {
            char[] alphabet = { 'a', 'b' };

            NDFA<string> ndfa = new NDFA<string>(alphabet);
            ndfa.AddTransition(new Transition<string>("q1", 'a', "q2"));
            ndfa.AddTransition(new Transition<string>("q1", 'b', "q2"));
            ndfa.AddTransition(new Transition<string>("q1", 'b', "q3"));

            ndfa.AddTransition(new Transition<string>("q2", 'a', "q3"));
            ndfa.AddTransition(new Transition<string>("q2", "q4"));
            ndfa.AddTransition(new Transition<string>("q2", 'b', "q4"));

            ndfa.AddTransition(new Transition<string>("q3", 'a', "q2"));

            ndfa.AddTransition(new Transition<string>("q4", 'a', "q2"));
            ndfa.AddTransition(new Transition<string>("q4", 'a', "q5"));

            ndfa.AddTransition(new Transition<string>("q5", 'b', "q5"));
            ndfa.AddTransition(new Transition<string>("q5", "q3"));

            ndfa.DefineAsStartState("q1");
            ndfa.DefineAsFinalState("q2");
            ndfa.DefineAsFinalState("q3");

            DFA<string> dfa = AutomataConverter.NDFAToDFA(ndfa);

            Graphiz<string> graphiz = new Graphiz<string>(dfa);
            graphiz.PrintGraph("dfa1");

            DFA<string> dfa2 = AutomataConverter.ToMinimalDFA(dfa);

            Graphiz<string> graphiz2 = new Graphiz<string>(dfa2);
            graphiz.PrintGraph("dfa2");
        }
    }
}
