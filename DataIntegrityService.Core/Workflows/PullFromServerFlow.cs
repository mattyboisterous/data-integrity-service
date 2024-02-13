using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Logging;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Models.Interfaces;
using DataIntegrityService.Core.Services.Interfaces;
using DataIntegrityService.Core.Services.Local;
using DataIntegrityService.Core.Workflows.Interfaces;

namespace DataIntegrityService.Core.Workflows
{
  public class PullFromServerFlow : IWorkflowService
  {
    public string Key => "PullFromServer";

    public async Task<IActionResponse> ExecuteNonGeneric(DataChangeTrackingModel message, IDataService dataService, CancellationToken cancellationToken, string typeName)
    {
      Logger.Info("PullFromServer", "Workflow 'PullFromServer' running...");
      Logger.Info("PullFromServer", $"Resolving type '{typeName}'...");

      Type dataType = Type.GetType(typeName);

      // Call the generic method indirectly
      var method = typeof(PullFromServerFlow).GetMethod("Execute").MakeGenericMethod(dataType);
      var resultTask = (Task<IActionResponse>)method.Invoke(this, new object[] { message, dataService, cancellationToken });

      return await resultTask;
    }

    public async Task<IActionResponse> Execute<T>(DataChangeTrackingModel message, IDataService dataService, CancellationToken cancellationToken) where T : IDataModel
    {
      try
      {
        if (dataService.IsInitialised)
        {
          Logger.Info("PullFromServer", $"Data service '{dataService.Key}' initialised, determining action to perform...");

          if (message.Action == ChangeAction.Delete.ToString())
          {
            Logger.Info("PullFromServer", $"Deleting data from local store...");
            dataService.DeleteLocal<T>(message.ItemKey);

            Logger.Info("PullFromServer", $"Work complete.");
            return new ActionResponse();
          }
          else
          {
            // fetch model from server...
            var dataResponse = await dataService.GetFromServer(message.ItemKey, cancellationToken);

            if (dataResponse.ActionSucceeded && dataResponse.Data != null)
            {
              Logger.Info("PullFromServer", "Model received, looking to perform any necessary transformations...");

              // perform any data tranformation before attempting to push data to local store...
              var data = dataService.TransformData(dataResponse.Data);

              if (message.Action == ChangeAction.Create.ToString())
              {
                Logger.Info("PullFromServer", $"Creating model on local store...");
                dataService.InsertLocal((T)data);
              }
              else
              {
                Logger.Info("PullFromServer", $"Updating model on local store...");
                dataService.UpdateLocal((T)data);
              }

              Logger.Info("PullFromServer", $"Work complete.");
              return dataResponse;
            }
            else 
            {
              Logger.Error("PullFromServer", $"Server data model '{message.DatasetName}' not found with key {message.Key}. Unable to update locally.");
              return dataResponse;
            }
          }

          //CurrentRunState.BytesDownloaded += Utilities.GetObjectSize(qas);
          //dataSet.Count = qas.Count;
        }
        else
          throw new InvalidOperationException("Please initialise 'PullFromServer' before calling 'Execute'.");
      }
      catch (Exception ex)
      {
        Logger.Error("PullFromServer", ex.ToString());
        throw;
      }
    }
  }
}
