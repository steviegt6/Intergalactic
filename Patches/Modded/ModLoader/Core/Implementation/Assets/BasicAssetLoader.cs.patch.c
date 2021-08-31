#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System.IO;
using ModLoader.Core.Interfaces.Assets;

namespace ModLoader.Core.Implementation.Assets
{
    public abstract class BasicAssetLoader<TAsset> : IAssetLoader<Asset<TAsset>, TAsset>
    {
        public abstract Asset<TAsset> LoadAsset(string path);

        public abstract Asset<TAsset> LoadAsset(Stream stream);
    }
}