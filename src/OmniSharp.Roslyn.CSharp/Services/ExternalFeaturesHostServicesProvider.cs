﻿using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;
using OmniSharp.Options;
using OmniSharp.Services;

namespace OmniSharp.Roslyn.CSharp.Services
{
    [Shared]
    [Export(typeof(IHostServicesProvider))]
    [Export(typeof(ExternalFeaturesHostServicesProvider))]
    public class ExternalFeaturesHostServicesProvider : IHostServicesProvider
    {
        public ImmutableArray<Assembly> Assemblies { get; }

        [ImportingConstructor]
        public ExternalFeaturesHostServicesProvider(IAssemblyLoader loader, OmniSharpOptions options, IOmniSharpEnvironment environment)
        {
            var builder = ImmutableArray.CreateBuilder<Assembly>();

            var roslynExtensionsLocations = options.RoslynExtensionsOptions.GetNormalizedLocationPaths(environment);
            if (roslynExtensionsLocations?.Any() == true)
            {
                foreach (var roslynExtensionsLocation in roslynExtensionsLocations)
                {
                    builder.AddRange(loader.LoadAllFrom(roslynExtensionsLocation));
                }
            }

            Assemblies = builder.ToImmutable();
        }
    }

    //[ExportLanguageServiceFactory(typeof(IDecompiledSourceService), LanguageNames.CSharp), Shared]
    //public class OmniSharpCSharpDecompiledSourceServiceFactory : ILanguageServiceFactory
    //{
    //    [ImportingConstructor]
    //    public OmniSharpCSharpDecompiledSourceServiceFactory()
    //    {
    //    }

    //    public ILanguageService CreateLanguageService(HostLanguageServices provider)
    //    {
    //        return new CSharpDecompiledSourceService(provider);
    //    }
    //}
}
