#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System;
using ModLoader.Common.Utilities;
using ModLoader.Core.Interfaces.Loading;

namespace ModLoader.Core.Implementation.Loading
{
    internal sealed class PreLoadableLoader : ILoadingHandler
    {
        public Type LoadingType => typeof(IPreLoadable);

        public void LoadObject(ILoadable loadable)
        {
            if (loadable.IsNot(out IPreLoadable preLoadable)) 
                return;

            preLoadable.Load();
            Intergalactic.PreLoadedObjects.Add(preLoadable);
        }
    }
}