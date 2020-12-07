using System;
using System.IO;
using System.Numerics;

namespace AoC2020.testTrampoline
{
    internal static class Main
    {
        public static void Solve()
        {
            Console.WriteLine("Test Trampoline [https://thomaslevesque.com/2011/09/02/tail-recursion-in-c/]:");

            //var result = Factorial(7500);
            //var result = Factorial2(6500, 1);

            Func<int, BigInteger, BigInteger> fact = Trampoline.MakeTrampoline<int, BigInteger, BigInteger>(Factorial3);
            BigInteger result = fact(500000, 1); // Took about 10 minutes to calculate, but not Stack Overflow!
            
            Console.WriteLine(result);

        }

        private static BigInteger Factorial(int n)
        {
            if (n < 2)
                return 1;
            return n * Factorial(n - 1);
        }

        static BigInteger Factorial2(int n, BigInteger product)
        {
            if (n < 2)
                return product;
            return Factorial2(n - 1, n * product);
        }

        static Bounce<int, BigInteger, BigInteger> Factorial3(int n, BigInteger product)
        {
            if (n < 2)
                return Trampoline.ReturnResult<int, BigInteger, BigInteger>(product);
            return Trampoline.Recurse<int, BigInteger, BigInteger>(n - 1, n * product);
        }
    }
    
    public static class Trampoline
    {
        public static Func<T1,T2,TResult> MakeTrampoline<T1,T2,TResult>(Func<T1,T2,Bounce<T1,T2, TResult>> function)
        {
            Func<T1,T2,TResult> trampolined = (T1 arg1, T2 arg2) =>
            {
                T1 currentArg1 = arg1;
                T2 currentArg2 = arg2;

                while (true)
                {
                    Bounce<T1,T2, TResult> result = function(currentArg1, currentArg2);

                    if (result.HasResult)
                    {
                        return result.Result;
                    }
                    else
                    {
                        currentArg1 = result.Param1;
                        currentArg2 = result.Param2;
                    }
                }
            };

            return trampolined;
        }


        public static Bounce<T1,T2, TResult> Recurse<T1,T2, TResult>(T1 arg1, T2 arg2)
        {
            return new Bounce<T1,T2, TResult>(arg1, arg2);
        }

        public static Bounce<T1,T2, TResult> ReturnResult<T1,T2, TResult>(TResult result)
        {
            return new Bounce<T1,T2, TResult>(result);
        }

    }


    public struct Bounce<T1,T2, TResult>
    {
        public T1 Param1 {get; private set;}
        public T2 Param2 { get; private set; }

        public TResult Result { get; private set; }
        public bool HasResult { get; private set; }
        public bool Recurse { get; private set; }

        public Bounce(T1 param1, T2 param2) : this()
        {
            Param1 = param1;
            Param2 = param2;
            HasResult = false;

            Recurse = true;

        }

        public Bounce(TResult result) : this()
        {
            Result = result;
            HasResult = true;

            Recurse = false;

        }
    }
}