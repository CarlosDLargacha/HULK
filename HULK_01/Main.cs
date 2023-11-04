using HULK_01;
using System.ComponentModel;

while (true)
{
    Console.Write('>');
    string input = Console.ReadLine();
    if (input[input.Length - 1] == ';')
    {
        input = input.Substring(0, input.Length - 1);
        Console.WriteLine(Tokenizer.tokenizer(input, 0));
    }
    else { Console.WriteLine("Syntax ERROR missing ;"); }
}


