using ClientCore;
using Rampastring.Tools;
using Rampastring.XNAUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ClientGUI
{
    /// <summary>
    /// A GUI creator that also includes ClientGUI's custom controls in addition
    /// to the controls of Rampastring.XNAUI.
    /// </summary>
    public class LocalizationLabel
    {
        public static LocalizationLabel current = new LocalizationLabel();

        private const string LANG_KEY = "Localisation";
        public string Language;
        public readonly string Default_Lang = "en-US";
        private Dictionary<string, string> LanguageList = new Dictionary<string, string>();
        private LocaleDictionary locale = new LocaleDictionary();
        private readonly LocaleDictionary defaultLocale = new LocaleDictionary
        {
            [LocaleKey.MainMenu] = "Main Menu",
        };

        public LocalizationLabel()
        {
            Initialize();
        }
        public LocaleValue this[LocaleKey key]
        {
            get
            {
                return GetLocaleValue(key);
            }
        }
        private void GetAllLocaleTypeFromIni()
        {
            var dirInfo = new DirectoryInfo(ProgramConstants.GetLocalePath());
            var dirList = dirInfo.GetFiles("*.ini", SearchOption.AllDirectories).ToList();
            foreach (var dir in dirList)
            {
                var dirName = dir.Name;
                var dirFullName = dir.FullName;
                var iniFile = new CCIniFile(dirFullName);
                List<string> keys = iniFile.GetSectionKeys(dirName);
                if (keys != null)
                {
                    foreach (string key in keys)
					{
                        if(key == "displayName")
						{
                            LanguageList.Add(dirName, iniFile.GetStringValue(dirName, key, String.Empty));
                        }
					}

                }
            }
        }

		private void Initialize()
		{
            InitializeLocale();
            GetAllLocaleTypeFromIni();

        }

		private void InitializeLocale()
		{
            LanguageList.Clear();
            foreach(LocaleKey key in Enum.GetValues(typeof(LocaleKey)))
			{
				if (!locale.ContainsKey(key))
				{
                    locale.Add(key, String.Empty);
                }
			}


        }

        public LocaleValue GetLocaleValue(LocaleKey key)
        {
            if (locale.ContainsKey(key) && !string.IsNullOrEmpty(locale[key]))

            {
                return locale[key];
			}
			else
			{
                return defaultLocale[key];
			}
            
        }

        protected virtual void GetLocaleFromIni()
        {
            if (File.Exists(ProgramConstants.GetLocalePath() + Language + ".ini"))
                GetINILocale(new CCIniFile(ProgramConstants.GetLocalePath() + Language + ".ini"));
            else if (File.Exists(ProgramConstants.GetLocalePath() + Default_Lang + ".ini"))
                GetINILocale(new CCIniFile(ProgramConstants.GetLocalePath() + Default_Lang + ".ini"));
        }

        /// <summary>
        /// Reads Locales from an INI file.
        /// </summary>
        protected virtual void GetINILocale(IniFile iniFile)
        {
            List<string> keys = iniFile.GetSectionKeys(LANG_KEY);

            if (keys != null)
            {
                foreach (string key in keys)
                {
					LocaleKey localeKey;
					if (Enum.TryParse(key, true, out localeKey))
					{
                        locale[localeKey] = iniFile.GetStringValue(LANG_KEY, key, String.Empty);
                    }
                }
            }
        }

    }

    internal class LocaleDictionary : Dictionary<LocaleKey, LocaleValue>
    {
        public void Add(LocaleKey key, string value)
		{
            base.Add(key, value);
		}
    }

	public class LocaleValue
	{
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
        private string _value;
		public LocaleValue(string value)
		{
			Value = value;
		}
		public static LocaleValue operator +(LocaleValue v1, string v2)
		{
            v1.Value += v2;
            return v1;
		}

		public static implicit operator LocaleValue(string v)
		{
            return new LocaleValue(v);
        }

		public static implicit operator string(LocaleValue v)
		{
            return v.Value;
        }
	}

    public enum LocaleKey
	{
        MainMenu,

    }

}
