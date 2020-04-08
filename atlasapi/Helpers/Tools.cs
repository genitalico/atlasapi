using System;
using System.Text;

namespace atlasapi.Helpers
{
    public static class Tools
    {
        #region PublicMethods
        public static string GetAlphanumericRandom(int size)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < size; i++)
            {
                sb.Append(GetAlphanumeric());
            }

            return sb.ToString();
        }
        #endregion

        #region PrivateMethods
        private static char GetAlphanumeric()
        {
            string chars = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            int num = rand.Next(0, chars.Length - 1);
            return chars[num];
        }
        #endregion

    }
}
