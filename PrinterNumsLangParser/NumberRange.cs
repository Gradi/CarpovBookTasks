using System;

namespace PrinterNumsLangParser
{
    public readonly struct NumberRange : IEquatable<NumberRange>
    {
        public readonly int Start;
        public readonly int End;

        public NumberRange(int start, int end)
        {
            Start = start;
            End = end;
        }

        public bool Equals(NumberRange other)
        {
            return Start == other.Start &&
                   End == other.End;
        }

        public override bool Equals(object obj) => obj is NumberRange other && Equals(other);

        public override int GetHashCode() => (Start, End).GetHashCode();

        public override string ToString()
        {
            if (Start == End)
            {
                return Start.ToString();
            }
            else
            {
                return $"{Start}-{End}";
            }
        }
    }
}
