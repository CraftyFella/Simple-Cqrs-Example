using System;
using System.Reflection;

namespace Inventory.Infrastructure.Reflection
{
	public class Property : IProperty
	{
		internal PropertyInfo PropertyInfo { get; set; }

		string IProperty.Name
		{
			get
			{
				return PropertyInfo.Name;
			}
		}

		object IProperty.GetValue(object obj, object[] index)
		{
			return PropertyInfo.GetValue(obj, index);
		}

		void IProperty.SetValue(object obj, object val, object[] index)
		{
			PropertyInfo.SetValue(obj, val, index);
		}

		public Type PropertyType
		{
			get { return PropertyInfo.PropertyType; }
		}
	}
}