using System;
using System.Reflection;
using Autofac;
using MediatR;
using MediatR.Pipeline;
using Module = Autofac.Module;

namespace Britannica.Application.MediatR
{
    internal class MediatRModule : Module
    {
        private readonly Assembly[] _assemblies;
        //private readonly Type[] _customBehaviorTypes;

        public MediatRModule(Assembly[] assemblies/*, Type[] customBehaviorTypes*/)
        {
            _assemblies = assemblies;
            //_customBehaviorTypes = customBehaviorTypes;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            var openHandlerTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(IRequestExceptionHandler<,,>),
                typeof(IRequestExceptionAction<,>),
                typeof(INotificationHandler<>),
            };

            foreach (var openHandlerType in openHandlerTypes)
            {
                builder.RegisterAssemblyTypes(_assemblies).AsClosedTypesOf(openHandlerType);
            }

            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            builder.RegisterGeneric(typeof(RequestExceptionActionProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestExceptionProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));


            //foreach (var customBehaviorType in _customBehaviorTypes)
            //{
            //    builder.RegisterGeneric(customBehaviorType).As(typeof(IPipelineBehavior<,>));
            //}

            builder.Register<ServiceFactory>(outerContext =>
            {
                var innerContext = outerContext.Resolve<IComponentContext>();
                return serviceType => innerContext.Resolve(serviceType);
            });


        }
    }
}