using Autofac;
using Britannica.Application.MediatR;
using Britannica.Application.MediatR.Pipeline;
using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Britannica.Application
{
    public static class DI
    {
        public static Assembly Assembly = Assembly.GetExecutingAssembly();

        public static ContainerBuilder RegisterApplication(this ContainerBuilder builder, List<Assembly> assemblys)
        {
            builder.RegisterAssemblyTypes(Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .InstancePerDependency();

            //services.AddMediatR(Assembly, Application.DI.Assembly, Domain.DI.Assembly);
            builder.RegisterMediatR(assemblys.ToArray());
            builder.RegisterGeneric(typeof(LoggingBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(ValidationBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(PerformanceBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
            return builder;
        }
    }
}

