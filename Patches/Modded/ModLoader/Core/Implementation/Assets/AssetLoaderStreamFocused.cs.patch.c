#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System.IO;

namespace ModLoader.Core.Implementation.Assets
{
    public abstract class AssetLoaderStreamFocused<TAsset> : BasicAssetLoader<TAsset>
    {
        public sealed override Asset<TAsset> LoadAsset(string path)
        {
            using (Stream stream = File.OpenRead(path))
                return LoadAsset(stream);
        }
    }
}