using BindingFlags = System.Reflection.BindingFlags;

namespace CleanTeeth.Tests.Infrastructure;

public class FakeServiceProvider : IServiceProvider
{
    private List<(Type, Type)> _services = new();

    public void AddService<I, T>()
    {
        _services.Add((typeof(I), typeof(T)));
    }

    public object? GetService(Type serviceType)
    {
        var service = _services.FirstOrDefault(s => s.Item1.FullName == serviceType.FullName).Item2;
        if (service is not null)
        {
            var constructors = service.GetConstructors();
            if (constructors.Length > 0)
            {
               var parameters = constructors[0].GetParameters();
               int paramLength = parameters.Length;
               if (paramLength> 0)
               {
                  object[] paramTypes = new object [paramLength] ;
                   for(int i=0; i< paramLength; i++)
                   {
                       var instance = GetService(parameters[i].ParameterType);
                       if (instance is not null)
                       {
                           paramTypes[i]= instance;
                       }
                   }

                   return Activator.CreateInstance(service, paramTypes);
               }
               else
               {
                   return Activator.CreateInstance(service);
               }
               
            }

            return null;

        }

        return null;
    }
}