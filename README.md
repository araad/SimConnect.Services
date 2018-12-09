# SimConnect.Services
Library that contains service implementation part of an FSX add-on server using SimConnect SDK

Set the following environment variables:
* SIMCON_ASSEMBLY_FOLDER: points to a shared folder that will be used to place all dlls that can be used for packaging or copying to other machines
* SIMCON_RUNTIME_FOLDER: points to a shared folder that will be used to place all dlls that will be used by Visual Studio during runtime

## Dependencies
* SimConnect SDK
* log4net
* [SimConnect.Core](https://github.com/araad/SimConnect.Core)
* [SimConnect.ServiceContracts](https://github.com/araad/SimConnect.ServiceContracts)
