using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuth.Shared.Utils;
public static class Utils
{
    public static string GetStrings(Random rd)
    {
        int count = 1;
        var str = "";
        while (count < 11)
        {
            int num;
            if (count == 2)
            {
                num = 65;
            }
            else if (count == 9)
            {
                num = 66;
            }
            else
            {
                num = rd.Next(33, 126);
                if (num == 96)
                {
                    num = rd.Next(33, 126);
                }
            }
            str += Convert.ToChar(num);
            count += 1;
        }

        return str;
    }
}
