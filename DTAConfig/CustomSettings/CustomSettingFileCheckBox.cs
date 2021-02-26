using ClientCore;
using ClientGUI;
using Rampastring.Tools;
using Rampastring.XNAUI;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DTAConfig.CustomSettings
{
    /// <summary>
    /// A check-box that toggles between two sets of files and saves the setting to user settings file.
    /// </summary>
    public class CustomSettingFileCheckBox : SettingCheckBoxBase, IFileSetting
    {
        public CustomSettingFileCheckBox(WindowManager windowManager) : base(windowManager) { }

        public bool CheckAvailability { get; set; }
        public bool ResetUnavailableValue { get; set; }

        private List<FileSourceDestinationInfo> enabledFiles = new List<FileSourceDestinationInfo>();
        private List<FileSourceDestinationInfo> disabledFiles = new List<FileSourceDestinationInfo>();

        private bool EnabledFilesComplete => enabledFiles.All(f => File.Exists(f.SourcePath));
        private bool DisabledFilesComplete => disabledFiles.All(f => File.Exists(f.SourcePath));

        public override void GetAttributes(IniFile iniFile)
        {
            base.GetAttributes(iniFile);

            var section = iniFile.GetSection(Name);

            if (section == null)
                return;

            enabledFiles = FileSourceDestinationInfo.ParseFSDInfoList(section, "EnabledFile");
            disabledFiles = FileSourceDestinationInfo.ParseFSDInfoList(section, "DisabledFile");
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
            bool currentValue = Checked;

            if (CheckAvailability)
            {
                Enabled = true;
                
                if (ResetUnavailableValue)
                {
                    if (DisabledFilesComplete != EnabledFilesComplete)
                        Checked = EnabledFilesComplete;
                    else if (!DisabledFilesComplete && !EnabledFilesComplete)
                        Checked = DefaultValue;
                }
            }

            return Checked != currentValue;
        }

        public override void Load()
        {
            Checked = UserINISettings.Instance.GetValue(SettingSection, SettingKey, DefaultValue);
            originalState = Checked;
        }

        public override bool Save()
        {
            bool canBeChecked = !CheckAvailability || EnabledFilesComplete;
            bool canBeUnchecked = !CheckAvailability || DisabledFilesComplete;

            if (Checked && canBeChecked)
            {
                disabledFiles.ForEach(f => f.Revert());
                enabledFiles.ForEach(f => f.Apply());
            }
            else if (!Checked && canBeUnchecked)
            {
                enabledFiles.ForEach(f => f.Revert());
                disabledFiles.ForEach(f => f.Apply());
            }
            else // selected state is unavailable, don't do anything
            {
                Logger.Log($"{nameof(CustomSettingFileCheckBox)}: " +
                    $"The selected state ({Checked}) is unavailable in {Name}");
                return false;
            }

            UserINISettings.Instance.SetValue(SettingSection, SettingKey, Checked);
            return RestartRequired && (Checked != originalState);
        }
    }
}
