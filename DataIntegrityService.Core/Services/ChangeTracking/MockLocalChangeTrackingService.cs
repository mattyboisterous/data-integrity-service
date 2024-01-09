using DataIntegrityService.Core.Logging;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Core.Services.ChangeTracking.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Services.ChangeTracking
{
  public class MockLocalChangeTrackingService : IChangeTrackingService
  {
    public string Key => "MockLocalChangeTrackingService";
    public bool IsInitialised { get; set; }

    public List<DataChangeTrackingModel> TrackedChanges { get; set; } = new List<DataChangeTrackingModel>();

    public bool ChangesExist()
    {
      return TrackedChanges.Any();
    }

    public async Task CompressPendingChanges()
    {
      // do nothing...
      await Task.CompletedTask;
    }

    public async Task FlagAsCompleted(DataChangeTrackingModel item)
    {
      // do nothing...
      TrackedChanges.Remove(item);
      await Task.CompletedTask;
    }

    public async Task FlagAsPoison(DataChangeTrackingModel item)
    {
      // do nothing...
      await Task.CompletedTask;
    }

    public async Task FlushAllPendingChanges()
    {
      TrackedChanges.Clear();
      await Task.CompletedTask;
    }

    public DataChangeTrackingModel GetNextChange()
    {
      return TrackedChanges.First();
    }

    public async Task IncrementAttempt(DataChangeTrackingModel item)
    {
      // do nothing...
      await Task.CompletedTask;
    }

    public async Task Initialise()
    {
      // todo: 40% -> Nada...
      // todo: 20% -> 1 visit create...
      // todo: 20% -> Memo update...
      // todo: 20% -> 2 visit and memo update...

      Logger.Info("MockLocalChangeTrackingService", $"Initialising, looking for pending changes...");

      Random rnd = new Random();
      int num = rnd.Next(0, 100);

      if (num > 40 && num <= 60)
      {
        TrackedChanges.Add(new DataChangeTrackingModel()
        {
          Id = Guid.NewGuid().ToString(),
          ItemKey = Guid.NewGuid().ToString(),
          Action = "Create",
          UserId = Guid.NewGuid().ToString(),
          Attempts = 0,
          Created = DateTime.UtcNow,
          DatasetName = "Visits"
        });
      }
      else if (num > 60 && num <= 80)
      {
        TrackedChanges.Add(new DataChangeTrackingModel()
        {
          Id = Guid.NewGuid().ToString(),
          ItemKey = Guid.NewGuid().ToString(),
          Action = "Update",
          UserId = Guid.NewGuid().ToString(),
          Attempts = 0,
          Created = DateTime.UtcNow,
          DatasetName = "Memos"
        });
      }
      else
      {
        TrackedChanges.Add(new DataChangeTrackingModel()
        {
          Id = Guid.NewGuid().ToString(),
          ItemKey = Guid.NewGuid().ToString(),
          Action = "Create",
          UserId = Guid.NewGuid().ToString(),
          Attempts = 0,
          Created = DateTime.UtcNow,
          DatasetName = "Visits"
        });
        TrackedChanges.Add(new DataChangeTrackingModel()
        {
          Id = Guid.NewGuid().ToString(),
          ItemKey = Guid.NewGuid().ToString(),
          Action = "Update",
          UserId = Guid.NewGuid().ToString(),
          Attempts = 0,
          Created = DateTime.UtcNow,
          DatasetName = "Visits"
        });
        TrackedChanges.Add(new DataChangeTrackingModel()
        {
          Id = Guid.NewGuid().ToString(),
          ItemKey = Guid.NewGuid().ToString(),
          Action = "Update",
          UserId = Guid.NewGuid().ToString(),
          Attempts = 0,
          Created = DateTime.UtcNow,
          DatasetName = "Memos"
        });
      }

      Logger.Info("MockLocalChangeTrackingService", $"{TrackedChanges.Count} pending changes found...");

      foreach (var change in TrackedChanges)
      {
        Logger.Info("MockLocalChangeTrackingService", $"{change.Action.ToUpper()} {change.DatasetName} with key {change.Key}. Attempts: {change.Attempts}");
      }

      IsInitialised = true;

      await Task.CompletedTask;
    }
  }
}
