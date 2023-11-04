using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace HULK_01
{
    class Let_In_Parser
    {
        internal static int comma_or_in = 0;
        internal static int position = 0;
        internal static string LetInParser(List<string> tokens, int index)
        {
            //Este método se encarga de guardar los nombres y los valores de las variables 
            string AssignVariable (List<string> tokens, int index)
            {
                //Se verifica que el nombre de la variables es válido
                if (Char.IsLetter(tokens[index][0]) == false) { return LetInError(10); }
                if (Native_Words.IsNativeWord(tokens[index])) { return LetInError(11); }
                if (Parser.function_name.Contains(tokens[index])) { return LetInError(12); }

                //Si el nombre de la variable es válido se continua con el método
                else
                {
                    //Se verifica la existencia del '=' después del nombre de la variable
                    if (tokens[index+1] != "=") { return LetInError(20); }
                    else
                    {
                        //Se verifica la existencia de la palabra reservada 'in'
                        if (tokens.Contains("in"))
                        {
                            string x = tokens[index];
                            tokens.RemoveRange(index, 2);

                            //Se guarda el valor de la variable en la lista let_in_value
                            Parser.let_in_value.Add(Parser.Begin_Parser(tokens, index, 5));

                            //Se guarda el nombre de la variable en la lista 'let_in_variable'
                            Parser.let_in_variable.Add(x);

                            //
                            tokens.RemoveAt(0);

                            //
                            if (comma_or_in == 1) { return LetInParser(tokens, index); }
                            if (comma_or_in == 2) { return AssignVariable(tokens, index); }
                            if (comma_or_in == 3) { return "!SYNTAX ERROR missing 'in'"; }
                            else { return "Something is Wrong"; }
                        }
                        else { return  LetInError(30); }
                    }
                }
            }

            return AssignVariable(tokens, index);

            string LetInParser(List<string> tokens, int index)
            {
                return Parser.Begin_Parser(tokens, 0, 0);
            }
            
            //Este método devuelve los errores en el 'let in'
            string LetInError (int error)
            {
                return "";
            }
            return "";           
        }
    }
}
