#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using ModLoader.Core.Loading;

namespace ModLoader.Core.Interfaces.Loading
{
    /// <summary>
    ///     Indicates that a <see cref="ILoadable"/> won't make it past the load process if its requirements are not met.
    /// </summary>
    /// <remarks>
    ///     To prevent a type from being loaded or instantiated altogether, use <see cref="AutoLoadExemptAttribute"/>.
    /// </remarks>
    public interface IHasLoadingRequirements
    {
        /// <summary>
        ///     Called before <see cref="ILoadable.Load"/> but after type activation.
        /// </summary>
        /// <returns>Whether this type should be loaded.</returns>
        bool CanLoad();
    }
}