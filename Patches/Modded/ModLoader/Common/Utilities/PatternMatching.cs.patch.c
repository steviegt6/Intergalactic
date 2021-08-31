#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

namespace ModLoader.Common.Utilities
{
    public static class PatternMatching
    {
        /// <summary>
        ///     Replicates modern <c>C#</c>'s <c>is not</c> pattern.
        /// </summary>
        public static bool IsNot<T>(this object obj, out T patternObject)
        {
            if (obj.GetType() == typeof(T))
            {
                patternObject = (T) obj;
                return true;
            }

            patternObject = default;
            return false;
        }
    }
}