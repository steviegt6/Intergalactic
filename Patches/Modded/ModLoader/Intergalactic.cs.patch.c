#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ModLoader.Common.Utilities;
using ModLoader.Core.Implementation.Loading;
using ModLoader.Core.Interfaces.Loading;
using ModLoader.Core.Loading;

namespace ModLoader
{
    public static class Intergalactic
    {
        public static readonly Version LoaderVersion = new Version(0, 1, 0, 0);

        public static List<Assembly> LoadedAssemblies = new List<Assembly>
        {
            typeof(Intergalactic).Assembly
        };

        /* //// ORDERING MATTERS HERE //// */
        internal static readonly List<ILoadingHandler> LoadingHandlers = new List<ILoadingHandler>
        {
            new PreLoadableLoader(),
            new LoadableLoader()
        };

        internal static List<ILoadable> RegisteredLoadables = new List<ILoadable>();
        internal static List<IPreLoadable> PreLoadedObjects = new List<IPreLoadable>();

        internal static void LoadModAssemblies()
        {
            // TODO: Load mod assemblies here.
        }

        internal static void LoadLoadablesFromAssemblies()
        {
            foreach (Type type in from loadedAssembly in LoadedAssemblies
                select loadedAssembly.GetTypes()
                into types
                from type in types.Where(x => x.Is<ILoadable>())
                where type.GetCustomAttribute<AutoLoadExemptAttribute>() == null
                select type)
            {
                foreach (ILoadingHandler handler in LoadingHandlers.Where(handler => type.Is(handler.LoadingType)))
                {
                    ILoadable loadable = Activator.CreateInstance(type) as ILoadable;

                    if (!loadable.CanLoadILoadable())
                        break;

                    handler.LoadObject(Activator.CreateInstance(type) as ILoadable);
                    break;
                }
            }

            foreach (ILoadable loadable in RegisteredLoadables)
                loadable.Load();
        }
    }
}