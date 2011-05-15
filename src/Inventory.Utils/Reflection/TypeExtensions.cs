using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inventory.Infrastructure.Reflection
{
	public static class TypeExtensions
	{
		/// <summary>
		/// Use this instead of get Members, Exposes IProperty Enumeration
		/// </summary>
		public static IEnumerable<IProperty> GetPropertiesAndFields(this Type type)
		{
			var typeProperties = new List<IProperty>();

            // First, add all the properties
            foreach (PropertyInfo prop in type.GetProperties().Where(p => p.DeclaringType == type))
            {
                typeProperties.Add(new Property() { PropertyInfo = prop });
            }

            // Now, add all the fields
            foreach (FieldInfo field in type.GetFields().Where(p => p.DeclaringType == type))
            {
            	typeProperties.Add(new Field() {FieldInfo = field});
            }

            // Finally, recurse on the base class to add its fields
            if (type.BaseType != null)
            {
                foreach (IProperty prop in GetPropertiesAndFields(type.BaseType))
                {
                    typeProperties.Add(prop);
                }
            }

            return typeProperties;
		}
	}
}
