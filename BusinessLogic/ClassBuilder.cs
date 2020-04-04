using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace BusinessLogic
{
    public class ClassBuilder
    {
        private readonly AssemblyName _asemblyName;
        private readonly Type _type;

        public ClassBuilder(string сlassName, List<string> propertyNames, Type type)
        {
            _asemblyName = new AssemblyName(сlassName);

            TypeBuilder dynamicClass = this.CreateClass();
            this.CreateConstructor(dynamicClass);
            for (int ind = 0; ind < propertyNames.Count(); ind++)
                CreateProperty(dynamicClass, ind, propertyNames[ind], type);
            _type = dynamicClass.CreateType();
        }

        public object CreateObject()
        {
            return Activator.CreateInstance(_type);
        }

        public void SetPropertyValue<T>(object obj, string propertyName, T value)
        {
            var property = obj.GetType().GetProperty(propertyName);
            property.SetValue(obj, value);
        }

        private TypeBuilder CreateClass()
        {
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(this._asemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder typeBuilder = moduleBuilder.DefineType(this._asemblyName.FullName,
                                TypeAttributes.Public |
                                TypeAttributes.Class |
                                TypeAttributes.AutoClass |
                                TypeAttributes.AnsiClass |
                                TypeAttributes.BeforeFieldInit |
                                TypeAttributes.AutoLayout,
                                null);
            return typeBuilder;
        }
        private void CreateConstructor(TypeBuilder typeBuilder)
        {
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
        }

        private void CreateProperty(TypeBuilder typeBuilder, int index, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = typeBuilder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);

            //attributes
            var attrCtorParams1 = new Type[] { typeof(string) };
            var attrCtorInfo1 = typeof(ColumnNameAttribute).GetConstructor(attrCtorParams1);
            var attrBuilder1 = new CustomAttributeBuilder(attrCtorInfo1, new object[] { propertyName });
            propertyBuilder.SetCustomAttribute(attrBuilder1);

            var attrCtorParams2 = new Type[] { typeof(int) };
            var attrCtorInfo2 = typeof(LoadColumnAttribute).GetConstructor(attrCtorParams2);
            var attrBuilder2 = new CustomAttributeBuilder(attrCtorInfo2, new object[] { index });
            propertyBuilder.SetCustomAttribute(attrBuilder2);
                       

            MethodBuilder getPropMthdBldr = typeBuilder.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr = typeBuilder.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName,
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
