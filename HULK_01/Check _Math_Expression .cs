using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HULK_01
{
    internal class Check_Math_Expression
    {
        //Este método determina si una expresión aritmética es viable analizando token por token
        internal static string Check(List<string> tokens, int index, int count)
        {
            double numericValue;
            
            //Si los paréntesis están balanciados se llama al método para calcular la expresión 
            if (index == tokens.Count)
            {
                return Arithmetic.Shunting_Yard(tokens);
            }

            //Si el token es un número se garantiza que esté entre operadores 
            if (double.TryParse(tokens[index], out numericValue) == true)
            {
                if (index > 0)
                {
                    if (double.TryParse(tokens[index - 1], out numericValue) == true || tokens[index - 1] == ")") { return "!SYNTAX ERROR: missing operator"; }
                }
                if (index < tokens.Count - 1)
                {
                    if (double.TryParse(tokens[index + 1], out numericValue) == true || tokens[index + 1] == "(") { return "!SYNTAX ERROR: missing operator"; }
                }
            }

            //Se garantiza que los operadores estén entre tokens numéricos
            if (tokens[index] == "+" || tokens[index] == "*" || tokens[index] == "/" || tokens[index] == "%" || tokens[index] =="^") 
            {
                if (index == 0 || index == tokens.Count - 1) { return "!SYNTAX ERROR: missing number"; }
                else 
                {
                    if (double.TryParse(tokens[index - 1], out numericValue) == false && tokens[index - 1] != ")") { return "!SYNTAX ERROR: string can not be operated with a number"; }
                    if (double.TryParse(tokens[index + 1], out numericValue) == false && tokens[index + 1] != "(") { return "!SYNTAX ERROR: string can not be operated with a number"; }
                }
            }
            if (tokens[index] == "-")
            {
                if (index == tokens.Count - 1) { return "!SYNTAX ERROR: missing number after (-) operator"; }
                else
                {
                    if (index > 0) { if (double.TryParse(tokens[index - 1], out numericValue) == false && tokens[index - 1] != ")" && tokens[index - 1] != "(") { return "!SYNTAX ERROR: string can not be operated with a number"; } }
                    if (double.TryParse(tokens[index + 1], out numericValue) == false && tokens[index + 1] != "(") { return "!SYNTAX ERROR: string can not be operated with a number"; }
                }
            }

            //Se vuelve a llamar al método check y se analiza el siguiente token
            return Check ( tokens, index + 1 , count);
        }
    }
}