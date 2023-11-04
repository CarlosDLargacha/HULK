using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HULK_01
{
    internal class If_Else_Parser
    {
        internal static string IfElseParser (List<string> tokens, int idx)
        {
            //Se chequea que la instrucción esté entre paréntesis
            if (tokens[idx][0] == '(')
            {
                //Se recorren los tokens en busca de la palabra reservada 'else'
                for (int i = idx + 1; i <= tokens.Count; i++)
                {
                    if (tokens[i] == "else")
                    {
                        string tof = If_Else_Tokenizer.Condition(tokens[idx].Substring(1, tokens[idx].Length - 2));
                        //Si la condición es verdadera se parsea entre la condición y el 'else'
                        if (tof == "true")
                        {
                            tokens.RemoveRange(i, tokens.Count - i);
                            tokens.RemoveRange(0, 2);
                            return Parser.Begin_Parser(tokens, 0, 0);
                        }
                        //Si la condición es false se parse después del else
                        if (tof == "false")
                        {
                            tokens.RemoveRange(0, i + 1);
                            return Parser.Begin_Parser(tokens, 0, 0);
                        }
                        else return tof;
                    }
                    //Si el último token es 'else' se lanza un error
                    if (i == tokens.Count - 1 && tokens[i] == "else") { return "!SYNTAX ERROR missing code after 'else'"; }
                    //Si no hay nigún token 'else' se lanza un error
                    if (i == tokens.Count) { return "!SYNTAX ERROR missing 'else'"; }
                }
                return "Something is wrong";
            }
            else return "!SYNTAX ERROR missing condition";
        }
    }
}
