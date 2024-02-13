using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core.Logging;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Models.Interfaces;
using DataIntegrityService.Core.Services.ChangeTracking.Interfaces;
using DataIntegrityService.Core.Services.Http;
using DataIntegrityService.Core.Services.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataIntegrityService.Console.Services.ChangeTracking.Static
{
  public class MockStaticChangeTrackingService : IStaticChangeTrackingService
  {
    public string Key => "MockStaticChangeTracking";
    public bool IsInitialised { get; set; }

    public List<StaticDataChangeTrackingModel> LocalReferenceDataSetState { get; set; } = new List<StaticDataChangeTrackingModel>();
    public List<StaticDataChangeTrackingModel> ServerReferenceDataSetState { get; set; } = new List<StaticDataChangeTrackingModel>();
    public required StaticChangeTrackingConfiguration Settings { get; set; }
    public bool ForceRehydrateAll { get; set; }
    public CancellationToken CancellationToken { get; set; }

    public async Task Initialise()
    {
      Logger.Info("MockStaticChangeTrackingService", $"Initialising, ensuring local and server version states are the same...");

      // fetch tracked local changes...
      LocalReferenceDataSetState = GetAllLocal(Key).ToList();

      Logger.Info("MockStaticChangeTrackingService", $"{LocalReferenceDataSetState.Count} local reference data set(s) found.");

      foreach (var dataset in LocalReferenceDataSetState)
      {
        Logger.Info("MockStaticChangeTrackingService", $"'{dataset.DatasetName}' (version '{dataset.Version}').");
      }

      // fetch tracked changes from server...
      var serverResponse = await GetAllFromServer(CancellationToken);

      if (serverResponse != null && serverResponse.ActionSucceeded)
      {
        ServerReferenceDataSetState = (List<StaticDataChangeTrackingModel>)serverResponse.Data;

        Logger.Info("MockStaticChangeTrackingService", $"{ServerReferenceDataSetState.Count} server reference data set(s) found.");

        foreach (var dataset in ServerReferenceDataSetState)
        {
          Logger.Info("MockStaticChangeTrackingService", $"'{dataset.DatasetName}' (version '{dataset.Version}').");
        }
      }
      else if (Settings.AbortOnFailure)
        throw new InvalidOperationException($"Unable to syncronise static data and AbortOnFailure is true. Stopping all work.");

      IsInitialised = true;
      await Task.CompletedTask;
    }

    public async Task<IDataResponse<IEnumerable<StaticDataChangeTrackingModel>>> GetAllFromServer(CancellationToken cancellationToken)
    {
      // todo: 90% -> nothing changed...
      // todo: 10% -> Quality Area has been updated...

      Random rnd = new Random();
      int num = rnd.Next(0, 100);

      if (num < 90)
      {
        ServerReferenceDataSetState.Add(new StaticDataChangeTrackingModel()
        {
          Id = "fdd2bdfa-ace0-4f99-9e4c-b470aeca95d1",
          DatasetName = "QualityAreas",
          Version = "26ff4e21-41e1-4ffa-b18b-8c3e8868f812"
        });
        ServerReferenceDataSetState.Add(new StaticDataChangeTrackingModel()
        {
          Id = "effcf237-a6b0-4201-9387-eabd470a138a",
          DatasetName = "Provisions",
          Version = "12bf1250-c73a-461b-9efc-280527bd2c4b"
        });
      }
      else
      {
        ServerReferenceDataSetState.Add(new StaticDataChangeTrackingModel()
        {
          Id = "fdd2bdfa-ace0-4f99-9e4c-b470aeca95d1",
          DatasetName = "QualityAreas",
          Version = "de816bbf-0046-4430-b73e-1544781ac304"
        });
        ServerReferenceDataSetState.Add(new StaticDataChangeTrackingModel()
        {
          Id = "effcf237-a6b0-4201-9387-eabd470a138a",
          DatasetName = "Provisions",
          Version = "12bf1250-c73a-461b-9efc-280527bd2c4b"
        });
      }

      var result = new DataResponse<IEnumerable<StaticDataChangeTrackingModel>>(ServerReferenceDataSetState);
      result.HttpResponseCode = 200;

      Logger.Info("MockStaticChangeTrackingService", $"GET => Returning models from server. HttpResponseCode => {result.HttpResponseCode}");

      return await Task.FromResult(result);
    }

    public IEnumerable<StaticDataChangeTrackingModel> GetAllLocal(string key)
    {
      // todo: 80% -> we have already pulled down reference data...
      // todo: 20% -> we have none...

      Random rnd = new Random();
      int num = rnd.Next(0, 100);

      if (num < 80)
      {
        LocalReferenceDataSetState.Add(new StaticDataChangeTrackingModel()
        {
          Id = "fdd2bdfa-ace0-4f99-9e4c-b470aeca95d1",
          DatasetName = "QualityAreas",
          Version = "26ff4e21-41e1-4ffa-b18b-8c3e8868f812"
        });
        LocalReferenceDataSetState.Add(new StaticDataChangeTrackingModel()
        {
          Id = "effcf237-a6b0-4201-9387-eabd470a138a",
          DatasetName = "Provisions",
          Version = "12bf1250-c73a-461b-9efc-280527bd2c4b"
        });
      }

      return LocalReferenceDataSetState;
    }

    public int UpsertLocal(string key, IEnumerable<StaticDataChangeTrackingModel> items)
    {
      throw new NotImplementedException();
    }
  }
}
