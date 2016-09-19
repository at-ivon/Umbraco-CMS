﻿using System.IO;
using Umbraco.Core.Components;
using Umbraco.Core.DependencyInjection;
using Umbraco.Core.IO;
using Umbraco.Core.Manifest;

namespace Umbraco.Core.Strategies
{
    public class ManifestWatcherComponent : UmbracoComponentBase, IUmbracoCoreComponent
    {
        // if configured and in debug mode, a ManifestWatcher watches App_Plugins folders for
        // package.manifest chances and restarts the application on any change
        private ManifestWatcher _mw;

        public void Initialize(IRuntimeState runtime)
        {
            if (runtime.Level < RuntimeLevel.Run || runtime.Debug == false) return;

            //if (ApplicationContext.Current.IsConfigured == false || GlobalSettings.DebugMode == false)
            //    return;

            var appPlugins = IOHelper.MapPath("~/App_Plugins/");
            if (Directory.Exists(appPlugins) == false) return;

            _mw = new ManifestWatcher(Current.Logger);
            _mw.Start(Directory.GetDirectories(appPlugins));
        }

        public override void Terminate()
        {
            _mw?.Dispose();
            _mw = null;
        }
    }
}