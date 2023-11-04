using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HULK_01
{
    public class Assign_Var_Value
    {
        internal static List<string> Value_List = new List<string>(); 
        internal static string AssignVarValue (string arguments, int idx)
        {
            string value = "";
            double number;
            int count = 0;

            for (int i = 0; i <= arguments.Length; i++)
            {
                if (i == arguments.Length)
                {
                    count += 1;
                    if (count != Parser.variables_function[idx].Count) { return "SEMANTIX ERROR '" + Parser.function_name[idx] + "' receives " + Parser.variables_function[idx].Count + " argument(s), but " + count + " were given"; }
                    value = Tokenizer.tokenizer(value, 0);
                    if (double.TryParse(value, out number) == false) { return "!SEMANTIX ERROR '" + Parser.function_name[idx] + "' receives number not string"; }
                    Value_List.Add(value);
                    break;
                }

                if (arguments[i]  == ',')
                {
                    count += 1;
                    value = Tokenizer.tokenizer(value, 0);
                    if (double.TryParse(value, out number) == false) { return "!SEMANTIX ERROR '" + Parser.function_name[idx] + "' receives number not string"; }
                    Value_List.Add(value);
                    value = "";
                    continue;
                }

                value += arguments[i];
            }
            return "okay";
        }
    }   
}
