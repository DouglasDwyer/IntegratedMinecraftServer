using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.Data
{
    public class PopupDisplay<T> where T : ComponentBase
    {
        public virtual Type GetComponentType()
        {
            return typeof(T);
        }
    }
}
