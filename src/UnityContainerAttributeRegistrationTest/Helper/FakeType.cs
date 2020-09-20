using System;
using System.Globalization;
using System.Linq;
using System.Reflection;


namespace UnityContainerAttributeRegistrationTest.Helper
{
#pragma warning disable CS8632
    public class FakeType : Type
    {
        private readonly Attribute[] attributes;

        public FakeType(string             ns,
                        string             name,
                        Assembly           assembly,
                        Type               baseType = null,
                        params Attribute[] attributes)
        {
            this.attributes = attributes;

            Namespace = ns;
            Name      = name;
            Assembly  = assembly;
            BaseType  = baseType ?? typeof(object);

            FullName = $"{ns}.{name}";
        }

        public override Module Module { get; }

        public override string? Namespace { get; }

        public override string Name { get; }

        public override Type UnderlyingSystemType { get; }

        public override Assembly Assembly { get; }

        public override string? AssemblyQualifiedName { get; }

        public override Type? BaseType { get; }

        public override string? FullName { get; }

        public override Guid GUID { get; }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return GetCustomAttributes(typeof(Attribute), inherit);
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            object[] result = attributes.Where(attr => attr.GetType() == attributeType || attr.GetType().IsSubclassOf(attributeType))
                                        .ToArray();

            return result;
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return attributes.Any(attr => attr.GetType() == attributeType);
        }

        public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override Type? GetElementType()
        {
            throw new NotImplementedException();
        }

        public override EventInfo? GetEvent(string name, BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override EventInfo[] GetEvents(BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override FieldInfo? GetField(string name, BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override FieldInfo[] GetFields(BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override object? InvokeMember(string               name,
                                             BindingFlags         invokeAttr,
                                             Binder?              binder,
                                             object?              target,
                                             object?[]?           args,
                                             ParameterModifier[]? modifiers,
                                             CultureInfo?         culture,
                                             string[]?            namedParameters)
        {
            throw new NotImplementedException();
        }

        public override Type? GetNestedType(string name, BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override Type[] GetNestedTypes(BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override Type? GetInterface(string name, bool ignoreCase)
        {
            throw new NotImplementedException();
        }

        public override Type[] GetInterfaces()
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            int hash = 1337;
            hash ^= Assembly.GetHashCode();
            hash ^= attributes.GetHashCode();
            hash ^= Name.GetHashCode();

            if(Namespace != null)
            {
                hash ^= Namespace.GetHashCode();
            }

            if(FullName != null)
            {
                hash ^= FullName.GetHashCode();
            }

            return hash;
        }

        protected override TypeAttributes GetAttributeFlagsImpl()
        {
            throw new NotImplementedException();
        }

        protected override ConstructorInfo? GetConstructorImpl(BindingFlags         bindingAttr,
                                                               Binder?              binder,
                                                               CallingConventions   callConvention,
                                                               Type[]               types,
                                                               ParameterModifier[]? modifiers)
        {
            throw new NotImplementedException();
        }

        protected override MethodInfo? GetMethodImpl(string               name,
                                                     BindingFlags         bindingAttr,
                                                     Binder?              binder,
                                                     CallingConventions   callConvention,
                                                     Type[]?              types,
                                                     ParameterModifier[]? modifiers)
        {
            throw new NotImplementedException();
        }

        protected override bool IsArrayImpl()
        {
            return false;
        }

        protected override bool IsByRefImpl()
        {
            throw new NotImplementedException();
        }

        protected override bool IsCOMObjectImpl()
        {
            throw new NotImplementedException();
        }

        protected override bool IsPointerImpl()
        {
            throw new NotImplementedException();
        }

        protected override bool IsPrimitiveImpl()
        {
            throw new NotImplementedException();
        }

        protected override PropertyInfo? GetPropertyImpl(string               name,
                                                         BindingFlags         bindingAttr,
                                                         Binder?              binder,
                                                         Type?                returnType,
                                                         Type[]?              types,
                                                         ParameterModifier[]? modifiers)
        {
            throw new NotImplementedException();
        }

        protected override bool HasElementTypeImpl()
        {
            throw new NotImplementedException();
        }
    }
#pragma warning restore CS8632
}
