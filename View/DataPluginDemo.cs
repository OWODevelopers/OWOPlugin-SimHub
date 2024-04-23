using GameReaderCommon;
using SimHub.Plugins;
using System.Windows.Media;
using OWOPlugin;
using User.PluginSdkDemo;


[PluginDescription("OWO Haptics Integration")]
[PluginAuthor("OWO")]
[PluginName("OWO Plugin")]
public class DataPluginDemo : IPlugin, IDataPlugin, IWPFSettingsV2
{
    Plugin plugin;

    public PluginManager PluginManager { get; set; }
    public ImageSource PictureIcon => this.ToIcon(User.PluginSdkDemo.Properties.Resources.sdkmenuicon);
    public string LeftMenuTitle => "OWO plugin";

    public void DataUpdate(PluginManager pluginManager, ref GameData data)
    {
        plugin.Data = WorldContext.From(data);
        plugin.UpdateFeelingBasedOnWorld();
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