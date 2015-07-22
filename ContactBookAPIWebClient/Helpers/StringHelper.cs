using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactBookAPIWebClient.Helpers
{
    public static class StringHelper
    {
        public static string ToCommaSeparatedValues(this string[] input)
        {
            string result = string.Empty;

            for (int i=0; i<input.Length; i++)
            {
                result = string.Concat(result, input[i], (i == input.Length ? "" : ","));    
            }
            return result;
        }
    }
}