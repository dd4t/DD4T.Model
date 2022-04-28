using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace DD4T.Serialization
{
    public static class Configuration
    {
        public static int JsonSerializerMaxDepth => ToInt(ConfigurationManager.AppSettings["DD4T.JsonSerializerMaxDepth"]);

        private static int ToInt(string v)
        {
            return v == null ? default(int) : Convert.ToInt32(v);
        }
    }
}
