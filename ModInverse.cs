using System.Numerics;

namespace WpfApp_ModInv
	{
	internal class ModInverse
		{
		public static BigInteger ModInverse_Proc(BigInteger a, BigInteger m)
			{
			BigInteger t = 0, newT = 1;
			BigInteger r = m, newR = a;

			while (newR != 0)
				{
				BigInteger q = r / newR;
				(t, newT) = (newT, t - q * newT);
				(r, newR) = (newR, r - q * newR);
				}

			if (t < 0) t += m;
			return t;
			}
		}
	}
