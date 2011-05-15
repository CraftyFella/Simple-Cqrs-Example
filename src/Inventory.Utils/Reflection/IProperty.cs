using System;

namespace Inventory.Infrastructure.Reflection
{
	public interface IProperty
	{
		string Name { get; }
		object GetValue(object obj, object[] index);
		void SetValue(object obj, object val, object[] index);
		Type PropertyType { get; }
	}
}