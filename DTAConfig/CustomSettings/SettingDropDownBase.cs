using ClientGUI;
using Rampastring.Tools;
using Rampastring.XNAUI;
using Rampastring.XNAUI.XNAControls;

namespace DTAConfig.CustomSettings
{
    public abstract class SettingDropDownBase : XNAClientDropDown, IUserSetting
    {
        public SettingDropDownBase(WindowManager windowManager) : base(windowManager) { }

        public int DefaultValue { get; set; }
        public string SettingSection { get; set; }
        public string SettingKey { get; set; }
        public bool RestartRequired { get; set; }

        protected string defaultKeySuffix = "_SelectedIndex";
        protected int originalState;

        public override void ParseAttributeFromINI(IniFile iniFile, string key, string value)
        {
            switch (key)
            {
                case "Items":
                    string[] items = value.Split(',');
                    for (int i = 0; i < items.Length; i++)
                    {
                        XNADropDownItem item = new XNADropDownItem
                        {
                            Text = items[i]
                        };
                        AddItem(item);
                    }
                    return;
                case "DefaultValue":
                    DefaultValue = Conversions.IntFromString(value, 0);
                    return;
                case "SettingSection":
                    SettingSection = string.IsNullOrEmpty(value) ? "CustomSettings" : value;
                    return;
                case "SettingKey":
                    SettingKey = string.IsNullOrEmpty(value) ? $"{Name}{defaultKeySuffix}" : value;
                    return;
                case "RestartRequired":
                    RestartRequired = Conversions.BooleanFromString(value, false);
                    return;
            }

            base.ParseAttributeFromINI(iniFile, key, value);
        }

        public abstract void Load();

        public abstract bool Save();
    }
}