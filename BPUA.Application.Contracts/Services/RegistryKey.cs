using System.Reflection;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Builds canonical keys and group prefixes based on attribute types.
    /// </summary>
    public static class RegistryKey
    {
        #region Constants
        /// <summary>
        /// Defines prefix
        /// </summary>
        const string Root = "__BPUA/";
        #endregion

        #region Methods
        /// <summary>
        /// Creates key for assembly item
        /// </summary>
        /// <param name="attributeType">Attribute type</param>
        /// <param name="assembly">Assembly for key creation</param>
        /// <returns>Key for assembly item</returns>
        public static string AssemblyItem(System.Type attributeType, Assembly assembly)
        {
            string? name;
            if (assembly.GetName().Name != null)
            {
                name = assembly.GetName().Name;
            }
            else
            {
                name = assembly.FullName;
            }

            if (string.IsNullOrEmpty(name))
            {
                name = System.Guid.NewGuid().ToString("N");
            }

            return Item(attributeType, name);
        }

        /// <summary>
        /// Creates group key prefix
        /// </summary>
        /// <param name="attributeType">Attribute type</param>
        /// <returns>Group key prefix</returns>
        public static string Group(System.Type attributeType)
        {
            return Root + attributeType.FullName + "/";
        }

        /// <summary>
        /// Creates item key
        /// </summary>
        /// <param name="attributeType">Attribute type</param>
        /// <param name="name">Item name</param>
        /// <returns>Item key</returns>
        public static string Item(System.Type attributeType, string name)
        {
            return Group(attributeType) + name;
        }
        public static string Service(System.Type serviceInterfaceType)
        {
            return Root + "service/" + serviceInterfaceType.FullName;
        }
        #endregion
    }
}
