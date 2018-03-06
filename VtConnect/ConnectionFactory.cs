using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VtConnect
{
    internal class ConnectionFactory
    {
        private static Dictionary<string, Type> _SchemeTypes;
        private static Dictionary<string, Type> SchemeTypes
        {
            get
            {
                if (_SchemeTypes == null)
                    _SchemeTypes = EnumerateSchemeTypes();

                return _SchemeTypes;
            }
        }

        private static Dictionary<string, Type> EnumerateSchemeTypes()
        {
            var schemeTypes =
                from assemblies in AppDomain.CurrentDomain.GetAssemblies()
                from types in assemblies.GetTypes()
                let attributes = types.GetCustomAttributes(typeof(UriSchemeAttribute), true)
                where attributes != null && attributes.Length > 0
                select new { Type = types, SchemeAttribute = attributes.Cast<UriSchemeAttribute>().Single() };

            var result = new Dictionary<string, Type>();

            Type connectionType = typeof(Connection);

            foreach(var schemeType in schemeTypes)
            {
                if(connectionType.IsAssignableFrom(schemeType.Type))
                {
                    result.Add(
                        schemeType.SchemeAttribute.Name,
                        schemeType.Type
                    );
                }
            }

            return result;
        }

        public static Connection CreateByScheme(string scheme)
        {
            if (!SchemeTypes.TryGetValue(scheme, out Type typeForScheme))
                return null;

            return (Connection)Activator.CreateInstance(typeForScheme);
        }
    }
}
