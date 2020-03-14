using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdvancedCSharp_Delegate_Extensions
{
    class Program
    {
        delegate double MyFirstDelegate(int r);
        static void Main(string[] args)
        {/*Comments

            // Step 1 using normal function
            MyFirstDelegate myFirstDelegate = CalculateAreaOfCircle;
            Console.WriteLine("Normal Method :"+myFirstDelegate.Invoke(10));

            // Step 2 using Anonymous functions
            MyFirstDelegate anonymousFunction = new MyFirstDelegate(
                delegate(int r) {
                    return 3.14 * r * r;
                });
            Console.WriteLine("Anonymous Function :" + anonymousFunction(10));

            // Step 3 Using Lambda expression uses of Lambda E=> Expression Trees Businesslanguage & Linq
            MyFirstDelegate useLambdaExp = new MyFirstDelegate(r=>3.14*r*r);
            Console.WriteLine("Lambda Expression :" + useLambdaExp(10));

            // USE OF GENERIC DELEGATE
            // a. Func
            Func<int,double> funcGenericDel = r => 3.14 * r * r;
            Console.WriteLine("Generic Delegate Func: "+funcGenericDel(10));

            // b. Predicate take any parameter and return boolean
            Predicate<string> predicateGenericDel = s => s.Length < 5;
            Console.WriteLine("Predicate Generic Delegate: "+predicateGenericDel("Mwangi"));

            // c. Action used for void functions
            Action<string> printName = name => Console.WriteLine(name);
            Console.WriteLine("Generic Delegate Action: ");
            printName(" Mwangi");
            */

            // Others 
            Func<int, int> square = l => l * l;
            Func<int, int> slowSquare = l =>
            {
                Thread.Sleep(100);
                return l * l;
            };



            //Measure(() => square(2));        // 00:00:00.0001388
            Measure(() => slowSquare(2));    // 00:00:00.1006509
            Measure(() => slowSquare(2));    // 00:00:00.1006509
            Measure(() => slowSquare(2));    // 00:00:00.1006509
            var numbers = Enumerable.Range(1, 10);
            Measure(() => numbers.Select(square).ToList());      // 00:00:00.0079551
            Measure(() => numbers.Select(slowSquare).ToList());  // 00:00:01.0024892
            Console.WriteLine("Use the momize function");
            var memoizedSlow = ExtensionClass.Memoize(slowSquare);
            Measure(() => memoizedSlow(2)); // 00:00:00.1070149
            Measure(() => memoizedSlow(2)); // 00:00:00.0005227
            Measure(() => memoizedSlow(2)); // 00:00:00.0004159
        }

        static double CalculateAreaOfCircle(int r)
        {
            return 3.14 * r * r;
        }

        static void Measure(Action action)
        {
            var sw = new Stopwatch();
            sw.Start();
            action();
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }


    }

    public static class ExtensionClass
    {
        public static Func<T, TResult> Memoize<T, TResult>(this Func<T, TResult> f)
        {
            var cache = new ConcurrentDictionary<T, TResult>();
            return result => cache.GetOrAdd(result, f);
        }
    }
}
