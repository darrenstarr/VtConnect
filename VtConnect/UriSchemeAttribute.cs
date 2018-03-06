using System;

namespace VtConnect
{
    internal class UriSchemeAttribute : Attribute
    {
        public string Name { get; private set; }
        public UriSchemeAttribute(string name)
        {
            Name = name;
        }
    }
}