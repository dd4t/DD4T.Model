using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace DD4T.Serialization
{
    public static class Configuration
    {
        public static int JsonSerializerMaxDepth => ToInt(ConfigurationManager.AppSettings["DD4T.JsonSerializerMaxDepth"], 128);

        private static int ToInt(string v, int defaultValue)
        {
            return v == null ? defaultValue: Convert.ToInt32(v);
        }
    }
}
