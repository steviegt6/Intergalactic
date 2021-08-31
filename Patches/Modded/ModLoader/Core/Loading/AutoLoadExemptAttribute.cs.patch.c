#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System;
using ModLoader.Core.Interfaces.Loading;

namespace ModLoader.Core.Loading
{
    /// <summary>
    ///     Indicates that a class implementing <see cref="ILoadable"/> won't be automatically loaded.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AutoLoadExemptAttribute : Attribute
    {
    }
}