#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System;

namespace ModLoader.Core.Interfaces.Loading
{
    /// <summary>
    ///     Loads <see cref="ILoadable"/> instances.
    /// </summary>
    internal interface ILoadingHandler
    {
        /// <summary>
        ///     The <see cref="ILoadable"/> type to handle.
        /// </summary>
        Type LoadingType { get; }

        /// <summary>
        ///     Loads the <see cref="ILoadable"/> instance (<paramref name="loadable"/>).
        /// </summary>
        /// <param name="loadable">The instance to load.</param>
        void LoadObject(ILoadable loadable);
    }
}