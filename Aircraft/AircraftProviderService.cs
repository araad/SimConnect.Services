using System;
using System.ServiceModel;
using SIM.Connect.Aircraft;
using SIM.Connect.CallbackContracts.Aircraft;
using SIM.Connect.Common;
using SIM.Connect.ServiceContracts.Aircraft;

namespace SIM.Connect
{
	public partial class SimconnectService : IAircraftProviderService
	{
		void IAircraftProviderService.Subscribe_ProviderService()
		{
			SimLogger.Log(LogMode.Info, "IAircraftProviderService", "Subscribing to provider service");

			var wCallback = OperationContext.Current.GetCallbackChannel<IAircraftProviderCallback>();

			CallbackChannelManager.Instance.AddCallbackChannel("IAircraftProviderCallback", wCallback);
			AircraftProvider.Instance.PropertyChanged += new PropertyValueChangedEventHandler(AircraftProvider_PropertyChanged);

			// First Pass
			wCallback.AircraftTitle_ValueChanged(AircraftProvider.Instance.AircraftTitleProp);
            SimLogger.Log(LogMode.Info, "IAircraftProviderService", string.Format("AircraftTitle_ValueChanged: {0}", AircraftProvider.Instance.AircraftTitleProp));

			wCallback.AircraftTotalWeight_ValueChanged(AircraftProvider.Instance.AircraftTotalWeightProp);
            SimLogger.Log(LogMode.Info, "IAircraftProviderService", string.Format("AircraftTotalWeight_ValueChanged: {0}", AircraftProvider.Instance.AircraftTotalWeightProp));

		}

		void IAircraftProviderService.Unsubscribe_ProviderService()
		{
			SimLogger.Log(LogMode.Info, "IAircraftProviderService", "Unsubscribing from provider service");

            AircraftProvider.Instance.PropertyChanged -= new PropertyValueChangedEventHandler(AircraftProvider_PropertyChanged);
            CallbackChannelManager.Instance.RemoveCallbackChannel("IAircraftProviderCallback");
		}

		void AircraftProvider_PropertyChanged(object sender, PropertyValueChangedEventArgs e)
		{
			 try
            {
                IAircraftProviderCallback wCallback = CallbackChannelManager.Instance.GetCallbackChannel("IAircraftProviderCallback") as IAircraftProviderCallback;
                    switch(e.Name)
                    {
                        case AircraftProvider.AircraftTitleKey:
                            wCallback.AircraftTitle_ValueChanged(e.Value);
                            SimLogger.Log(LogMode.Info, "IAircraftProviderService", string.Format("AircraftTitle_ValueChanged: {0}", e.Value));
                            break;
                        case AircraftProvider.AircraftTotalWeightKey:
                            wCallback.AircraftTotalWeight_ValueChanged(e.Value);
                            SimLogger.Log(LogMode.Info, "IAircraftProviderService", string.Format("AircraftTotalWeight_ValueChanged: {0}", e.Value));
                            break;
                        default:
                            SimLogger.Log(LogMode.Warn, "IAircraftProviderService", string.Format("{0} value changed not published", e.Name));
                            break;
                    }
            }
            catch (Exception ex)
            {
                SimLogger.Log(LogMode.Error, "IAircraftProviderService", "Error while calling back subscriber with <properyName>_ValueChanged", ex.Message, ex.StackTrace);
            }
		}
	}
}
