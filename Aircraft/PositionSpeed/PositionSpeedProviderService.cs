using System;
using System.ServiceModel;
using SIM.Connect.Aircraft.PositionSpeed;
using SIM.Connect.CallbackContracts.Aircraft.PositionSpeed;
using SIM.Connect.Common;
using SIM.Connect.ServiceContracts.Aircraft.PositionSpeed;

namespace SIM.Connect
{
	public partial class SimconnectService : IPositionSpeedProviderService
	{
		void IPositionSpeedProviderService.Subscribe_ProviderService()
		{
			SimLogger.Log(LogMode.Info, "IPositionSpeedProviderService", "Subscribing to provider service");

			var wCallback = OperationContext.Current.GetCallbackChannel<IPositionSpeedProviderCallback>();

			CallbackChannelManager.Instance.AddCallbackChannel("IPositionSpeedProviderCallback", wCallback);
			PositionSpeedProvider.Instance.PropertyChanged += new PropertyValueChangedEventHandler(PositionSpeedProvider_PropertyChanged);

			// First Pass
			wCallback.Latitude_ValueChanged(PositionSpeedProvider.Instance.LatitudeProp);
            SimLogger.Log(LogMode.Info, "IPositionSpeedProviderService", string.Format("Latitude_ValueChanged: {0}", PositionSpeedProvider.Instance.LatitudeProp));

			wCallback.Longitude_ValueChanged(PositionSpeedProvider.Instance.LongitudeProp);
            SimLogger.Log(LogMode.Info, "IPositionSpeedProviderService", string.Format("Longitude_ValueChanged: {0}", PositionSpeedProvider.Instance.LongitudeProp));

			wCallback.MSLAltitude_ValueChanged(PositionSpeedProvider.Instance.MSLAltitudeProp);
            SimLogger.Log(LogMode.Info, "IPositionSpeedProviderService", string.Format("MSLAltitude_ValueChanged: {0}", PositionSpeedProvider.Instance.MSLAltitudeProp));

			wCallback.AGLAltitude_ValueChanged(PositionSpeedProvider.Instance.AGLAltitudeProp);
            SimLogger.Log(LogMode.Info, "IPositionSpeedProviderService", string.Format("AGLAltitude_ValueChanged: {0}", PositionSpeedProvider.Instance.AGLAltitudeProp));

			wCallback.MagneticHeading_ValueChanged(PositionSpeedProvider.Instance.MagneticHeadingProp);
            SimLogger.Log(LogMode.Info, "IPositionSpeedProviderService", string.Format("MagneticHeading_ValueChanged: {0}", PositionSpeedProvider.Instance.MagneticHeadingProp));

			wCallback.TrueHeading_ValueChanged(PositionSpeedProvider.Instance.TrueHeadingProp);
            SimLogger.Log(LogMode.Info, "IPositionSpeedProviderService", string.Format("TrueHeading_ValueChanged: {0}", PositionSpeedProvider.Instance.TrueHeadingProp));

		}

		void IPositionSpeedProviderService.Unsubscribe_ProviderService()
		{
			SimLogger.Log(LogMode.Info, "IPositionSpeedProviderService", "Unsubscribing from provider service");

            PositionSpeedProvider.Instance.PropertyChanged -= new PropertyValueChangedEventHandler(PositionSpeedProvider_PropertyChanged);
            CallbackChannelManager.Instance.RemoveCallbackChannel("IPositionSpeedProviderCallback");
		}

		void IPositionSpeedProviderService.SetLatitude(object newValue)
		{
			PositionSpeedProvider.Instance.SetLatitude((double)newValue);
		}

		void IPositionSpeedProviderService.SetLongitude(object newValue)
		{
			PositionSpeedProvider.Instance.SetLongitude((double)newValue);
		}

		void IPositionSpeedProviderService.SetMSLAltitude(object newValue)
		{
			PositionSpeedProvider.Instance.SetMSLAltitude((double)newValue);
		}

		void IPositionSpeedProviderService.SetAGLAltitude(object newValue)
		{
			PositionSpeedProvider.Instance.SetAGLAltitude((double)newValue);
		}

		void IPositionSpeedProviderService.SetMagneticHeading(object newValue)
		{
			PositionSpeedProvider.Instance.SetMagneticHeading((double)newValue);
		}

		void IPositionSpeedProviderService.SetTrueHeading(object newValue)
		{
			PositionSpeedProvider.Instance.SetTrueHeading((double)newValue);
		}

		void PositionSpeedProvider_PropertyChanged(object sender, PropertyValueChangedEventArgs e)
		{
			 try
            {
                IPositionSpeedProviderCallback wCallback = CallbackChannelManager.Instance.GetCallbackChannel("IPositionSpeedProviderCallback") as IPositionSpeedProviderCallback;
                    switch(e.Name)
                    {
                        case PositionSpeedProvider.LatitudeKey:
                            wCallback.Latitude_ValueChanged(e.Value);
                            SimLogger.Log(LogMode.Info, "IPositionSpeedProviderService", string.Format("Latitude_ValueChanged: {0}", e.Value));
                            break;
                        case PositionSpeedProvider.LongitudeKey:
                            wCallback.Longitude_ValueChanged(e.Value);
                            SimLogger.Log(LogMode.Info, "IPositionSpeedProviderService", string.Format("Longitude_ValueChanged: {0}", e.Value));
                            break;
                        case PositionSpeedProvider.MSLAltitudeKey:
                            wCallback.MSLAltitude_ValueChanged(e.Value);
                            SimLogger.Log(LogMode.Info, "IPositionSpeedProviderService", string.Format("MSLAltitude_ValueChanged: {0}", e.Value));
                            break;
                        case PositionSpeedProvider.AGLAltitudeKey:
                            wCallback.AGLAltitude_ValueChanged(e.Value);
                            SimLogger.Log(LogMode.Info, "IPositionSpeedProviderService", string.Format("AGLAltitude_ValueChanged: {0}", e.Value));
                            break;
                        case PositionSpeedProvider.MagneticHeadingKey:
                            wCallback.MagneticHeading_ValueChanged(e.Value);
                            SimLogger.Log(LogMode.Info, "IPositionSpeedProviderService", string.Format("MagneticHeading_ValueChanged: {0}", e.Value));
                            break;
                        case PositionSpeedProvider.TrueHeadingKey:
                            wCallback.TrueHeading_ValueChanged(e.Value);
                            SimLogger.Log(LogMode.Info, "IPositionSpeedProviderService", string.Format("TrueHeading_ValueChanged: {0}", e.Value));
                            break;
                        default:
                            SimLogger.Log(LogMode.Warn, "IPositionSpeedProviderService", string.Format("{0} value changed not published", e.Name));
                            break;
                    }
            }
            catch (Exception ex)
            {
                SimLogger.Log(LogMode.Error, "IPositionSpeedProviderService", "Error while calling back subscriber with <properyName>_ValueChanged", ex.Message, ex.StackTrace);
            }
		}
	}
}
