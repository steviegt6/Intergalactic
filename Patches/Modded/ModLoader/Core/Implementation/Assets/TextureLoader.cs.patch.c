#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System.IO;
using UnityEngine;

namespace ModLoader.Core.Implementation.Assets
{
    public class TextureLoader : AssetLoaderStreamFocused<Texture2D>
    {
        public override Asset<Texture2D> LoadAsset(Stream stream)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                stream.CopyTo(memStream);

                Texture2D texture = new Texture2D(2, 2);
                texture.LoadRawTextureData(memStream.ToArray());
                return new Asset<Texture2D>(texture);
            }
        }
    }
}