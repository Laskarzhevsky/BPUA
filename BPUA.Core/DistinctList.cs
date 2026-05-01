using System.Collections.Generic;

namespace BPUA.Core
{
    /// <summary>
    /// Provides distinct list functionality
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public class DistinctList<T> : List<T>
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public DistinctList() : base()
        {
        }

        /// <summary>
        /// Makes instance of distinct list copying unique values from list with non-unique values
        /// </summary>
        /// <param name="list">List with non-unique values</param>
        public DistinctList(List<T> list)
        {
            Add(list);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds unique value
        /// </summary>
        /// <param name="value">Value for adding</param>
        public new void Add(T value)
        {
            if (value == null)
            {
                return;
            }

            if (Contains(value))
            {
                return;
            }

            base.Add(value);
        }

        /// <summary>
        /// Adds unique values from list with non-unique values
        /// </summary>
        /// <param name="list">List with non-unique values</param>
        public void Add(List<T> list)
        {
            if (list == null)
            {
                return;
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (Contains(list[i]))
                {
                    continue;
                }

                Add(list[i]);
            }
        }
        #endregion
    }
}
