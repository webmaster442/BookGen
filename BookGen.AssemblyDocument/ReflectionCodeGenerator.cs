using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BookGen.AssemblyDocument
{
    public static class ReflectionCodeGenerator
    {
        public static string GetCode(this PropertyInfo property, bool isStatic = false)
        {
            List<string> output = new List<string>();
            if (property.GetMethod?.IsPublic ?? false)
                output.Add("public");
            else if (property.GetMethod?.IsFamilyOrAssembly ?? false)
                output.Add("protected internal");
            else if (property.GetMethod?.IsAssembly ?? false)
                output.Add("internal");
            else if (property.GetMethod?.IsFamily ?? false)
                output.Add("protected");
            else
                output.Add("private");

            if (isStatic)
                output.Add("static");

            output.Add(property.PropertyType.GetNormalizedTypeName());
            output.Add(property.Name);
            output.Add("{");

            if (property.CanRead)
                output.Add("get;");

            if (property.IsInitOnly())
            {
                output.Add("init;");
            }
            else if (property.CanWrite
                && (property.SetMethod?.IsPublic ?? false))
            {
                output.Add("set;");
            }
            output.Add("}");
            return string.Join(' ', output);
        }

        public static string GetCode(this ConstructorInfo constructor, bool isStatic = false)
        {
            List<string> output = new List<string>();
            if (constructor.IsPublic)
                output.Add("public");
            else if (constructor.IsFamilyOrAssembly)
                output.Add("protected internal");
            else if (constructor.IsAssembly)
                output.Add("internal");
            else if (constructor.IsFamily)
                output.Add("protected");
            else
                output.Add("private");

            if (isStatic)
                output.Add("static");

            output.Add(constructor.DeclaringType?.GetNormalizedTypeName(false) ?? "");
            AddParameters(constructor.GetParameters(), output);

            return string.Join(' ', output);
        }

        private static void AddParameters(ParameterInfo[] parameterInfos, List<string> output)
        {
            output.Add("(");
            //foreach (var parameter in parameterInfos)
            for (int i=0; i<parameterInfos.Length; i++)
            {
                var parameter = parameterInfos[i];

                if (parameter.IsOut)
                    output.Add("out");
                if (parameter.ParameterType.IsByRef)
                    output.Add("ref");
                output.Add(parameter.ParameterType.GetNormalizedTypeName());
                output.Add(parameter.Name ?? "");
                if (parameter.IsOptional)
                    output.Add(parameter.DefaultValue?.ToString() ?? "");
                
                if (i != parameterInfos.Length -1)
                    output.Add(",");
            }
            output.Add(");");
        }

        private static bool IsInitOnly(this PropertyInfo propertyInfo)
        {
            MethodInfo? setMethod = propertyInfo.SetMethod;
            if (setMethod == null)
                return false;

            var isExternalInitType = typeof(System.Runtime.CompilerServices.IsExternalInit);
            return setMethod.ReturnParameter.GetRequiredCustomModifiers().Contains(isExternalInitType);
        }

    }
}
