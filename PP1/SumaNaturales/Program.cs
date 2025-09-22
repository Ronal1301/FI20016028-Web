using System;
using System.Text;

class Program
{
    const int Max = int.MaxValue;

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var (nForAsc,  sForAsc)  = FindAscendingLastValid(SumFor);
        var (nForDesc, sForDesc) = FindDescendingFirstValid(SumFor);

        var (nIteAsc,  sIteAsc)  = FindAscendingLastValid(SumIte);
        var (nIteDesc, sIteDesc) = FindDescendingFirstValidIterativeFast();

        Console.WriteLine();
        Console.WriteLine("• SumFor:");
        Console.WriteLine($"        ◦ From 1 to Max → n: {nForAsc} → sum: {sForAsc}");
        Console.WriteLine($"        ◦ From Max to 1 → n: {nForDesc} → sum: {sForDesc}");
        Console.WriteLine();
        Console.WriteLine("• SumIte:");
        Console.WriteLine($"        ◦ From 1 to Max → n: {nIteAsc} → sum: {sIteAsc}");
        Console.WriteLine($"        ◦ From Max to 1 → n: {nIteDesc} → sum: {sIteDesc}");
        Console.WriteLine();
    }

    static int SumFor(int n) => n * (n + 1) / 2;

    static int SumIte(int n)
    {
        int acc = 0;
        for (int i = 1; i <= n; i++)
            acc += i;
        return acc;
    }

    // de 1 a Max → último sum > 0
    static (int n, int sum) FindAscendingLastValid(Func<int, int> sumFunc)
    {
        int lastN = 0, lastSum = 0;

        for (int i = 1; i <= Max; i++)
        {
            int s = sumFunc(i);
            if (s > 0)
            {
                lastN = i;
                lastSum = s;
            }
            else
            {
                break;
            }
        }
        return (lastN, lastSum);
    }

    // de Max a 1 → primer sum > 0
    static (int n, int sum) FindDescendingFirstValid(Func<int, int> sumFunc)
    {
        for (int i = Max; i >= 1; i--)
        {
            int s = sumFunc(i);
            if (s > 0)
                return (i, s);
        }
        return (0, 0);
    }

    
    static int SumIteFastMod32(int n)
    {
       
        unchecked
        {
            ulong nn = (ulong)(uint)n;
            ulong sum = nn * (nn + 1UL) / 2UL;   
            uint mod32 = (uint)(sum & 0xFFFF_FFFFUL);
            return (int)mod32; 
        }
    }

    static (int n, int sum) FindDescendingFirstValidIterativeFast()
    {
        for (int i = Max; i >= 1; i--)
        {
            int s = SumIteFastMod32(i); 
            if (s > 0)
                return (i, s);
        }
        return (0, 0);
    }
}
