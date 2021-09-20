//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace BookGen.Core.Documenter
{
    internal class MethodDocumenter : DocumenterBase
    {
        public override void Document(MarkdownWriter targetDocument, Type type, XElement docSource)
        {
            var methods = type.IsInterface ? type.GetMethods() : type.GetMethods(BindingFlags.Public);

            methods = methods.Where(x => !x.Name.StartsWith("get_")
                                    && !x.Name.StartsWith("set_")).ToArray();

            if (methods.Length < 1) return;

            targetDocument.Heading(2, "Methods");

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
                                                               GetTypeName(parameter.ParameterType),
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
                targetDocument.WriteLine("* `{0} {1}({2});`", method.ReturnType.Name, method.Name, pars);
                targetDocument.WriteLine("    {0}", Selectors.GetMethodSummary(docSource, selector));

                foreach ((string name, string description) paramDesc in Selectors.GetMethodParamDescriptions(docSource, selector))
                {
                    targetDocument.WriteLine("    * `{0}`: {1}", paramDesc.name, paramDesc.description);
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
