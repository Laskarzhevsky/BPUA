using System;
using System.Collections.Generic;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Provides BPUA application layers progression
    /// </summary>
    public static class BpuaApplicationLayers
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        static BpuaApplicationLayers()
        {
            BpuaApplicationLayersProgression = new Dictionary<string, string>();
            BpuaApplicationLayersProgression.Add("SL", "BL");
            BpuaApplicationLayersProgression.Add("BL", "DPL");
            BpuaApplicationLayersProgression.Add("DPL", "DAL");
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets next layer name
        /// </summary>
        /// <param name="currentLayerName">Current layer name</param>
        /// <returns>Next layer name</returns>
        public static string GetNextLayerName(string? currentLayerName)
        {
            if (string.IsNullOrWhiteSpace(currentLayerName))
            {
                return "SL";
            }

            BpuaApplicationLayersProgression.TryGetValue(currentLayerName, out string? nextLayerName);
            if (string.IsNullOrEmpty(nextLayerName))
            {
                throw new InvalidOperationException($"No next layer found for the current layer: {currentLayerName}");
            }

            return nextLayerName;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets BPUA application layers progression
        /// </summary>
        static Dictionary<string, string> BpuaApplicationLayersProgression
        {
            get; set;
        }
        #endregion
    }
}
