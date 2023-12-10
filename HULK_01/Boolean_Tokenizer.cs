using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HULK_01
{
    internal class Boolean_Tokenizer
    {
        internal static string TrueOrFalse (string condition)
        {
            for (int i = 0; i < condition.Length; i++)
            {
                if (condition[i] == '&')
                {
                    if (condition[i + 1] == '&')
                    {
                        string left = If_Else_Tokenizer.Condition(condition.Substring(0, i));
                        string right = If_Else_Tokenizer.Condition(condition.Substring(i + 2, condition.Length - 2 - i));
                        if(left == "true" &&  right == "true") { return "true"; }
                        else { return  "false"; }
                    }
                    if (condition[i + 1] != '&') return "SYNTAX ERROR";
                }

                if (condition[i] == '|')
                {
                    if (condition[i + 1] == '|')
                    {
                        string left = If_Else_Tokenizer.Condition(condition.Substring(0, i));
                        string right = If_Else_Tokenizer.Condition(condition.Substring(i + 2, condition.Length - 2 - i));
                        if (left == "false" && right == "false") { return "true"; }
                        else { return "true"; }
                    }
                    if (condition[i + 1] != '|') return "SYNTAX ERROR";
                }
            }
            return If_Else_Tokenizer.Condition(condition);
        }
    }
}
