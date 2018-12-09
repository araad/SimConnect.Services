using System;
using System.ServiceModel;
using SIM.Connect.Aircraft.Fuel;
using SIM.Connect.CallbackContracts.Aircraft.Fuel;
using SIM.Connect.Common;
using SIM.Connect.ServiceContracts.Aircraft.Fuel;

namespace SIM.Connect
{
	public partial class SimconnectService : IFuelProviderService
	{
		void IFuelProviderService.Subscribe_ProviderService()
		{
			SimLogger.Log(LogMode.Info, "IFuelProviderService", "Subscribing to provider service");

			var wCallback = OperationContext.Current.GetCallbackChannel<IFuelProviderCallback>();

			CallbackChannelManager.Instance.AddCallbackChannel("IFuelProviderCallback", wCallback);
			FuelProvider.Instance.PropertyChanged += new PropertyValueChangedEventHandler(FuelProvider_PropertyChanged);

			// First Pass
			wCallback.FuelTankCenterLevel_ValueChanged(FuelProvider.Instance.FuelTankCenterLevelProp);
            SimLogger.Log(LogMode.Info, "IFuelProviderService", string.Format("FuelTankCenterLevel_ValueChanged: {0}", FuelProvider.Instance.FuelTankCenterLevelProp));

			wCallback.FuelTankCenterQuantity_ValueChanged(FuelProvider.Instance.FuelTankCenterQuantityProp);
            SimLogger.Log(LogMode.Info, "IFuelProviderService", string.Format("FuelTankCenterQuantity_ValueChanged: {0}", FuelProvider.Instance.FuelTankCenterQuantityProp));

			wCallback.FuelTankCenterCapacity_ValueChanged(FuelProvider.Instance.FuelTankCenterCapacityProp);
            SimLogger.Log(LogMode.Info, "IFuelProviderService", string.Format("FuelTankCenterCapacity_ValueChanged: {0}", FuelProvider.Instance.FuelTankCenterCapacityProp));

			wCallback.FuelTotalQuantityWeight_ValueChanged(FuelProvider.Instance.FuelTotalQuantityWeightProp);
            SimLogger.Log(LogMode.Info, "IFuelProviderService", string.Format("FuelTotalQuantityWeight_ValueChanged: {0}", FuelProvider.Instance.FuelTotalQuantityWeightProp));

			wCallback.FuelTotalQuantity_ValueChanged(FuelProvider.Instance.FuelTotalQuantityProp);
            SimLogger.Log(LogMode.Info, "IFuelProviderService", string.Format("FuelTotalQuantity_ValueChanged: {0}", FuelProvider.Instance.FuelTotalQuantityProp));

			wCallback.FuelTotalCapacity_ValueChanged(FuelProvider.Instance.FuelTotalCapacityProp);
            SimLogger.Log(LogMode.Info, "IFuelProviderService", string.Format("FuelTotalCapacity_ValueChanged: {0}", FuelProvider.Instance.FuelTotalCapacityProp));

		}

		void IFuelProviderService.Unsubscribe_ProviderService()
		{
			SimLogger.Log(LogMode.Info, "IFuelProviderService", "Unsubscribing from provider service");

            FuelProvider.Instance.PropertyChanged -= new PropertyValueChangedEventHandler(FuelProvider_PropertyChanged);
            CallbackChannelManager.Instance.RemoveCallbackChannel("IFuelProviderCallback");
		}

		void IFuelProviderService.SetFuelTankCenterLevel(object newValue)
		{
			FuelProvider.Instance.SetFuelTankCenterLevel((double)newValue);
		}

		void IFuelProviderService.SetFuelTankCenterQuantity(object newValue)
		{
			FuelProvider.Instance.SetFuelTankCenterQuantity((double)newValue);
		}

		void FuelProvider_PropertyChanged(object sender, PropertyValueChangedEventArgs e)
		{
			 try
            {
                IFuelProviderCallback wCallback = CallbackChannelManager.Instance.GetCallbackChannel("IFuelProviderCallback") as IFuelProviderCallback;
                    switch(e.Name)
                    {
                        case FuelProvider.FuelTankCenterLevelKey:
                            wCallback.FuelTankCenterLevel_ValueChanged(e.Value);
                            SimLogger.Log(LogMode.Info, "IFuelProviderService", string.Format("FuelTankCenterLevel_ValueChanged: {0}", e.Value));
                            break;
                        case FuelProvider.FuelTankCenterQuantityKey:
                            wCallback.FuelTankCenterQuantity_ValueChanged(e.Value);
                            SimLogger.Log(LogMode.Info, "IFuelProviderService", string.Format("FuelTankCenterQuantity_ValueChanged: {0}", e.Value));
                            break;
                        case FuelProvider.FuelTankCenterCapacityKey:
                            wCallback.FuelTankCenterCapacity_ValueChanged(e.Value);
                            SimLogger.Log(LogMode.Info, "IFuelProviderService", string.Format("FuelTankCenterCapacity_ValueChanged: {0}", e.Value));
                            break;
                        case FuelProvider.FuelTotalQuantityWeightKey:
                            wCallback.FuelTotalQuantityWeight_ValueChanged(e.Value);
                            SimLogger.Log(LogMode.Info, "IFuelProviderService", string.Format("FuelTotalQuantityWeight_ValueChanged: {0}", e.Value));
                            break;
                        case FuelProvider.FuelTotalQuantityKey:
                            wCallback.FuelTotalQuantity_ValueChanged(e.Value);
                            SimLogger.Log(LogMode.Info, "IFuelProviderService", string.Format("FuelTotalQuantity_ValueChanged: {0}", e.Value));
                            break;
                        case FuelProvider.FuelTotalCapacityKey:
                            wCallback.FuelTotalCapacity_ValueChanged(e.Value);
                            SimLogger.Log(LogMode.Info, "IFuelProviderService", string.Format("FuelTotalCapacity_ValueChanged: {0}", e.Value));
                            break;
                        default:
                            SimLogger.Log(LogMode.Warn, "IFuelProviderService", string.Format("{0} value changed not published", e.Name));
                            break;
                    }
            }
            catch (Exception ex)
            {
                SimLogger.Log(LogMode.Error, "IFuelProviderService", "Error while calling back subscriber with <properyName>_ValueChanged", ex.Message, ex.StackTrace);
            }
		}
	}
}
