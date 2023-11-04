using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace HULK_01
{
    internal class Arithmetic
    {
        internal static string Shunting_Yard (List<string> tokens)
        {
            double numericValue;
            //Pila la guardar los valores numericos
            Stack<string> west = new Stack<string>();
            //Pila para guardar los operadores
            Stack<string> south = new Stack<string>();

            //Este método se encargara de enviar los números a la pila west y los operadores a la pila south
            string Aux(List<string> tokens, int index)
            {
                //Cuando ya se recorrieron todos los token se realizan las operaciones restantes y se devuelve el resultado final
                if (index == tokens.Count) 
                {
                    return "" + Final_Operations(double.Parse(west.Pop()));
                }
                //Si el token es un número se envía a la pila west
                if (double.TryParse(tokens[index], out numericValue) == true) { west.Push(tokens[index]); return Aux(tokens, index + 1); }

                else 
                {
                    //Si hay algún operador en la fila se compara con el operador a insertar
                    if (south.Count > 0)
                    {
                        //Antes de entrar un nuevo operador a la fila se confirma que su prioridad sea menor o igual que la del anterior
                        if (Priority(tokens, index, south.Peek()) == true)
                        {
                            string symbol = south.Pop();
                            double number_2 = double.Parse(west.Pop());
                            double number_1 = double.Parse(west.Pop());
                            west.Push(Calculator(symbol, number_1, number_2));
                            south.Push(tokens[index]);
                        }
                        //Si la prioridad es mayor se inserta el operador en la pila
                        else south.Push(tokens[index]);
                    }
                    //Si la pila no tiene ningún elemento se inserta el operador
                    else south.Push(tokens[index]);
                }
                //Se vuelve a llamar al método para analizar el siguiente token
                return Aux(tokens, index + 1);
            }
            
            //Método que determina la prioridad entre dos operadores
            bool Priority (List<string> tokens, int index, string previows_op) 
            {
                if (tokens[index] == "+" || tokens[index] == "-") { if (previows_op != "(") { return true; } else return false; }
                if (tokens[index] == "*" || tokens[index] == "/" || tokens[index] == "%")
                {
                    if ( previows_op != "+" && previows_op != "-" && previows_op != "(") { return true; }
                    else return false;
                }
                else return false;
            }

            //Este método se encarga en hacer las operaciones entre dos números
            string Calculator (string symbol, double number_1, double number_2)
            {
                double result = 0;
                switch (symbol)
                {
                    case "+": 
                        result = number_1 + number_2;
                        break;
                    case "-":
                        result = number_1 - number_2;
                        break;
                    case "*":
                        result = number_1 * number_2; 
                        break;
                    case "/":
                        result = number_1 / number_2;
                        break;
                    case "%":
                        result = number_1 % number_2;
                        break;
                    case "^":
                        result = Math.Pow(number_1, number_2);
                        break;
                }
                string answer = "" + result;
                return answer;
            }

            //Cuando ya no quedan más tokens en la lista se realizan las operaciones pendientes
            double Final_Operations(double number_2)
            {
                if (south.Count() == 0) { return number_2; }
                else
                {
                    double number_1 = double.Parse(west.Pop());
                    string symbol = south.Pop();
                    return Final_Operations(double.Parse(Calculator(symbol, number_1, number_2)));
                }
            }
            
            return Aux(tokens, 0);
        }
    }
}
