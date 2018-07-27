using Microsoft.ServiceFabric.Services.Remoting.V2;
using System;
using System.Collections.Concurrent;

namespace Ruhnow.ServiceFabric.Core.Pipeline
{
    public class MessagePipeline
    {
        private static ConcurrentDictionary<string, PipelineRequestEventRegistration> requestHandlers = new ConcurrentDictionary<string, PipelineRequestEventRegistration>();
        private static ConcurrentDictionary<string, PipelineResponseEventRegistration> responseHandlers = new ConcurrentDictionary<string, PipelineResponseEventRegistration>();

        internal static void Execute(IServiceRemotingRequestMessage request)
        {
            var pipelineHandlers = requestHandlers.Values;
            foreach (var pipelineHandler in pipelineHandlers)
            {
                pipelineHandler.Handler.Process(request);
            }
        }
        internal static void Execute(IServiceRemotingResponseMessage response)
        {
            var pipelineHandlers = responseHandlers.Values;
            foreach (var pipelineHandler in pipelineHandlers)
            {
                pipelineHandler.Handler.Process(response);
            }
        }

        public static void RemoveRequestHandler(string name)
        {
            requestHandlers.TryRemove(name, out PipelineRequestEventRegistration reg);
        }

        public static void RemoveResponseHandler(string name)
        {
            responseHandlers.TryRemove(name, out PipelineResponseEventRegistration reg);
        }

        public static void AddRequestHandler<TRequestHandler>(string name) where TRequestHandler : IPipelineRequestEventHandler
        {
            if (typeof(TRequestHandler).GetConstructor(new Type[0]) == null)
            {
                throw new InvalidOperationException("Request handler must have a parameterless constructor.");
            }

            TRequestHandler handler = Activator.CreateInstance<TRequestHandler>();
            PipelineRequestEventRegistration registration = new PipelineRequestEventRegistration()
            {
                Handler = handler
            };

            requestHandlers.AddOrUpdate(name, registration, (nme, reg) => registration);
        }

        public static void AddResponseHandler<TResponseHandler>(string name) where TResponseHandler : IPipelineResponseEventHandler
        {
            if (typeof(TResponseHandler).GetConstructor(new Type[0]) == null)
            {
                throw new InvalidOperationException("Response handler must have a parameterless constructor.");
            }

            TResponseHandler handler = Activator.CreateInstance<TResponseHandler>();
            PipelineResponseEventRegistration registration = new PipelineResponseEventRegistration()
            {
                Handler = handler
            };

            responseHandlers.AddOrUpdate(name, registration, (nme, reg) => registration);
        }
    }
}
