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
    /// 
    /// </summary>
    public class LocaleDictionary : Dictionary<LocaleKey, LocaleValue>
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
        public string Format(params object[] args)
        {
            string output;
            output = Value;
            try
            {
                output = string.Format(Value, args);
            }
            catch (FormatException e)
            {
                Logger.Log(e.ToString());
                return output;
            }
            try
            {
                output = output.Replace(LocalizationLabel.newLinePH, Environment.NewLine);
            }
            catch (FormatException e)
            {
                Logger.Log(e.ToString());
                return output;
            }
            return output;
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
}
