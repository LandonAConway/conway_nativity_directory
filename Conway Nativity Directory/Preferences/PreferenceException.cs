using System;

namespace Conway_Nativity_Directory
{
    public class PreferenceException : Exception
    {
        public PreferenceException(string message, IPreference preference)
        {
            this.message = message;
            this.preference = preference;
        }

        private string message;
        public new string Message
        {
            get
            {
                if (String.IsNullOrEmpty(message))
                    return base.Message;
                else
                    return message;
            }
        }

        private IPreference preference;
        public IPreference Preference { get { return preference; } }
    }
}