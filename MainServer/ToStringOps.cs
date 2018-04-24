using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer
{
    internal static class ToStringOps
    {
        internal static string FieldValues<T>(T v)
        {
            var sb = new StringBuilder(128);
            foreach (var prop in v.GetType().GetProperties())
            {
                sb
                    .Append(prop.Name)
                    .Append(": ")
                    .Append(prop.GetValue(v))
                    .Append("\n");
            }

            foreach (var field in v.GetType().GetFields())
            {
                sb
                    .Append(field.Name)
                    .Append(": ")
                    .Append(field.GetValue(v))
                    .Append("\n");
            }

            return sb.ToString();
        }
    }
}
