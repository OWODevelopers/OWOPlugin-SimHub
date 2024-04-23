﻿using System.Windows.Media;
using GameReaderCommon;
using OWOPluginSimHub.Application;
using OWOPluginSimHub.Domain;
using OWOPluginSimHub.Properties;
using SimHub.Plugins;

namespace OWOPluginSimHub.View
{
    [PluginDescription("OWO Haptics Integration")]
    [PluginAuthor("OWO")]
    [PluginName("OWO Plugin")]
    public class DataPluginDemo : IPlugin, IDataPlugin, IWPFSettingsV2
    {
        Plugin plugin;

        public PluginManager PluginManager { get; set; }
        public ImageSource PictureIcon => this.ToIcon(Resources.sdkmenuicon);
        public string LeftMenuTitle => "OWO plugin";

        public void DataUpdate(PluginManager pluginManager, ref GameData data)
        {
            plugin.Feel(WorldContext.From(data));
        }

        public System.Windows.Controls.Control GetWPFSettingsControl(PluginManager pluginManager)
        {
            return new SettingsControlDemo(this);
        }

        public void Init(PluginManager pluginManager)
        {
            plugin = new Plugin(new OWOHaptic());
            SimHub.Logging.Current.Info("OWO Plugin Initialized!");
        }
    
        public void End(PluginManager pluginManager)
        {
        }
    }
}