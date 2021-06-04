using BookGen.Api;
using BookGen.AssemblyDocument.XmlDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BookGen.AssemblyDocument.Documenters
{
    internal class TypeMembersDocumenter : DocumenterBase
    {

        public TypeMembersDocumenter(Doc xmlDocumentation, ILog log) : base(xmlDocumentation, log)
        {
        }

        public override bool CanExecute(Type type)
        {
            var t = type.GetTypeType();
            return t != Domain.TypeType.Enum && t != Domain.TypeType.Delegate;
        }

        public override void Execute(Type type, MarkdownBuilder output)
        {
            List<ConstructorInfo> constructors = new();
            List<FieldInfo> fields = new List<FieldInfo>();
            List<PropertyInfo> properties = new List<PropertyInfo>();
            List<EventInfo> events = new List<EventInfo>();
            List<MethodInfo> methods = new List<MethodInfo>();

            foreach (var member in type.GetMembers())
            {
                if (member.MemberType.HasFlag(MemberTypes.Constructor))
                    constructors.Add((member as ConstructorInfo)!);
                else if (member.MemberType.HasFlag(MemberTypes.Field))
                    fields.Add((member as FieldInfo)!);
                else if (member.MemberType.HasFlag(MemberTypes.Property))
                    properties.Add((member as PropertyInfo)!);
                else if (member.MemberType.HasFlag(MemberTypes.Event))
                    events.Add((member as EventInfo)!);
                else if (member.MemberType.HasFlag(MemberTypes.Method))
                    methods.Add((member as MethodInfo)!);
            }

            output.H2("Constructors");
            WriteConstructors(constructors, output);

            output.H2("Fields");
            WriteFields(fields, output);

            output.H2("Properties");
            WriteProperties(fields, output);

            output.H2("Events");
            WriteEvents(events, output);

            output.H2("Methods");
            WriteMethods(methods, output);
        }

        private void WriteConstructors(List<ConstructorInfo> constructors, MarkdownBuilder output)
        {
            foreach (var ctor in constructors.GetPublicProtected())
            {
                
            }
        }

        private void WriteFields(List<FieldInfo> fields, MarkdownBuilder output)
        {
            throw new NotImplementedException();
        }

        private void WriteProperties(List<FieldInfo> fields, MarkdownBuilder output)
        {
            throw new NotImplementedException();
        }

        private void WriteEvents(List<EventInfo> events, MarkdownBuilder output)
        {
            throw new NotImplementedException();
        }

        private void WriteMethods(List<MethodInfo> methods, MarkdownBuilder output)
        {
            throw new NotImplementedException();
        }
    }
}