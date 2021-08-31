#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

namespace ModLoader.Core.Interfaces.Loading
{
    /// <summary>
    ///     Loads types immediately when the type is resolved and validated in <see cref="Intergalactic.LoadLoadablesFromAssemblies"/>.
    /// </summary>
    internal interface IPreLoadable : ILoadable
    {
    }
}