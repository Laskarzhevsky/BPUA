using System;
using System.Collections.Generic;
using System.Reflection;

namespace BPUA.Core
{
    /// <summary>
    /// Provides list of assemblies funcgtionality
    /// </summary>
    public static class ListOfAssemblies
    {
        /// <summary>
        /// Sorts list of assemblies by Assembly.FullName (ascending)
        /// </summary>
        public static void SortListByFullName(List<Assembly> list)
        {
            for (int i = 1; i < list.Count; i++)
            {
                Assembly? key = list[i];
                if (key == null || key.FullName == null)
                {
                    continue;
                }

                string keyName = key.FullName;
                int j = i - 1;

                while (j >= 0)
                {
                    Assembly? current = list[j];
                    if (current == null || current.FullName == null)
                    {
                        break;
                    }

                    string currentName = current.FullName;
                    if (string.Compare(currentName, keyName, StringComparison.Ordinal) > 0)
                    {
                        list[j + 1] = list[j];
                        j--;
                    }
                    else
                    {
                        break;
                    }
                }

                list[j + 1] = key;
            }
        }
    }
}
