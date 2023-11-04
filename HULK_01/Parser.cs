
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HULK_01
{
    internal class Parser
    {
        //Esta lista contiene los nombres de las funciones integradas
        internal static List<string> integrated_functions = new List<string>();

        //---------------------------------------------------------------------

        //En esta lista se guardan los nombres de las variables
        internal static List<string> let_in_variable = new List<string>();

        //En esta lista se guardan los valores de las variables 
        internal static List<string> let_in_value = new List<string>();

        //---------------------------------------------------------------------

        //Esta lista gurda los nombres de las funciones
        internal static List<string> function_name = new List<string>();

        //Lista que guarda las variables de una función
        internal static List<List<string>> variables_function = new List<List<string>>();

        //Lista que guarda el cuerpo de una función
        internal static List<List<string>> function_body = new List<List<string>>();

        //---------------------------------------------------------------------

        //En esta lista se guardan las variables de la función que se está parseando
        internal static List<string> function_parse_variable = new List<string>();

        //En esta lista se guardan los valores de la función que se está parseando
        internal static List<string> function_parse_values = new List<string>();

        internal static string Begin_Parser(List<string> tokens_parser, int index, int caller_id)
        {  
            double numericValue;
            //
            if(index >= tokens_parser.Count && tokens_parser.Count == 1) { return tokens_parser[0]; }

            //
            if(caller_id == 5 && index == tokens_parser.Count) { Let_In_Parser.comma_or_in = 3; return ""; }

            //Si el token es la palabra reservada 'let' se llama al método LetInParser
            if (tokens_parser[index] == "let")
            { 
                if (caller_id == 3) { return "!SYNTAX ERROR"; }

                //
                tokens_parser.Remove(tokens_parser[index]);

                //Se guarda en un string el resultado del let in
                string result = Let_In_Parser.LetInParser(tokens_parser, index);

                //Se limpian las listas con los nombres y valores de las variables
                Parser.let_in_variable.Remove(let_in_variable[let_in_variable.Count - 1]);
                Parser.let_in_value.Remove(let_in_value[let_in_value.Count - 1]);

                //Se devuelve el resultado
                return result;
            }
            else if (tokens_parser[index].Equals("let", StringComparison.OrdinalIgnoreCase)) return "let se escribe en minúsculas!";

            //Si el token es una variable se sustituye por su valor
            if (Parser.let_in_variable.Contains(tokens_parser[index]))
            {
                for(int i = 0; i < let_in_variable.Count; i++)
                {
                    if (tokens_parser[index] == let_in_variable[i]) { tokens_parser[index] = let_in_value[i]; break; }
                }
            }

            //Si el token es la palabra reservada 'print' se llama al método PrintParser
            if (tokens_parser[index] == "print")
            {
                if (caller_id == 5) { return "!SEMANTIC ERROR a variable can not be equal to 'print'"; }
                if (caller_id == 4) { return "!SYNTAX ERROR"; }
                if (caller_id == 3) { return "!SYNTAX ERROR"; }
                if (caller_id == 2) { return "!SEMANTIC ERROR print inside condition"; }
                if (caller_id == 1) { return "!SEMANTIC ERROR print inside print"; }

                string result = Print_Parser.PrintParser(tokens_parser, index);
               
                return result;
            }
            else if (tokens_parser[index].Equals("print", StringComparison.OrdinalIgnoreCase)) return "print se escribe en minúsculas!";

            //Si el token es la palabra reservada 'if' se llama al método IfElseParser
            if (tokens_parser[index] == "if") 
            {
                if (caller_id == 5) { return "!SEMANTIC ERROR a variable can not be equal to 'if'"; }
                if (caller_id == 3) { return "!SEMANTIC ERROR"; }
                if (caller_id == 1) { return "!SEMANTIC ERROR if inside 'print'"; }
                string result = If_Else_Parser.IfElseParser(tokens_parser, index + 1);
                return result;
            }

            //Si el token es la palabra reservada 'function' se llama al método FunctionParser
            if (tokens_parser[index] == "function")
            {
                if (caller_id == 5) { return "!SEMANTIC ERROR a variable can not be equal to 'if'"; }
                if (caller_id == 4) { return "!SEMANTIC ERROR a function can not be added to an string"; }
                if (caller_id == 3) { return "!SEMANTIC ERROR"; }
                if (caller_id == 2) { return "!SEMANTIC ERROR function inside condition"; }
                if (caller_id == 1) { return "!SEMANTIC ERROR function inside print"; }

                //Se remueve la palabra reservada 'function'
                tokens_parser.RemoveAt(index);

                //Se llama al método para parsear la función
                string result = Function_Parser.FunctionParser(tokens_parser, index);

                if (result[0] == '!') { return result; }
                return "Function Declared"; 
            }

            //Si el token es un nombre de función
            if (function_name.Contains(tokens_parser[index]))
            {
                int idx = - 1;

                for (int i = 0; i < function_name.Count; i++)
                {
                    if (function_name[i] == tokens_parser[index]) { idx = i; break; }   
                }

                //Se remueve el nombre de la función de la lista de tokens
                tokens_parser.RemoveAt(index);

                //Se verifica que existen los argumentos de la función
                if (tokens_parser[index][0] == '(')
                {
                    string error = tokens_parser[index].Substring(1, tokens_parser[index].Length - 2);
                    error = error.Trim();
                    error = Assign_Var_Value.AssignVarValue(error, idx);
                    if (error[0] == '!') { return error; }

                    for (int i = 0; i < variables_function[idx].Count; i++)
                    {
                        function_parse_variable.Add(variables_function[idx][i]);
                        function_parse_values.Add(Assign_Var_Value.Value_List[i]);
                    }
                    Assign_Var_Value.Value_List.Clear();

                    List<string> temp = new List<string>();

                    for (int i = 0; i < function_body[idx].Count; i++)
                    {
                        temp.Add(function_body[idx][i]);
                    }

                    tokens_parser[index] = Begin_Parser(temp, 0, 0);

                    if (tokens_parser[index][0] == '!') { return tokens_parser[index]; }

                    function_parse_values.RemoveAt(function_parse_values.Count-1);

                    function_parse_variable.RemoveAt(function_parse_variable.Count-1);
                }

                else return "!SYNTAX ERROR missing arguments of function '" + function_name[idx] + "'";
            }

            if (Parser.function_parse_variable.Contains(tokens_parser[index]))
            {
                for (int i = function_parse_variable.Count-1; i >= 0; i--)
                {
                    if (tokens_parser[index] == function_parse_variable[i]) { tokens_parser[index] = function_parse_values[i]; break; }
                }
            }

            //Si el token es '@' se procede a concatenar los strings
            if (tokens_parser[index] == "@")
            {
                //Se verifica que el '@' sea el segundo token
                if (index != 1) { return "!SYNTAX ERROR two strings needed"; }

                //Se guarda la cadena a la izquierda del '@'
                string left = tokens_parser[0];

                //Se remueven la cadena izquierda y el '@'
                tokens_parser.Remove(tokens_parser[1]);
                tokens_parser.Remove(tokens_parser[0]);
                 
                //Se le suma a la cadena izquierda la cadena que está a la derecha del @
                return left + Begin_Parser(tokens_parser, 0, 4);
            }

            //Si el token es una cadena se parsea
            if (tokens_parser[index][0] == '"') 
            {
                if (caller_id == 3) { return "!SYNTAX ERROR"; }
                tokens_parser[index] = tokens_parser[index].Substring(1, tokens_parser[index].Length - 2);
                if (caller_id == 5) { return Begin_Parser(tokens_parser, index + 1, 5); }
                return Begin_Parser(tokens_parser, index + 1, 3);
            }

            //Si hay un token encerrado entre paréntesis, se cambia el token por el valor de la expresión aritmética dentro de los paréntesis
            if (tokens_parser[index][0] == '(')
            {
                string tokens = tokens_parser[index].Substring(1, tokens_parser[index].Length - 2);
                tokens_parser[index] = Tokenizer.tokenizer(tokens, 0);
            }

            //Se verifica si el token es la palabra reservada 'in'
            if (tokens_parser[index] == "in")
            {
                if (caller_id == 4) { return "!SEMANTIC ERROR missing 'let'"; }
                if (caller_id == 3) { return "!SYNTAX ERROR"; }
                if (caller_id == 2) { return "!SEMANTIC ERROR 'in' inside condition"; }
                if (caller_id == 1) { return "!SEMANTIC ERROR 'in' inside print"; }
                if (caller_id == 0) { return "!SEMANTIC ERROR missing 'let'"; }

                //Se verifica si el 'in' es el último token
                if (index == tokens_parser.Count - 1) { Let_In_Parser.comma_or_in = 3;  return ""; }

                //Se guarda en una lista el valor de la variable
                List<string> variable = new List<string>();

                for (int i = 0; i < index; i++)
                {
                    variable.Add(tokens_parser[i]);
                }
                tokens_parser.RemoveRange(0, index);

                Let_In_Parser.comma_or_in = 1;

                //Se devuelve el valor de la variable
                if (variable.Count == 1) { return variable[0]; }
                return Check_Math_Expression.Check(variable, 0, 0);
            }

            if (tokens_parser[index] == ",")
            {
                if (caller_id == 4) { return "!SEMANTIC ERROR ',' after string"; }
                if (caller_id == 3) { return "!SYNTAX ERROR"; }
                if (caller_id == 2) { return "!SEMANTIC ERROR ',' inside condition"; }
                if (caller_id == 1) { return "!SEMANTIC ERROR ',' inside print"; }
                if (caller_id == 0) { return "!SYNTAX ERROR ',' invalid token;"; }

                //Se verifica si ',' es el último token
                if (index == tokens_parser.Count - 1) { Let_In_Parser.comma_or_in = 3; return ""; }

                //Se guarda en una lista el valor de la variable
                List<string> variable = new List<string>();

                for (int i = 0; i < index; i++)
                {
                    variable.Add(tokens_parser[i]);
                }
                tokens_parser.RemoveRange(0, index);

                Let_In_Parser.comma_or_in = 2;

                //Se devuelve el valor de la variable
                if (variable.Count == 1) { return variable[0]; }
                return Check_Math_Expression.Check(variable, 0, 0);
            }

                //Se verifica si el token es un número o un operador
                if (double.TryParse(tokens_parser[index], out numericValue) == false ) 
            {
               if( tokens_parser[index] != "+" && tokens_parser[index] != "-" && tokens_parser[index] != "*" && tokens_parser[index] != "/" && tokens_parser[index] != "%" && tokens_parser[index] != "^" && tokens_parser[index] != "(" && tokens_parser[index] != ")")              
                    { return "!LEXICAL ERROR: " + tokens_parser[index] + " is not a valid token"; }
            }

            //Si se llega al último token se revisa si es una operación aritmética viable
            if (index == tokens_parser.Count - 1)
            {
                return Check_Math_Expression.Check(tokens_parser, 0, 0); 
            }

            else//Se pasa a analizar el siguiente token
            {
                if (caller_id == 5) { return Begin_Parser(tokens_parser, index + 1, 5); }
                return Begin_Parser (tokens_parser, index + 1, 0);
            }
        }

        // Método para chequear que un string es lowercase
        bool IsAllLowerCase(string teststring)
        {
            for (int i = 0; i < teststring.Length; i++)
            {
                if (!Char.IsLower(teststring[i]))
                    return false;
            }
            return true;
        }
    }
}
