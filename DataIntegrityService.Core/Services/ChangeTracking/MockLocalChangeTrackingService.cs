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
      Logger.Info("MockLocalChangeTrackingService", TrackedChanges.Any() ? $"Further changes exist..." : "No further changes exist.");
      return TrackedChanges.Any();
    }

    public async Task CompressPendingChanges()
    {
      Logger.Info("MockLocalChangeTrackingService", $"Compressing pending changes...");

      // do nothing...
      await Task.CompletedTask;
    }

    public async Task FlagAsCompleted(DataChangeTrackingModel item)
    {
      Logger.Info("MockLocalChangeTrackingService", $"Flagging this item as complete, removing...");

      TrackedChanges.Remove(item);

      // do nothing...
      await Task.CompletedTask;
    }

    public async Task FlagAsPoison(DataChangeTrackingModel item)
    {
      Logger.Warn("MockLocalChangeTrackingService", $"Flagging this item as POISON, removing...");

      TrackedChanges.Remove(item);

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
      Logger.Warn("MockLocalChangeTrackingService", $"Fetching next change...");
      
      var change = TrackedChanges.First();
      //TrackedChanges.Remove(change);

      return change;
    }

    public async Task IncrementAttempt(DataChangeTrackingModel item)
    {
      item.Attempts++;

      Logger.Warn("MockLocalChangeTrackingService", $"Incrementing attempt at processing this change, value is {item.Attempts}");

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

      Logger.Info("MockLocalChangeTrackingService", $"{TrackedChanges.Count} pending local changes found...");

      foreach (var change in TrackedChanges)
      {
        Logger.Info("MockLocalChangeTrackingService", $"{change.Action.ToUpper()} {change.DatasetName} with key {change.Key}. Attempts: {change.Attempts}");
      }

      IsInitialised = true;

      await Task.CompletedTask;
    }
  }
}
