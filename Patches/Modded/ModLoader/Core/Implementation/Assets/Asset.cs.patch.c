#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using ModLoader.Core.Interfaces.Assets;

namespace ModLoader.Core.Implementation.Assets
{
    public sealed class Asset<T> : ILoadableAsset<T>
    {
        public T AssetValue { get; }

        public Asset(T asset)
        {
            AssetValue = asset;
        }
    }
}