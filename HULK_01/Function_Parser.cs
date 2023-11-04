using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HULK_01
{
    internal class Function_Parser
    {
        internal static string FunctionParser (List<string> tokens, int idx)
        {
            //Se confirma que el nombre de la función empiece con una letra
            if (Char.IsLetter(tokens[idx][0]))
            {
                //Se verifica que el nombre de la función no sea una palabra reservada
                if (Native_Words.IsNativeWord(tokens[idx]))
                {
                    return "!LEXICAL ERROR " + tokens[idx] + " is not a valid name for a function";
                }

                //Se guarda el nombre de la función
                Parser.function_name.Add(tokens[idx]);
                tokens.RemoveAt(idx);

                //Se verifica que los argumentos de la función esté entre paréntesis
                if (tokens[idx][0] == '(')
                {
                    //Se analizan los argumentos dentro del paréntesis
                    string arguments = tokens[idx].Substring(1, tokens[idx].Length - 2);
                    arguments = arguments.Trim();
                    arguments = Function_Arguments.FunctionArguments(arguments);

                    tokens.RemoveAt(idx);

                    //Se verifica si hubo algún error
                    if (arguments[0] == '!') { return arguments; }

                    else
                    {
                        List<string> temp = new List<string>();
                        for (int i = 0; i < Function_Arguments.Arguments_List.Count; i++)
                        {
                            temp.Add(Function_Arguments.Arguments_List[i]);
                        }
                        //Se guarda la lista de argumentos en variables_function
                        Parser.variables_function.Add(temp);
                        Function_Arguments.Arguments_List.Clear();

                        //Se verifica la existencia del '=>'
                        if (tokens[idx] == "=" && tokens[idx + 1] == ">")
                        {
                            tokens.RemoveRange(idx, 2);

                            //Se guarda el cuerpo de la función en la lista
                            Parser.function_body.Add(tokens);

                            return "okay";
                        }
                        else { return "!SYNTAX ERROR missing '=>'"; }
                    }
                }
                else return "!SYNTAX ERROR missing open parenthesis";
            }
            else return "!LEXICAL ERROR " + tokens[idx] + " is not a valid name for a function";
        }
    }
}
