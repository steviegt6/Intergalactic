#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System.IO;

namespace ModLoader.Core.Interfaces.Assets
{
    public interface IAssetLoader<out TLoadable, TAsset> where TLoadable : ILoadableAsset<TAsset>
    {
        TLoadable LoadAsset(string path);

        TLoadable LoadAsset(Stream stream);
    }
}