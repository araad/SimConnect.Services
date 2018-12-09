using System;
using System.ComponentModel;
using System.ServiceModel;
using SIM.Connect.CallbackContracts.Simconnect;
using SIM.Connect.ServiceContracts.Simconnect;
using SIM.Connect.Simconnect;
using SIM.Connect.Common;

namespace SIM.Connect
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public partial class SimconnectService : ISimconnectProviderService
    {
        void ISimconnectProviderService.Subscribe_ProviderService()
        {
            SimLogger.Log(LogMode.Info, "ISimconnectProviderService", "Subscribing to provider service");

            var wCallback = OperationContext.Current.GetCallbackChannel<ISimconnectProviderCallback>();

            CallbackChannelManager.Instance.AddCallbackChannel("ISimconnectProviderCallback", wCallback);
            SimconnectProvider.Instance.SimPropertyChanged += new PropertyValueChangedEventHandler(SimconnectProvider_PropertyChanged);
            
            // First pass            
            wCallback.SimName_ValueChanged(SimconnectProvider.Instance.SimName);
            SimLogger.Log(LogMode.Info, "ISimconnectProviderService", string.Format("SimName_ValueChanged: {0}", SimconnectProvider.Instance.SimName));
        }

        void ISimconnectProviderService.Unsubscribe_ProviderService()
        {
            SimLogger.Log(LogMode.Info, "ISimconnectProviderService", "Unsubscribing from provider service");

            SimconnectProvider.Instance.SimPropertyChanged -= new PropertyValueChangedEventHandler(SimconnectProvider_PropertyChanged);
            CallbackChannelManager.Instance.RemoveCallbackChannel("ISimconnectProviderCallback");
        }

        void ISimconnectProviderService.JoinSimulation()
        {
            //CallbackChannelManager.Instance.AddCallbackChannel("ISimInfoProviderCallback", OperationContext.Current.GetCallbackChannel<ISimInfoProviderCallback>());
            SimconnectProvider.Instance.JoinSimulation();
        }

        void ISimconnectProviderService.LeaveSimulation()
        {
            SimconnectProvider.Instance.LeaveSimulation();
        }

        void SimconnectProvider_PropertyChanged(object sender, PropertyValueChangedEventArgs e)
        {
            try
            {
                ISimconnectProviderCallback wCallback = CallbackChannelManager.Instance.GetCallbackChannel("ISimconnectProviderCallback") as ISimconnectProviderCallback;
                if (wCallback != null)
                {
                    switch (e.Name)
                    {
                        case SimconnectProvider.SimNameKey:
                            wCallback.SimName_ValueChanged((string)e.Value);
                            SimLogger.Log(LogMode.Info, "ISimconnectProviderService", string.Format("SimName_ValueChanged: {0}", e.Value));
                            break;
                        default:
                            SimLogger.Log(LogMode.Warn, "ISimconnectProviderService", string.Format("{0} value changed not published", e.Name));
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SimLogger.Log(LogMode.Info, "ISimconnectProviderService", "Error", ex.Message, ex.StackTrace);
            }
        }
    }
}
