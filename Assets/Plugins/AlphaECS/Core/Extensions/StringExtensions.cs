using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public static class StringExtensions
{
	public static Type GetTypeWithAssembly(this string typeName)
	{
        return TypeUtilities.GetTypeWithAssembly(typeName);
	}

	public static Type TryGetConvertedType(this string typeName)
	{
        return TypeUtilities.TryGetConvertedType(typeName);
	}
}
