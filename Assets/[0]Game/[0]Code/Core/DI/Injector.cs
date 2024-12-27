using System;
using System.Reflection;
using UnityEngine;

namespace Game
{
    public static class Injector
    {
        public static void Inject(object component)
        {
            var componentType = component.GetType();

            // Инъекция в поля
            var fields = componentType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var field in fields)
            {
                if (field.IsDefined(typeof(InjectAttribute), false))
                {
                    var fieldType = field.FieldType;
                    var value = ServiceLocator.Get(fieldType);
                    field.SetValue(component, value);
                }
            }

            // Инъекция в методы
            var methods =
                componentType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var method in methods)
            {
                if (method.IsDefined(typeof(InjectAttribute), true))
                {
                    var parameters = method.GetParameters();
                    var arguments = new object[parameters.Length];

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var parameterType = parameters[i].ParameterType;
                        var value = ServiceLocator.Get(parameterType);
                        if (value == null)
                        {
                            throw new InvalidOperationException(
                                $"Не удалось разрешить зависимость для параметра {parameterType.Name} метода {method.Name}");
                        }

                        arguments[i] = value;
                    }

                    method.Invoke(component, arguments);
                }
            }
        }
    }
}