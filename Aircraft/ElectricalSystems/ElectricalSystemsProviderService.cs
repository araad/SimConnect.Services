using System;
using System.ServiceModel;
using SIM.Connect.Aircraft.ElectricalSystems;
using SIM.Connect.CallbackContracts.Aircraft.ElectricalSystems;
using SIM.Connect.Common;
using SIM.Connect.ServiceContracts.Aircraft.ElectricalSystems;

namespace SIM.Connect
{
	public partial class SimconnectService : IElectricalSystemsProviderService
	{
		void IElectricalSystemsProviderService.Subscribe_ProviderService()
		{
			SimLogger.Log(LogMode.Info, "IElectricalSystemsProviderService", "Subscribing to provider service");

			var wCallback = OperationContext.Current.GetCallbackChannel<IElectricalSystemsProviderCallback>();

			CallbackChannelManager.Instance.AddCallbackChannel("IElectricalSystemsProviderCallback", wCallback);
			ElectricalSystemsProvider.Instance.PropertyChanged += new PropertyValueChangedEventHandler(ElectricalSystemsProvider_PropertyChanged);

			// First Pass
			wCallback.ElectricalMasterBattery_ValueChanged(ElectricalSystemsProvider.Instance.ElectricalMasterBatteryProp);
            SimLogger.Log(LogMode.Info, "IElectricalSystemsProviderService", string.Format("ElectricalMasterBattery_ValueChanged: {0}", ElectricalSystemsProvider.Instance.ElectricalMasterBatteryProp));

		}

		void IElectricalSystemsProviderService.Unsubscribe_ProviderService()
		{
			SimLogger.Log(LogMode.Info, "IElectricalSystemsProviderService", "Unsubscribing from provider service");

            ElectricalSystemsProvider.Instance.PropertyChanged -= new PropertyValueChangedEventHandler(ElectricalSystemsProvider_PropertyChanged);
            CallbackChannelManager.Instance.RemoveCallbackChannel("IElectricalSystemsProviderCallback");
		}

		void IElectricalSystemsProviderService.SetElectricalMasterBattery(object newValue)
		{
			ElectricalSystemsProvider.Instance.SetElectricalMasterBattery((bool)newValue);
		}

		void ElectricalSystemsProvider_PropertyChanged(object sender, PropertyValueChangedEventArgs e)
		{
			 try
            {
                IElectricalSystemsProviderCallback wCallback = CallbackChannelManager.Instance.GetCallbackChannel("IElectricalSystemsProviderCallback") as IElectricalSystemsProviderCallback;
                    switch(e.Name)
                    {
                        case ElectricalSystemsProvider.ElectricalMasterBatteryKey:
                            wCallback.ElectricalMasterBattery_ValueChanged(e.Value);
                            SimLogger.Log(LogMode.Info, "IElectricalSystemsProviderService", string.Format("ElectricalMasterBattery_ValueChanged: {0}", e.Value));
                            break;
                        default:
                            SimLogger.Log(LogMode.Warn, "IElectricalSystemsProviderService", string.Format("{0} value changed not published", e.Name));
                            break;
                    }
            }
            catch (Exception ex)
            {
                SimLogger.Log(LogMode.Error, "IElectricalSystemsProviderService", "Error while calling back subscriber with <properyName>_ValueChanged", ex.Message, ex.StackTrace);
            }
		}
	}
}
