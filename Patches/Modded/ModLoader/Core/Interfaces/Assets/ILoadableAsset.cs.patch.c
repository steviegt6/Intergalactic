#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

namespace ModLoader.Core.Interfaces.Assets
{
    public interface ILoadableAsset<out T>
    {
        T AssetValue { get; }
    }
}