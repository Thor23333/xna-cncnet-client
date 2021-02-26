using System;
using ClientGUI;
using Rampastring.Tools;
using Rampastring.XNAUI;

namespace DTAConfig.CustomSettings
{
    public abstract class SettingCheckBoxBase : XNAClientCheckBox, IUserSetting
    {
        public SettingCheckBoxBase(WindowManager windowManager) : base(windowManager) { }

        public bool DefaultValue { get; set; }
        public string SettingSection { get; set; }
        public string SettingKey { get; set; }
        public bool RestartRequired { get; set; }

        private string _parentCheckBoxName;
        /// <summary>
        /// Name of parent check-box control.
        /// </summary>
        public string ParentCheckBoxName
        {
            get { return _parentCheckBoxName; }
            set
            {
                _parentCheckBoxName = value;
                UpdateParentCheckBox(FindParentCheckBox());
            }
        }

        private XNAClientCheckBox _parentCheckBox;
        /// <summary>
        /// Parent check-box control.
        /// </summary>
        public XNAClientCheckBox ParentCheckBox
        {
            get { return _parentCheckBox; }
            set
            {
                UpdateParentCheckBox(value);
                _parentCheckBoxName = _parentCheckBox != null ? _parentCheckBox.Name : null;
            }
        }

        /// <summary>
        /// Value required from parent check-box control if set.
        /// </summary>
        public bool ParentCheckBoxRequiredValue { get; set; } = true;

        protected string defaultKeySuffix = "_Checked";
        protected bool originalState;

        public override void ParseAttributeFromINI(IniFile iniFile, string key, string value)
        {
            switch (key)
            {
                case "Checked":
                case "DefaultValue":
                    DefaultValue = Conversions.BooleanFromString(value, false);
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
                case "ParentCheckBoxName":
                    ParentCheckBoxName = value;
                    return;
                case "ParentCheckBoxRequiredValue":
                    ParentCheckBoxRequiredValue = Conversions.BooleanFromString(value, true);
                    return;
            }

            base.ParseAttributeFromINI(iniFile, key, value);
        }

        public abstract void Load();

        public abstract bool Save();


        private XNAClientCheckBox FindParentCheckBox()
        {
            if (string.IsNullOrEmpty(ParentCheckBoxName))
            {
                return null;
            }

            foreach (var control in Parent.Children)
            {
                if (control is XNAClientCheckBox && control.Name == ParentCheckBoxName)
                {
                    return control as XNAClientCheckBox;
                }
            }

            return null;
        }

        private void UpdateParentCheckBox(XNAClientCheckBox parentCheckBox)
        {
            if (ParentCheckBox != null)
                ParentCheckBox.CheckedChanged -= ParentCheckBox_CheckedChanged;

            _parentCheckBox = parentCheckBox;

            if (ParentCheckBox != null)
                ParentCheckBox.CheckedChanged += ParentCheckBox_CheckedChanged;
        }

        private void ParentCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as XNAClientCheckBox).Checked == ParentCheckBoxRequiredValue)
                AllowChecking = true;
            else
            {
                AllowChecking = false;
                Checked = false;
            }
        }

    }
}