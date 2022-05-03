using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MathCalculator
{
    class Program
    {

        struct Leksema
        {
            public string Type;
            public double Value;
        }

        static int GetRank(string op)
        {
            if (op == "+" || op == "-")
                return 1;
            if (op == "*" || op == "/")
                return 2;
            if (op == "^")
                return 3;
            return 0;
        }
        static string[] unaryMinus(string[] result)
        {
            string[] res = new string[result.Length - 2];
            res[0] = result[0] + result[1] + result[2];
            int i = 3;
            int j = 1;
            while (j != res.Length && i != result.Length)
            {
                res[j] = result[i];
                i++;
                j++;
            }
            return res;
        }



        static void Calc(ref Stack<Leksema> n, ref Stack<Leksema> o, ref Leksema item)
        {
            double a;
            double b;
            double c;
            a = n.Pop().Value;
            switch (o.Peek().Type)
            {
                case "+":
                    {
                        b = n.Pop().Value;
                        c = a + b;
                        Console.WriteLine("Результат сложения= {0}", c);
                        item.Type = "0";
                        item.Value = c;
                        n.Push(item);
                        o.Pop();
                        break;
                    }
                case "-":
                    {
                        b = n.Pop().Value;
                        c = b - a;
                        Console.WriteLine("Результат вычитания= {0}", c);
                        item.Type = "0";
                        item.Value = c;
                        n.Push(item);
                        o.Pop();
                        break;
                    }
                case "*":
                    {
                        b = n.Pop().Value;
                        c = a * b;
                        Console.WriteLine("Результат умножения= {0}", c);
                        item.Type = "0";
                        item.Value = c;
                        n.Push(item);
                        o.Pop();
                        break;
                    }
                case "/":
                    {
                        b = n.Pop().Value;
                        c = b / a;
                        Console.WriteLine("Результат деления= {0}", c);
                        item.Type = "0";
                        item.Value = c;
                        n.Push(item);
                        o.Pop();
                        break;
                    }
                case "^":
                    {
                        b = n.Pop().Value;
                        c = Math.Pow(b, a);
                        Console.WriteLine("Результат деления= {0}", c);
                        item.Type = "0";
                        item.Value = c;
                        n.Push(item);
                        o.Pop();
                        break;
                    }
            }

        }

        static void WriteStacks(Stack<Leksema> numbers, Stack<Leksema> opers)
        {

            if (numbers.Count > 0 && opers.Count > 0)
            {
                Console.WriteLine("*************************************************************************************************************");
                foreach (var n in numbers)
                    Console.WriteLine(n.Value);
                Console.WriteLine("***************************************************");
                foreach (var op in opers)
                    Console.WriteLine(op.Type);

                Console.WriteLine("*************************************************************************************************************");
            }
        }

        static void Main(string[] args)
        {
            Leksema item = new Leksema();

            Stack<Leksema> numbers = new Stack<Leksema>();

            Stack<Leksema> opers = new Stack<Leksema>();

            string expression = Console.ReadLine();

            //int len = expression.Length;

            string[] result = Regex.Split(expression, @"([*()\^\/]|(?<!E)[\+\-])");

            if (result[0] == "" && result[1] == "-")
            {
                result = unaryMinus(result);
            }

            Console.WriteLine("Длина разбитой на подстроки строки: " + result.Length);

            foreach (string str in result)
                Console.WriteLine(str);


            for (int i = 0; i < result.Length; i++)
            {
                if (Regex.IsMatch(result[i], @"^-?[0-9]|[0-9,\.]+$"))
                {
                    item.Type = "0";
                    item.Value = Convert.ToDouble(result[i]);
                    numbers.Push(item);
                    continue;
                }
                if (result[i] == "+" || result[i] == "-" || result[i] == "*"  || result[i] == "/" || result[i] == "^")
                {
                    if (opers.Count == 0)
                    {
                        item.Type = result[i];
                        item.Value = 0;
                        opers.Push(item);
                        continue;
                    }
                    if (opers.Count != 0 && (GetRank(result[i]) > GetRank(opers.Peek().Type)))
                    {
                        item.Type = result[i];
                        item.Value = 0;
                        opers.Push(item);
                        continue;
                    }
                    if (opers.Count != 0 && (GetRank(result[i]) <= GetRank(opers.Peek().Type)))
                    {
                        Calc(ref numbers, ref opers, ref item);
                        item.Type = result[i];
                        item.Value = 0;
                        opers.Push(item);
                        continue;
                    }
                }
                if (result[i] == "(")
                {
                    item.Type = result[i];
                    item.Value = 0;
                    opers.Push(item);
                    continue;
                }
                if(result[i]== ")")
                {
                    while(opers.Peek().Type != "(")
                    {
                        Calc(ref numbers, ref opers, ref item);
                        continue;  
                    }
                    opers.Pop();
                    continue;
                }
            }

            while (opers.Count != 0)
            {
                Calc(ref numbers, ref opers, ref item);
            }


            if (numbers.Count >= 0)
            {
                Console.WriteLine("Ответ: {0}", numbers.Peek().Value);
            }

            Console.ReadLine();

        }
    }
}
