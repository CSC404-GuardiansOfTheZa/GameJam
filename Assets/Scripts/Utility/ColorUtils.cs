using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorUtils
{
	public static bool Equals(Color c1, Color c2) {
		return Mathf.Approximately(c1.r, c2.r) &&
		       Mathf.Approximately(c1.g, c2.g) &&
		       Mathf.Approximately(c1.b, c2.b) &&
		       Mathf.Approximately(c1.a, c2.a);
	}
}
