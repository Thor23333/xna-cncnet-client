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

    public static class LocalizationManager
    {

        //public static bool SetLanguage(string languageId)
        //{
        //    return 
        //}
        public const string Default_Lang = "en-US";
        public const string Default_Lang_UIName = "English";
        public const string LANG_KEY = "Localisation";
        public static string Default_Label = LocaleKey.Default_Label.Lang();
        public static Dictionary<string, LanguageInfo> GetAllLocaleTypeFromIni()
        {
            var LanguageList = new Dictionary<string, LanguageInfo>();
            LanguageList.Add(Default_Lang, new LanguageInfo(Default_Lang_UIName + Default_Label, false));

            var dirInfo = new DirectoryInfo(ProgramConstants.GetLocalePath());
            var dirList = dirInfo.GetFiles("*.ini", SearchOption.AllDirectories).ToList();
            foreach (var dir in dirList)
            {
                var dirFullName = dir.FullName;
                var dirName = Path.GetFileNameWithoutExtension(dirFullName); 
                var iniFile = new CCIniFile(dirFullName);
                List<string> keys = iniFile.GetSectionKeys(dirName);
                if (dirName == Default_Lang)
                {
                    LanguageList[Default_Lang] = new LanguageInfo(iniFile.GetStringValue(dirName, "UIName", Default_Lang_UIName) + Default_Label, false);
                }
                else if (keys != null)
                {
                    var langInfo = new LanguageInfo(iniFile.GetStringValue(dirName, "UIName", String.Empty), iniFile.GetBooleanValue(dirName, "Hidden", false));
                    LanguageList.Add(dirName, langInfo);
                }
            }
            return LanguageList;
        }
        public static void OutputDefaultLanguage(object sender, EventArgs e)
        {
            var path = ProgramConstants.GetLocalePath() + Default_Lang + ".ini";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            var inifile = new CCIniFile(path);
            inifile.AddSection(Default_Lang);
            inifile.SetStringValue(Default_Lang, "UIName", Default_Lang_UIName);
            inifile.AddSection(LANG_KEY);
            foreach (var pair in LocalizationLabel.defaultLocale)
            {
                inifile.SetStringValue(LANG_KEY, pair.Key.ToString(), pair.Value);
            }
        }
        /// <summary>
        /// 获取对应语言的字符串并格式化
        /// Get Locale String And Format it
        /// </summary>
        public static string Lang(this LocaleKey key, params object[] args)  => LocalizationLabel.Instance?.GetLocaleValue(key).Format(args);
    }

    public class LanguageInfo
    {
        public bool Hidden { get; }
        public string UIName { get; }
        public LanguageInfo(string name, bool hidden)
        {
            UIName = name;
            Hidden = hidden;
        }
    }
}
