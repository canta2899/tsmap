using System.Collections;

namespace TypescriptMapper.Extensions;

public static class TypeExtensions
{
   private static Type[] CollectionTypes = { typeof(IEnumerable), typeof(ICollection<>), typeof(IEnumerator) };
   
   public static bool IsNumeric(this Type type)
   {
        switch (Type.GetTypeCode(type))
        { 
            case TypeCode.Byte:
            case TypeCode.SByte:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Single:
              return true;
            default:
              return false;
        }
   }

   public static bool IsCollection(this Type type)
   {
      return type.GetInterfaces().Any(x => 
         CollectionTypes.Contains(x) || (x.IsGenericType && CollectionTypes.Contains(x.GetGenericTypeDefinition())));
   }

   public static bool IsString(this Type type)
   {
      switch (Type.GetTypeCode(type))
      {
         case TypeCode.String:
         case TypeCode.Char:
            return true;
         default:
            return false;
      }
   }

   public static bool IsBool(this Type type)
   {
      return type == typeof(bool);
   }

   public static bool IsDate(this Type type) => Type.GetTypeCode(type) == TypeCode.DateTime;

   public static bool IsArray(this Type type) => type.IsArray;

   public static bool IsObject(this Type type) => type.Name == "Object";

   public static Type? GetArrayType(this Type type)
   {
      if (!type.IsArray) throw new Exception("Type is not an array, therefore inner cannot be retrieved");
      return type?.GetElementType();
   }
}