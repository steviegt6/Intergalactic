#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System;
using ModLoader.Core.Interfaces.Loading;

namespace ModLoader.Core.Implementation.Loading
{
    internal class LoadableLoader : ILoadingHandler
    {
        public Type LoadingType => typeof(ILoadable);

        public void LoadObject(ILoadable loadable) => Intergalactic.RegisteredLoadables.Add(loadable);
    }
}