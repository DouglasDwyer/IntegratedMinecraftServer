using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace IMS_Interface
{
    public static class Extensions
    {
        //Taken from https://dzone.com/articles/rendering-a-component-instance-with-razor-componen
        public static RenderFragment GetRenderFragment(this ComponentBase component)
        {
            var fragmentField = GetPrivateField(component.GetType(), "_renderFragment");
            var value = (RenderFragment)fragmentField.GetValue(component);
            return value;
        }

        private static FieldInfo GetPrivateField(this Type t, String name)
        {
            const BindingFlags bf = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            FieldInfo fi;
            while ((fi = t.GetField(name, bf)) == null && (t = t.BaseType) != null) { }
            return fi;
        }
    }
}
