using System.Numerics;

namespace IsPrime_Checker
{
    internal class Prime_Checker
    {
        public static bool IsPrime(BigInteger n)
        {
            if (n < 2)
                return false;

            if (n == 2)
                return true;

            if (n % 2 == 0)
                return false;

            for (BigInteger i = 3; i * i <= n; i += 2)
            {
                if (n % i == 0)
                    return false;
            }
            return true;
        }
    }
}
