using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HULK_01
{
    internal class Function_Arguments
    {
        //Lista para guardar los argumentos de la función
        internal static List<string> Arguments_List = new List<string>();
        internal static string FunctionArguments (string arguments)
        {
            //
            string token_argument = "";

            //
            int number;

            //
            for (int i = 0; i <= arguments.Length; i++)
            {
                //Se verifica que el nombre del argumento no empiece con un número
                if (token_argument.Length == 1)
                {
                    if (int.TryParse(arguments, out number)) { return "!LEXICAL ERROR an argument token can not star with a number";  }
                }

                //Cuando se llega al final de la cadena se guarda la cadena
                if (i == arguments.Length)
                {
                   Arguments_List.Add(token_argument);

                   return "valid arguments";
                }

                //Si el carácter es un espacio vacíon se salta
                if (arguments[i] == ' ') { continue; }

                //Se verifica si el carácter es una coma
                if (arguments[i] == ',')
                {
                    if (i == 0 ||  i == arguments.Length - 1) { return "!SYNTAX ERROR missing argument"; }
                    Arguments_List.Add(token_argument);
                    token_argument = "";
                    continue;
                }

                //Se verifica si el carácter es especial
                if (especial_char_arguments(arguments[i]))
                {
                    return "!LEXICAL ERRROR '" + arguments[i] + "'is not a valid token";
                }

                token_argument += arguments[i];
            }

            return "Something is wrong with Arguments";
        }

        // Método para determinar si  un carácter es especial
        internal static bool especial_char_arguments(char ch)
        {
            switch (ch)
            {
                case '-': return true;
                case '+': return true;
                case '*': return true;
                case '^': return true;
                case '/': return true;
                case '%': return true;
                case ':': return true;
                case '?': return true;
                case '!': return true;
                case '=': return true;
                case '<': return true;
                case '>': return true;
                case '|': return true;
                case '(': return true;
                case ')': return true;
                case ';': return true;
                case '.': return true;
            }
            return false;
        }
    }
}
