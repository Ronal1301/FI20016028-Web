using System;
using System.Linq;

namespace BinaryCalc.Models
{
    public static class BinaryCalculator
    {
        public static string PadTo8(string bin)
        {
            if (string.IsNullOrEmpty(bin)) return "00000000";
            if (bin.Length >= 8) return bin.PadLeft(8, '0').Substring(bin.Length - 8);
            return bin.PadLeft(8, '0');
        }

        private static (string aN, string bN) Normalize(string a, string b)
        {
            var max = Math.Max(a.Length, b.Length);
            return (a.PadLeft(max, '0'), b.PadLeft(max, '0'));
        }

        private static string TrimLeadingZeros(string bin)
        {
            var t = bin.TrimStart('0');
            return string.IsNullOrEmpty(t) ? "0" : t;
        }

        public static int BinToInt(string bin) => Convert.ToInt32(bin, 2);
        public static string ToBin(int n) => Convert.ToString(n, 2);
        public static string ToOct(int n) => Convert.ToString(n, 8);
        public static string ToDec(int n) => n.ToString();
        public static string ToHex(int n) => Convert.ToString(n, 16).ToUpper();

        public static (string bin, string oct, string dec, string hex) AllBases(int n)
        {
            return (TrimLeadingZeros(ToBin(n)), ToOct(n), ToDec(n), ToHex(n));
        }

        public static (string bin, string oct, string dec, string hex) AllBasesFromBin(string bin)
        {
            var n = BinToInt(bin);
            return (TrimLeadingZeros(bin), ToOct(n), ToDec(n), ToHex(n));
        }

        public static string And(string a, string b)
        {
            var (an, bn) = Normalize(a, b);
            var chars = an.Zip(bn, (x, y) => (x == '1' && y == '1') ? '1' : '0').ToArray();
            return TrimLeadingZeros(new string(chars));
        }

        public static string Or(string a, string b)
        {
            var (an, bn) = Normalize(a, b);
            var chars = an.Zip(bn, (x, y) => (x == '1' || y == '1') ? '1' : '0').ToArray();
            return TrimLeadingZeros(new string(chars));
        }

        public static string Xor(string a, string b)
        {
            var (an, bn) = Normalize(a, b);
            var chars = an.Zip(bn, (x, y) => (x != y) ? '1' : '0').ToArray();
            return TrimLeadingZeros(new string(chars));
        }

        public static int Add(string a, string b) => BinToInt(a) + BinToInt(b);
        public static int Mul(string a, string b) => BinToInt(a) * BinToInt(b);
    }
}
