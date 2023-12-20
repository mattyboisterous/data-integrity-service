using Microsoft.Extensions.Configuration;
using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core;
using DataIntegrityService.Core.Models;
using DataIntegrityService.Console.Models;

// dummy a new user and pass to entry point...
IUserProfile user = new UserProfile();

CancellationTokenSource cts = new CancellationTokenSource();

EntryPoint.Run(user, cts.Token);
