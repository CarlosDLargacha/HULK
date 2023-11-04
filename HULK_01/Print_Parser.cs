using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HULK_01
{
    internal class Print_Parser
    {
        internal static string PrintParser(List<string> tokens, int idx)
        {
            //Verificar que después del print haya un token con paréntesis
            if (tokens[idx + 1][0] == '(') 
            { 
                //Obtener la subcadena sin los paréntesis externos
                string to_print = tokens[idx + 1].Substring(1, tokens[idx + 1].Length - 2);

                //Tokenizar la subcadena dentro del paréntesis
                return Tokenizer.tokenizer(to_print, 1);
            }
            else return "!SYNTAX ERROR missing parenthesis";
        }
    }
}
