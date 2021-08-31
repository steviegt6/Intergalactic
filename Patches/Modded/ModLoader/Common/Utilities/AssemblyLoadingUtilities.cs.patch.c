#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System;
using ModLoader.Core.Interfaces.Loading;

namespace ModLoader.Common.Utilities
{
    public static class AssemblyLoadingUtilities
    {
        /// <summary>
        ///     Checks if the type matches the type of <typeparam name="T"></typeparam>. <br />
        ///     Supports checking interfaces and derived types.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <param name="type">The type to validate.</param>
        /// <param name="activatorSafe">Whether to validate <see cref="Activator"/> creation safety with <see cref="IsActivatorSafe"/>.</param>
        /// <returns><see langword="true"/> when all conditions are met.</returns>
        public static bool Is<T>(this Type type, bool activatorSafe = true) => type.Is(typeof(T), activatorSafe);

        /// <inheritdoc cref="Is{T}"/>
        public static bool Is(this Type type, Type tType, bool activatorSafe = true)
        {
            bool isInterface = tType.IsInterface;

            // Actual checks. Use IsAssignableFrom for interfaces, otherwise check derived types.
            bool IsValid() => isInterface ? tType.IsAssignableFrom(type) : type.IsSubclassOf(tType);

            // Don't check for activator safety if it isn't requested.
            if (!activatorSafe)
                return IsValid();

            return IsValid() && type.IsActivatorSafe();
        }

        /// <summary>
        ///     Checks whether or not a type can be safely activated through <see cref="Activator.CreateInstance(Type)"/>. <br />
        ///     More specifically, it ensures there is an accessible parameterless constructor, the type is not abstract, and the type is not an interface.
        /// </summary>
        /// <param name="type">The type to validate.</param>
        /// <returns><see langword="true"/> if all conditions are met.</returns>
        public static bool IsActivatorSafe(this Type type) => type.GetConstructor(Array.Empty<Type>()) != null && 
                                                              !type.IsInterface && !type.IsAbstract;

        public static bool CanLoadILoadable(this ILoadable loadable) =>
            loadable is IHasLoadingRequirements loadingRequirements ? loadingRequirements.CanLoad() : true;
    }
}