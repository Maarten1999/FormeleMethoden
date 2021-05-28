using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormeleMethodenCS
{
    class RegExp
    {
        public enum Operator { PLUS, STAR, OR, DOT, ONE }

        Operator @operator { get; set; }
        string Terminals { get; set; }

        public RegExp Left {get; set;}
        public RegExp Right { get; set; }

        public RegExp()
        {
            @operator = Operator.ONE;
        }

        public RegExp(string Terminals) : this()
        {
            this.Terminals = Terminals;
        }

        public RegExp Plus()
        {
            RegExp result = new RegExp();
            result.@operator = Operator.PLUS;
            result.Left = this;
            result.Terminals = this.Terminals + "+";

            return result;
        }
        public RegExp Star()
        {
            RegExp result = new RegExp();
            result.@operator = Operator.STAR;
            result.Left = this;
            result.Terminals = this.Terminals + "*";
            return result;
        }

        public RegExp Or(RegExp regExp)
        {
            RegExp result = new RegExp();
            result.@operator = Operator.OR;
            result.Left = this;
            result.Right = regExp;
            result.Terminals = this.Terminals + " | " + regExp.Terminals;
            return result;
        }

        public RegExp Dot(RegExp regExp)
        {
            RegExp result = new RegExp();
            result.@operator = Operator.DOT;
            result.Left = this;
            result.Right = regExp;
            result.Terminals = this.Terminals + " " + regExp.Terminals;
            return result;
        }

        public bool Accept(string s)
        {
            var set = GetLanguage(s.Length);
            return set.Contains(s);
        }
        public SortedSet<string> GetLanguage(int length)
        {
            SortedSet<string> emptyLanguage = new SortedSet<string>(new LengthComparer());
            SortedSet<string> languageResult = new SortedSet<string>(new LengthComparer());

            SortedSet<string> languageLeft, languageRight;

            if (length < 1) return emptyLanguage;
            
            switch (@operator)
            {
                case Operator.ONE:
                    languageResult.Add(Terminals);
                    goto case Operator.OR;
                case Operator.OR:
                    languageLeft = Left?.GetLanguage(length - 1) ?? emptyLanguage;
                    languageRight = Right?.GetLanguage(length - 1) ?? emptyLanguage;
                    languageResult.AddAll(languageLeft);
                    languageResult.AddAll(languageRight);
                    break;
                case Operator.STAR:
                case Operator.PLUS:
                    languageLeft = Left?.GetLanguage(length - 1) ?? emptyLanguage;
                    languageResult.AddAll(languageLeft);
                    for (int i = 1; i < length; i++)
                    {
                        HashSet<string> languageTemp = new HashSet<string>(languageResult);
                        foreach (string a in languageLeft)
                            foreach (string b in languageTemp)
                            {
                                languageResult.Add(a + b);
                            }
                        if (@operator == Operator.STAR)
                            languageResult.Add("");
                    }
                    break;
                case Operator.DOT:
                    languageLeft = Left?.GetLanguage(length - 1) ?? emptyLanguage;
                    languageRight = Right?.GetLanguage(length - 1) ?? emptyLanguage;

                    foreach (string a in languageLeft)
                        foreach (string b in languageRight)
                        {
                            languageResult.Add(a + b);
                        }
                    break;              
                default:
                    throw new NotImplementedException("Operator not implemented.");
            }
            return languageResult;
        }

        public override string ToString()
        {
            return Terminals;
        }
        private class LengthComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return (x.Length == y.Length) ? x.CompareTo(y) : x.Length - y.Length;
            }
        }
    }
}
