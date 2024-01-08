﻿using Microsoft.Extensions.Configuration;
using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core;
using DataIntegrityService.Console.Models;
using DataIntegrityService.Core.Models.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using DataIntegrityService.Console.Providers;
using DataIntegrityService.Console.Services;
using DataIntegrityService.Core.Services.Interfaces;
using DataIntegrityService.Console.Services.Http;
using DataIntegrityService.Core.Services.Http;

// dummy a new user and pass to entry point...
IUserProfile user = new UserProfile();

// create and track a cancellation token if the user bails out...
CancellationTokenSource cts = new CancellationTokenSource();

var entryPoint = new EntryPoint();

// create our local/app specific services here and inject...
IServiceCollection services = new ServiceCollection();
services.AddTransient<IDataService, EvidenceNoteService>();
services.AddTransient<IDataService, MemoService>();
services.AddTransient<IDataService, ProvisionService>();
services.AddTransient<IDataService, QualityAreaService>();
services.AddTransient<IDataService, VisitService>();
services.AddTransient<IHttpService, MockHttpService>();

// configure, initialise...
entryPoint.ConfigureServices(services);
await entryPoint.Initialise();

// execute...
await entryPoint.Run(user, false, cts.Token);
