using System.Windows.Media;
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
        PluginWrapper pluginWrapper;
        
        public PluginManager PluginManager { get; set; }
        public ImageSource PictureIcon => this.ToIcon(Resources.sdkmenuicon);
        public string LeftMenuTitle => "OWO plugin";

        public void DataUpdate(PluginManager pluginManager, ref GameData data)
        {
            pluginWrapper.data = WorldContext.From(data);
        }

        public System.Windows.Controls.Control GetWPFSettingsControl(PluginManager pluginManager)
        {
            return new SettingsControlDemo(this);
        }

        public void Init(PluginManager pluginManager)
        {
            pluginWrapper = new PluginWrapper(new Plugin(new OWOHaptic()));
            SimHub.Logging.Current.Info("OWO Plugin Initialized!");
            _ = pluginWrapper.Feel();
        }
    
        public void End(PluginManager pluginManager)
        {
        }
    }
}