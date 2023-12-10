using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace HULK_01
{
    internal class If_Else_Tokenizer
    {
        internal static string Condition(string condition)
        {
            double number;

            //Se crean los string donde se guardan los miembros a comparar
            string left = "";
            string right = "";

            //Se recorre la cadena para encontrar el comparador
            for (int i = condition.Length - 1; i >= 0; i--)
            {
                if (condition[i] == '=')
                {
                    if (condition[i - 1] == '>')
                    {
                        left = Tokenizer.tokenizer(condition.Substring(0, i - 1), 2);
                        right = Tokenizer.tokenizer(condition.Substring(i + 1, condition.Length - 1 -i), 2);

                        //Se confirma que ambos miembros son números
                        if (double.TryParse(left, out number) && double.TryParse(left, out number))
                        {
                            double left_number = double.Parse(left);
                            double right_number = double.Parse(right);
                            if (left_number >= right_number) { return "true"; }
                            else { return "false"; }
                        }
                        else return "SYNTAX ERROR strings can't be compared";
                    }
                    if (condition[i - 1] == '<')
                    {
                        left = Tokenizer.tokenizer(condition.Substring(0, i - 1), 2);
                        right = Tokenizer.tokenizer(condition.Substring(i + 1, condition.Length - 1 -i), 2);

                        //Se confirma que ambos miembros son números
                        if (double.TryParse(left, out number) && double.TryParse(left, out number))
                        {
                            double left_number = double.Parse(left);
                            double right_number = double.Parse(right);
                            if (left_number <= right_number) { return "true"; }
                            else { return "false"; }
                        }
                        else return "SYNTAX ERROR strings can't be compared";
                    }
                    if (condition[i - 1] == '!')
                    {
                        left = Tokenizer.tokenizer(condition.Substring(0, i - 1), 2);
                        right = Tokenizer.tokenizer(condition.Substring(i + 1, condition.Length - 1 - i), 2);
                        if (left != right) { return "true"; }
                        else { return "false"; }
                    }
                    if (condition[i - 1] == '=')
                    {
                        left = Tokenizer.tokenizer(condition.Substring(0, i - 1), 2);
                        right = Tokenizer.tokenizer(condition.Substring(i + 1, condition.Length - 1 - i), 2);
                        if (left == right) { return "true"; }
                        else { return "false"; }
                    }
                }
                if (condition[i] == '>')
                {
                    left = Tokenizer.tokenizer(condition.Substring(0, i - 1), 2);
                    right = Tokenizer.tokenizer(condition.Substring(i + 1, condition.Length - i - 1), 2);

                    //Se confirma que ambos miembros son números
                    if (double.TryParse(left, out number) && double.TryParse(left, out number))
                    {
                        double left_number = double.Parse(left);
                        double right_number = double.Parse(right);
                        if (left_number > right_number) { return "true"; }
                        else { return "false"; }
                    }
                    else return "SYNTAX ERROR strings can't be compared";
                }
                if (condition[i] == '<')
                {
                    left = Tokenizer.tokenizer(condition.Substring(0, i - 1), 2);
                    right = Tokenizer.tokenizer(condition.Substring(i + 1, condition.Length - i - 1), 2);

                    //Se confirma que ambos miembros son números
                    if (double.TryParse(left, out number) && double.TryParse(left, out number))
                    {
                        double left_number = double.Parse(left);
                        double right_number = double.Parse(right);
                        if (left_number < right_number) { return "true"; }
                        else { return "false"; }
                    }
                    else return "SYNTAX ERROR strings can't be compared";
                }
            }
            return "SYNTAX ERROR";
        } 
    }
}
