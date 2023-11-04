using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;

namespace HULK_01
{
    internal class Tokenizer
    {        
        //Método que crea los tokens a partir de la entrada en consola
        internal static string tokenizer(string input, int caller_id)
        {
            
            //Lista en la que se guardan los token de la entrada
            List<string> tokens = new List<string>();

            string token = "";
            int error = 0;

            //ciclo para analizar la entrada carácter a carácter y obtener los tokens de la entrada
            for (int i = 0; i <= input.Length; i++)
            {
                //Al llegar al final de la entrada se guarda el token
                if (i == input.Length)
                {
                    if (token != "") { tokens.Add(token); }
                    break;
                }

                //Si el carácter son comillas significa el comienzo de un string
                if (Is_Quotes(input[i]))
                {
                    if (token != "") { tokens.Add(token); token = "";}
                    token += input[i];
                    i = GetQuotedChain(i, input, ref token, ref error);
                    if (error == 1) { return "!SYNTAX ERRROR missing quotes"; }
                    else continue;
                }

                // Si el carácter es paréntesis voy a salvar todo hasta el cierre del mismo
                if (begin_parenthesis(input[i]))
                {
                    if (token != "") { tokens.Add(token); token = "";}
                    token += input[i];
                    i = GetParenthesisChain(i, input, ref token, ref error);
                    if (error == 2) { return "!SYNTAX ERROR missing closing paréntesis"; }
                    else continue;
                }

                // Si el carácter es cierre de paréntesis nunca vi el de apertura y doy error de sintaxis
                if (end_parenthesis(input[i]))
                {
                    return "!SYNTAX ERROR missing opening paréntesis"; 
                }

                //Si el carácter es especial, añadir el token anterior y este token a la lista
                if (especial_char(input[i]))
                {
                    if (token != "") { tokens.Add(token); token = ""; }
                    tokens.Add(input[i].ToString());
                    continue;
                }

                //Si el carácter es un espacio se omite
                if (input[i] == ' ')
                {
                    if (token != "") { tokens.Add(token); token = ""; }
                }
                else { token += input[i]; }       
            }

            //Ciclo para formar los números decimales
            long longValue;
            for (int i = 1; i < tokens.Count - 1; i++)
            {
                if (tokens[i] == "." && long.TryParse(tokens[i - 1], out longValue) == true && long.TryParse(tokens[i - 1], out longValue) == true)
                {
                    tokens[i] = tokens[i - 1] + tokens[i] + tokens[i + 1];
                    tokens.Remove(tokens[i + 1]);
                    tokens.Remove(tokens[i - 1]);
                }
            }

            //Ciclo para formar los números negativos
            for (int i = 0; i < tokens.Count - 1; i++)
            {
                if (tokens[i] == "-")
                {
                    if (i == 0) { tokens[i + 1] = "-" + tokens[i + 1]; tokens.Remove(tokens[i]); }
                    else
                    {
                        if (tokens[i - 1] == "(") { tokens[i + 1] = "-" + tokens[i + 1]; tokens.Remove(tokens[i]); }
                    }
                }
                else continue;
            }

            return Parser.Begin_Parser(tokens, 0, caller_id);
        }

        // Método para determinar si  un carácter es especial
        internal static bool especial_char(char ch)
        { 
            switch (ch)
            {
                case ',': return true;
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
            }
            return false;
        }

        //Método para detectar si el carácter es comillas
       internal static bool Is_Quotes(char ch) { if (ch == '"') return true; else return false; }

        //Método para detectar si el carácter es paréntesis inicio
        internal static bool begin_parenthesis(char ch) { if (ch == '(') return true; else return false; }

        //Método para detectar si el carácter es paréntesis final
        internal static bool end_parenthesis(char ch) { if (ch == ')') return true; else return false; }

        // Function to capture all between quotes
        internal static int GetQuotedChain(int index, string chain, ref string token, ref int error)
        {

            int idx = 0;
            for (int j = index + 1; j <= chain.Length; j++)
            {
                if (j == chain.Length) { error = 1; break; }
                else if (chain[j] == '"' && chain[j - 1] != '/') { token += chain[j]; idx = j; break; }
                else { token += chain[j]; idx = j;}
            }
            return idx;
        }

        //Function to capture all between parenthesis
        internal static int GetParenthesisChain(int index, string chain, ref string token, ref int error)
        {            
            int idx = 0;
            int counter = 1;
            for (int j = index + 1; j <= chain.Length; j++)
            {
                if (j == chain.Length) { error = 2; break; }
                else if (begin_parenthesis(chain[j]))
                {
                    counter++;
                    token += chain[j];
                    idx = j;
                }
                else if (end_parenthesis(chain[j]))
                {
                    counter--;
                    token += chain[j];
                    idx = j;
                    if (counter == 0) { break; }
                }
                else if (Is_Quotes(chain[j])) 
                { 
                    // Token con valor desde apertura de paréntesis
                    token += chain[j];
                    j = GetQuotedChain(j, chain, ref token, ref error); 
                    if (error == 1) { break; }
                    else continue;
                }
                else { token += chain[j]; }
            }
            return idx;
        }   
    }
}
