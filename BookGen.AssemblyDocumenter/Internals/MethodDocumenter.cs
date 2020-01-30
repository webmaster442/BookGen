﻿//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace BookGen.AssemblyDocumenter.Internals
{
    internal static class MethodDocumenter
    {
        public static void DocumentMethods(MarkdownDocument document, Type type, XElement documentation)
        {
            var methods = type.IsInterface ? type.GetMethods() : type.GetMethods(BindingFlags.Public);

            methods = methods.Where(x => !x.Name.StartsWith("get_")
                                    && !x.Name.StartsWith("set_")).ToArray();

            if (methods.Length < 1) return;

            document.Heading(2, "Methods");

            foreach (var method in methods)
            {
                var selector = GetSelectorName(type, method);
                StringBuilder pars = new StringBuilder();
                var parameters = method.GetParameters();
                int i = 0;
                foreach (var parameter in parameters)
                {
                    string callmode = "";
                    if (parameter.IsOut)
                        callmode = "out ";
                    else if (parameter.ParameterType.IsByRef)
                        callmode = "ref ";

                    if (parameter.HasDefaultValue)
                    {
                        pars.AppendFormat("{0} {1} {2} = {3}", callmode,
                                                               Helpers.GetTypeName(parameter.ParameterType),
                                                               parameter.Name,
                                                               parameter.DefaultValue);
                    }
                    else
                    {
                        pars.AppendFormat("{0} {1} {2}", callmode, parameter.ParameterType, parameter.Name);
                    }
                    if (i < parameters.Length - 1)
                        pars.Append(", ");

                    ++i;
                }
                document.WriteLine("* `{0} {1}({2});`", method.ReturnType.Name, method.Name, pars);
                document.WriteLine("    {0}", DocumentSelectors.GetMethodSummary(documentation, selector));

                foreach ((string name, string description) paramDesc in DocumentSelectors.GetMethodParamDescriptions(documentation, selector))
                {
                    document.WriteLine("    * `{0}`: {1}", paramDesc.name, paramDesc.description);
                }

            }
        }

        private static string GetSelectorName(Type type, MethodInfo method)
        {
            var parameters = method.GetParameters();
            int generic = 0;
            if (parameters.Length < 1)
                return $"{type.FullName}.{method.Name}()";

            var types = string.Join(",", parameters.Select(p =>
            {
                if (p.ParameterType.FullName == null)
                {
                    var retval = $"``{generic}";
                    ++generic;
                    return retval;
                }
                return p.ParameterType.FullName;
            }));

            if (method.ContainsGenericParameters)
            {
                var genericDount = method.GetGenericArguments().Length;
                return $"{type.FullName}.{method.Name}``{genericDount}({types})";
            }
            else
            {
                return $"{type.FullName}.{method.Name}({types})";
            }

        }
    }
}
