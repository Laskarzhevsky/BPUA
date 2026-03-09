using System;
using System.Collections.Generic;
using System.Linq;

namespace BPUA.Core
{
    /// <summary>
    /// Provides breadcrumbs functionality
    /// </summary>
    public static class Breadcrumbs
    {
        #region Public Methods
        /// <summary>
        /// Appends breadcrumb to breadcrumbs by using separator
        /// </summary>
        /// <param name="breadcrumbs">Breadcrumbs string</param>
        /// <param name="breadcrumb">Breadcrumb string</param>
        /// <param name="separator">Separator string</param>
        /// <returns>Breadcrumbs with breadcrumb appended</returns>
        public static string Append(string? breadcrumbs, string breadcrumb, string separator = "/")
        {
            if (breadcrumbs == null)
            {
                breadcrumbs = "";
            }

            if (string.IsNullOrEmpty(breadcrumb))
            {
                return breadcrumbs;
            }

            if (breadcrumbs.EndsWith(separator))
            {
                breadcrumbs = breadcrumbs + breadcrumb;
            }
            else
            {
                if (string.IsNullOrEmpty(breadcrumbs))
                {
                    breadcrumbs = breadcrumb;
                }
                else
                {
                    breadcrumbs = breadcrumbs + separator + breadcrumb;
                }
            }

            return breadcrumbs;
        }

        public static int GetBreadcrumbsDepth(string? breadcrumbs, char separator = '/')
        {
            if (string.IsNullOrEmpty(breadcrumbs))
            {
                return 0;
            }

            int breadcrumbsDepth = 0;
            for (int i = 0; i < breadcrumbs.Length; i++)
            {
                if (breadcrumbs[i] == separator)
                {
                    breadcrumbsDepth++;
                }
            }

            return breadcrumbsDepth;
        }

        /// <summary>
        /// Gets breadcrumbs without last segment
        /// </summary>
        /// <param name="breadcrumbs">Breadcrumbs string</param>
        /// <param name="separator">Separator string</param>
        /// <returns>Previous breadcrumbs</returns>
        public static string GetBreadcrumbsWithoutLastSegment(string? breadcrumbs, string separator = "/")
        {
            if (string.IsNullOrEmpty(breadcrumbs))
            {
                return "";
            }

            if (breadcrumbs.Contains(separator))
            {
                int separatorLastPosition = breadcrumbs.LastIndexOf(separator);
                return breadcrumbs.Substring(0, separatorLastPosition);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Gets breadcrumbs last segment
        /// </summary>
        /// <param name="breadcrumbs">Breadcrumbs string</param>
        /// <param name="separator">Separator string</param>
        /// <returns>Last breadcrumb</returns>
        public static string GetBreadcrumsLastSegment(string? breadcrumbs, string separator = "/")
        {
            if (string.IsNullOrEmpty(breadcrumbs))
            {
                return "";
            }

            if (breadcrumbs.Contains(separator))
            {
                string[] breadcrumbsAsArray = breadcrumbs.Split(separator);
                return breadcrumbsAsArray[breadcrumbsAsArray.Length - 1];
            }
            else
            {
                return breadcrumbs;
            }
        }

        /// <summary>
        /// Normalizes breadcrumbs
        /// </summary>
        /// <param name="breadcrumbsString">Breadcrumbs string</param>
        /// <returns>Normalized breadcrumbs</returns>
        public static string NormalizeBreadcrumbs(string? breadcrumbsString)
        {
            if (breadcrumbsString == null)
            {
                return string.Empty;
            }

            string normalizedBreadcrumbs = breadcrumbsString.Replace('\\', '/').Trim();
            while (normalizedBreadcrumbs.StartsWith("/", System.StringComparison.Ordinal))
            {
                normalizedBreadcrumbs = normalizedBreadcrumbs.Substring(1);
            }

            while (normalizedBreadcrumbs.EndsWith("/", System.StringComparison.Ordinal))
            {
                normalizedBreadcrumbs = normalizedBreadcrumbs.Substring(0, normalizedBreadcrumbs.Length - 1);
            }

            return normalizedBreadcrumbs;
        }

        public static string ParentOf(string normalizedPath)
        {
            if (string.IsNullOrEmpty(normalizedPath))
            {
                return string.Empty;
            }

            int idx = normalizedPath.LastIndexOf('/');
            if (idx <= 0)
            {
                return string.Empty;
            }

            return normalizedPath.Substring(0, idx);
        }

        /// <summary>
        /// Gets breadcrumbs as an array
        /// </summary>
        /// <param name="breadcrumbs">Breadcrumbs string</param>
        /// <param name="separator">Separator string</param>
        /// <returns>Array of breadcrumbs</returns>
        public static string[] ToArray(string? breadcrumbs, string separator = "/")
        {
            if (string.IsNullOrEmpty(breadcrumbs))
            {
                return Array.Empty<string>();
            }

            return breadcrumbs.Split(separator);
        }

        /// <summary>
        /// Gets breadcrumbs as a list
        /// </summary>
        /// <param name="breadcrumbs">Breadcrumbs string</param>
        /// <param name="separator">Separator string</param>
        /// <returns>List of breadcrumbs</returns>
        public static List<string> ToList(string? breadcrumbs, string separator = "/")
        {
            if (string.IsNullOrEmpty(breadcrumbs))
            {
                return new List<string>();
            }

            return breadcrumbs.Split(separator).ToList();
        }
        #endregion
    }
}
