﻿namespace Inventory.Infrastructure.Reflection
{
	public static class PrivateReflectionDynamicObjectExtensions
	{
		public static dynamic AsDynamic(this object o)
		{
			return PrivateReflectionDynamicObject.WrapObjectIfNeeded(o);
		}
	}
}