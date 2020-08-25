using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormeleMethoden
{
    public class Transition<T> : IComparable<Transition<T>> where T : IComparable 
    {
        public static readonly char EPSILON = '$';

        public T SourceState { get; private set; }
        public T DestState { get; private set; }
        public char Symbol { get; private set; }
        public Transition(T sourceDest, char symbol) : this(sourceDest, symbol, sourceDest) { }

        public Transition(T source, T destination) : this(source, EPSILON, destination) { }
        public Transition(T source, char symbol, T destination)
        {
            this.SourceState = source;
            this.Symbol = symbol;
            this.DestState = destination;
        }


        public int CompareTo(Transition<T> other)
        {
            int sourceCompare = SourceState.CompareTo(other.SourceState);
            int symbolCompare = Symbol.CompareTo(other.Symbol);
            int destCompare = DestState.CompareTo(other.DestState);

            return (sourceCompare != 0 ? sourceCompare : (symbolCompare != 0 ? symbolCompare : destCompare));
        }

        public override string ToString()
        {
            return $"({SourceState}, {Symbol}) --> {DestState}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Transition<T> t = (Transition<T>)obj;  // CompareTo??
            return (this.SourceState.Equals(t.SourceState) && this.DestState.Equals(t.DestState)
                && this.Symbol == t.Symbol);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
