using ClientCore;
using Rampastring.Tools;
using Rampastring.XNAUI;
using System.Collections.Generic;
using System.IO;

namespace DTAConfig.CustomSettings
{
    /// <summary>
    /// A dropdown that switches between multiple sets of files.
    /// </summary>
    public class CustomSettingFileDropDown : SettingDropDownBase, IFileSetting
    {
        public CustomSettingFileDropDown(WindowManager windowManager) : base(windowManager) { }

        private readonly List<List<FileSourceDestinationInfo>> itemFilesList = new List<List<FileSourceDestinationInfo>>();

        public bool CheckAvailability { get; private set; }
        public bool ResetUnavailableValue { get; private set; }

        public override void GetAttributes(IniFile iniFile)
        {
            base.GetAttributes(iniFile);

            var section = iniFile.GetSection(Name);
            if (section == null)
                return;

            for (int i = 0; i < Items.Count; i++)
                itemFilesList.Add(FileSourceDestinationInfo.ParseFSDInfoList(section, $"Item{i}File"));
        }

        public override void ParseAttributeFromINI(IniFile iniFile, string key, string value)
        {
            switch (key)
            {
                case "CheckAvailability":
                    CheckAvailability = Conversions.BooleanFromString(value, false);
                    return;
                case "ResetUnavailableValue":
                    ResetUnavailableValue = Conversions.BooleanFromString(value, false);
                    return;
            }

            base.ParseAttributeFromINI(iniFile, key, value);
        }

        public bool RefreshSetting()
        {
            int currentValue = SelectedIndex;

            if (CheckAvailability)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    Items[i].Selectable = true;
                    foreach (var fileInfo in itemFilesList[i])
                    {
                        if (!File.Exists(fileInfo.SourcePath))
                        {
                            Items[i].Selectable = false;
                            break;
                        }
                    }
                }

                if (ResetUnavailableValue && !Items[SelectedIndex].Selectable)
                    SelectedIndex = DefaultValue;
            }

            return SelectedIndex != currentValue;
        }

        public override void Load()
        {
            SelectedIndex = UserINISettings.Instance.GetValue(SettingSection, SettingKey, DefaultValue);
            originalState = SelectedIndex;
        }

        public override bool Save()
        {
            if (Items[SelectedIndex].Selectable)
            {
                for (int i = 0; i < itemFilesList.Count; i++)
                {
                    if (i != SelectedIndex)
                        itemFilesList[i].ForEach(f => f.Revert());
                }

                itemFilesList[SelectedIndex].ForEach(f => f.Apply());
            }
            else // selected item is unavailable, don't do anything
            {
                Logger.Log($"{nameof(CustomSettingFileDropDown)}: " +
                    $"The selected item ({Items[SelectedIndex].Text}) is unavailable in {Name}");
                return false;
            }

            UserINISettings.Instance.SetValue(SettingSection, SettingKey, SelectedIndex);
            return RestartRequired && (SelectedIndex != originalState);
        }
    }
}
