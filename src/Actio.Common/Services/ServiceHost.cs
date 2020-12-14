﻿using Actio.Common.Events;
using Actio.Common.RabbitMq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using RawRabbit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Actio.Common.Services
{
    public class ServiceHost : IServiceHost
    {
        private readonly IWebHost _webHost;
        public ServiceHost(IWebHost webHost)
        {
            _webHost = webHost;
        }
        public void Run() => _webHost.Run();

        public static HostBuilder Create<TStartup>(string [] args) where TStartup : class
        {
            Console.Title = typeof(TStartup).Namespace;

            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            var webHostBuilder = WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(config)
                .UseStartup<TStartup>();

            return new HostBuilder(webHostBuilder.Build(*));

        }


        public abstract class BuilderBase
        {
            public abstract ServiceHost Build();
        }

        public class HostBuilder : BuilderBase
        {
            private readonly IWebHost _webHost;
            private IBusClient _busClient;
            public HostBuilder(IWebHost webHost)
            {
                _webHost = webHost;
            }

            public BusBuilder UseRabbitMq()
            {
                _busClient = (IBusClient)_webHost.Services.GetService(typeof(IBusClient));

                return new BusBuilder(_webHost, _busClient);
            }
            public override ServiceHost Build()
            {
                return new ServiceHost(_webHost);
            }

        }

        public class BusBuilder : BuilderBase
        {
            private readonly IWebHost _webHost;
            private IBusClient _busClient;
            public BusBuilder(IWebHost webHost, IBusClient busClient)
            {
                _webHost = webHost;
                _busClient = busClient;
            }

            public BusBuilder SubscribeToCommand<TCommand>() where TCommand : ICommand
            {
                var handler = (ICommandHandler<TCommand>)_webHost.Services
                    .GetService(typeof(ICommandHandler<TCommand>));
                _busClient.WithCommandHandlerAsync(handler);

                return this;
            }

            public BusBuilder SubscribeToEvent<TEvent>() where TEvent : IEvent
            {
                var handler = (IEventHandler<TEvent>)_webHost.Services
                    .GetService(typeof(IEventHandler<TEvent>));
                _busClient.WithEventHandlerAsync(handler);

                return this;
            }

            public override ServiceHost Build()
            {
                return new ServiceHost(_webHost);
            }

        }
    }
}