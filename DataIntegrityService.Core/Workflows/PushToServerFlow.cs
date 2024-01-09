using DataIntegrityService.Core.Logging;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Models.Interfaces;
using DataIntegrityService.Core.Services.Interfaces;
using DataIntegrityService.Core.Services.Local;
using DataIntegrityService.Core.Workflows.Interfaces;

namespace DataIntegrityService.Core.Workflows
{
    public class PushToServerFlow : IWorkflowService
  {
    public string Key => "PushToServer";

    public async Task<IActionResponse> ExecuteNonGeneric(DataChangeTrackingModel message, IDataService dataService, CancellationToken cancellationToken, string typeName)
    {
      Logger.Info("PushToServer", "PushToServer running...");
      Logger.Info("PushToServer", $"Resolving type '{typeName}'...");

      Type dataType = Type.GetType(typeName);

      // Call the generic method indirectly
      var method = typeof(PushToServerFlow).GetMethod("Execute").MakeGenericMethod(dataType);
      var resultTask = (Task<IActionResponse>)method.Invoke(this, new object[] { message, dataService, cancellationToken });

      return await resultTask;
    }

    public async Task<IActionResponse> Execute<T>(DataChangeTrackingModel message, IDataService dataService, CancellationToken cancellationToken) where T : IDataModel
    {
      try
      {
        if (dataService.IsInitialised)
        {
          if (message.Action == ChangeAction.Delete.ToString())
          {
            Logger.Info("PushToServer", $"Deleting model on server...");
            return await dataService.DeleteFromServer(message.ItemKey, cancellationToken);
          }
          else
          {
            Logger.Info("PushToServer", $"Data service '{dataService.Key}' initialised, fetching model from local device...");

            // fetch model from local store...
            var dataModel = dataService.GetLocal<T>(message.ItemKey);

            if (dataModel != null)
            {
              Logger.Info("PushToServer", "Model received, looking to perform any necessary transformations...");

              // perform any data tranformation before attempting to push data to server...
              var data = dataService.TransformData(dataModel);

              if (message.Action == ChangeAction.Create.ToString())
              {
                Logger.Info("PushToServer", $"Creating model on server...");
                return await dataService.CreateOnServer(data, cancellationToken);
              }
              else
              {
                Logger.Info("PushToServer", $"Updating model on server...");
                return await dataService.UpdateOnServer(data, cancellationToken);
              }
            }
            else
            {
              Logger.Error("PushToServer", $"Local data model '{message.DatasetName}' not found with key {message.ItemKey}. Unable to push to server.");
              return new ActionResponse() { ActionSucceeded = false };
            }
          }

          //CurrentRunState.BytesDownloaded += Utilities.GetObjectSize(qas);
          //dataSet.Count = qas.Count;
        }
        else
          throw new InvalidOperationException("Please initialise 'PushToServer' before calling 'Execute'.");
      }
      catch (Exception ex)
      {
        Logger.Error("PushToServer", ex.ToString());
        throw;
      }
    }
  }
}
