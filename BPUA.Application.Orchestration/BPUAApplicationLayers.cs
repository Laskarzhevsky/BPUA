using System.Collections.Generic;

namespace BPUA.Application.Orchestration
{
    /// <summary>
    /// Provides BPUA application layers progression
    /// </summary>
    public static class BPUAApplicationLayers
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        static BPUAApplicationLayers()
        {
            BPUAApplicationLayersProgression = new Dictionary<string, string>();
            BPUAApplicationLayersProgression.Add("SL", "BL");
            BPUAApplicationLayersProgression.Add("BL", "DPL");
            BPUAApplicationLayersProgression.Add("DPL", "DAL");
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets next layer name
        /// </summary>
        /// <param name="currentLayerName">Current layer name</param>
        /// <returns>Next layer name</returns>
        public static string? GetNextLayerName(string? currentLayerName)
        {
            if (string.IsNullOrWhiteSpace(currentLayerName))
            {
                return "SL";
            }

            BPUAApplicationLayersProgression.TryGetValue(currentLayerName, out string? nextLayerName);
            if (string.IsNullOrEmpty(nextLayerName))
            {
                return currentLayerName;
            }

            return nextLayerName;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets BPUA application layers progression
        /// </summary>
        static Dictionary<string, string> BPUAApplicationLayersProgression
        {
            get; set;
        }
        #endregion
    }
}
