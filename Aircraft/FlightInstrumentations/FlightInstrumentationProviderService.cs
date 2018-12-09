using System;
using System.ServiceModel;
using SIM.Connect.Aircraft.FlightInstrumentation;
using SIM.Connect.CallbackContracts.Aircraft.FlightInstrumentation;
using SIM.Connect.Common;
using SIM.Connect.ServiceContracts.Aircraft.FlightInstrumentation;

namespace SIM.Connect
{
	public partial class SimconnectService : IFlightInstrumentationProviderService
	{
		void IFlightInstrumentationProviderService.Subscribe_ProviderService()
		{
			SimLogger.Log(LogMode.Info, "IFlightInstrumentationProviderService", "Subscribing to provider service");

			var wCallback = OperationContext.Current.GetCallbackChannel<IFlightInstrumentationProviderCallback>();

			CallbackChannelManager.Instance.AddCallbackChannel("IFlightInstrumentationProviderCallback", wCallback);
			FlightInstrumentationProvider.Instance.PropertyChanged += new PropertyValueChangedEventHandler(FlightInstrumentationProvider_PropertyChanged);

			// First Pass
			wCallback.IndicatedAirspeed_ValueChanged(FlightInstrumentationProvider.Instance.IndicatedAirspeedProp);
            SimLogger.Log(LogMode.Info, "IFlightInstrumentationProviderService", string.Format("IndicatedAirspeed_ValueChanged: {0}", FlightInstrumentationProvider.Instance.IndicatedAirspeedProp));

		}

		void IFlightInstrumentationProviderService.Unsubscribe_ProviderService()
		{
			SimLogger.Log(LogMode.Info, "IFlightInstrumentationProviderService", "Unsubscribing from provider service");

            FlightInstrumentationProvider.Instance.PropertyChanged -= new PropertyValueChangedEventHandler(FlightInstrumentationProvider_PropertyChanged);
            CallbackChannelManager.Instance.RemoveCallbackChannel("IFlightInstrumentationProviderCallback");
		}

		void IFlightInstrumentationProviderService.SetIndicatedAirspeed(object newValue)
		{
			FlightInstrumentationProvider.Instance.SetIndicatedAirspeed((double)newValue);
		}

		void FlightInstrumentationProvider_PropertyChanged(object sender, PropertyValueChangedEventArgs e)
		{
			 try
            {
                IFlightInstrumentationProviderCallback wCallback = CallbackChannelManager.Instance.GetCallbackChannel("IFlightInstrumentationProviderCallback") as IFlightInstrumentationProviderCallback;
                    switch(e.Name)
                    {
                        case FlightInstrumentationProvider.IndicatedAirspeedKey:
                            wCallback.IndicatedAirspeed_ValueChanged(e.Value);
                            SimLogger.Log(LogMode.Info, "IFlightInstrumentationProviderService", string.Format("IndicatedAirspeed_ValueChanged: {0}", e.Value));
                            break;
                        default:
                            SimLogger.Log(LogMode.Warn, "IFlightInstrumentationProviderService", string.Format("{0} value changed not published", e.Name));
                            break;
                    }
            }
            catch (Exception ex)
            {
                SimLogger.Log(LogMode.Error, "IFlightInstrumentationProviderService", "Error while calling back subscriber with <properyName>_ValueChanged", ex.Message, ex.StackTrace);
            }
		}
	}
}
