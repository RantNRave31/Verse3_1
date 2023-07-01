using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GKYU.CoreLibrary.MetaData
{
    public static class TypeExtensions
    {
        public static bool IsSameOrSubclass(this Type potentialBase, Type potentialDescendant)
        {
            return potentialDescendant.IsSubclassOf(potentialBase)
                   || potentialDescendant == potentialBase;
        }
        private static MethodInfo FindGenericMethod(this Type type, string methodName, Type[] typeArguments, Type[] parameterTypes, out MethodInfo methodInfo, out ParameterInfo[] parameters)
        {

            methodInfo = null;
            parameters = null;

            if (null == parameterTypes)
            {
                methodInfo = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
                methodInfo = methodInfo.MakeGenericMethod(typeArguments);
                parameters = methodInfo.GetParameters();
            }
            else
            {
                // Method is probably overloaded. As far as I know there's no other way 
                // to get the MethodInfo instance, we have to
                // search for it in all the type methods
                MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                foreach (MethodInfo method in methods)
                {
                    if (method.Name == methodName)
                    {
                        // create the generic method
                        MethodInfo genericMethod = method.MakeGenericMethod(typeArguments);
                        parameters = genericMethod.GetParameters();

                        // compare the method parameters
                        if (parameters.Length == parameterTypes.Length)
                        {
                            for (int i = 0; i < parameters.Length; i++)
                            {
                                if (parameters[i].ParameterType != parameterTypes[i])
                                {
                                    continue; // this is not the method we're looking for
                                }
                            }

                            // if we're here, we got the right method
                            methodInfo = genericMethod;
                            break;
                        }
                        else if (parameterTypes.Length == 0)//TODO:  fix bug, make it useful for types other than current use (if more than one func sig, bug)
                        {
                            methodInfo = genericMethod;
                        }
                    }
                }

                if (null == methodInfo)
                {
                    throw new InvalidOperationException("Method not found");
                }
            }
            return methodInfo;
        }
        public static MethodInfo FindGenericMethod(this Type type, string methodName, Type[] typeArguments, Type[] parameterTypes)
        {
            MethodInfo methodInfo;
            ParameterInfo[] parameters;
            type.FindGenericMethod(methodName, typeArguments, parameterTypes, out methodInfo, out parameters);
            return methodInfo;
        }
        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }
    }
}
