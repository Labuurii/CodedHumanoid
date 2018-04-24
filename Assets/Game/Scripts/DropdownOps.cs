using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

public static class DropdownOps
{
    public static int OptionValueByString(Dropdown dropdown, string v)
    {
        for(var i = 0; i < dropdown.options.Count; ++i)
        {
            if(dropdown.options[i].text == v)
            {
                return i;
            }
        }

        throw new ArgumentException("string is not part of the dropdown options");
    }
}
