using System;
using System.Reflection;
using System.Reflection.Emit;

namespace AnyProxy
{
    public class ProxyProvider
    {
        public Proxy<T> CreateProxy<T>(T obj)
        {
            var newType = CreateNewType<T>();
            var newObject = Activator.CreateInstance(newType);
            return newObject as Proxy<T>;
        }

        public Proxy<T> CreateProxy<T, TAppliedType>(T obj)
        {
            var newType = CreateNewType<T, TAppliedType>();
            var newObject = (Proxy<T>)Activator.CreateInstance(newType);
            return newObject;
        }

        private TypeBuilder BuildTypeBuilder<T>()
        {
            var originalType = typeof(T);
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = new AssemblyName(Guid.NewGuid().ToString());
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            var typeAttributes = TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit | TypeAttributes.AutoLayout;
            var typeBuilder = moduleBuilder.DefineType($"Proxy<{originalType}>", typeAttributes, originalType);
            return typeBuilder;
        }

        private Type CreateNewType<T>()
        {
            var originalType = typeof(T);
            var typeBuilder = BuildTypeBuilder<T>();

            var constructor = typeBuilder.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            var typeInfo = typeBuilder.CreateTypeInfo();
            var newType = typeInfo.AsType();
            return newType;
        }

        private Type CreateNewType<T, TAppliedType>()
        {
            var originalType = typeof(T);
            var appliedType = typeof(TAppliedType);
            var typeBuilder = BuildTypeBuilder<T>();

            var constructor = typeBuilder.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            var typeInfo = typeBuilder.CreateTypeInfo();
            var newType = typeInfo.AsType();
            return newType;
        }

        private void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { propertyType });

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }
    }
}
