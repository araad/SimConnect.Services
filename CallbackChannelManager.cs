using System;
using System.Collections.Generic;
using System.Linq;

namespace SIM.Connect
{
    public class CallbackChannelManager
    {
        Dictionary<string, object> mCallbackChannels;

        private static volatile CallbackChannelManager sInstance;
        private static object sLocker = new object();

        static public CallbackChannelManager Instance
        {
            get
            {
                if (sInstance == null)
                {
                    lock (sLocker)
                    {
                        if (sInstance == null)
                        {
                            sInstance = new CallbackChannelManager();
                        }
                    }
                }

                return sInstance;
            }
        }

        private CallbackChannelManager()
        {
            this.mCallbackChannels = new Dictionary<string, object>();
        }

        public object GetCallbackChannel(string iCallbackChannelType)
        {
            foreach (var wCallbackCh in this.mCallbackChannels)
            {
                if (wCallbackCh.Key == iCallbackChannelType)
                {
                    return wCallbackCh.Value;
                }
            }

            return null;
        }

        public void AddCallbackChannel(string iCallbackChannelType, object iCallbackChannel)
        {
            if (this.mCallbackChannels.Keys.Contains(iCallbackChannelType))
            {
                this.mCallbackChannels.Remove(iCallbackChannelType);
            }
            try
            {
                this.mCallbackChannels.Add(iCallbackChannelType, iCallbackChannel);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void RemoveCallbackChannel(string iCallbackChannelType)
        {
            this.mCallbackChannels.Remove(iCallbackChannelType);
        }
    }
}
