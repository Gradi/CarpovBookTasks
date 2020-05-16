using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using milc.Attributes;

namespace milc
{
    public abstract class Visitor<T>
    {
        private readonly IReadOnlyDictionary<Type, Action<T>> _visitors;
        private readonly bool _isThrowOnMissingVisitor;

        protected Visitor(bool throwOnMissingVisitor = true)
        {
            var thisType = GetType();
            var genericType = typeof(T);

            var visitorMethods = thisType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                .Where(m => !m.IsDefined(typeof(IgnoreVisitorAttribute)) && m.Name.StartsWith(nameof(Visit)))
                .Where(m =>
                {
                    var parameters = m.GetParameters();
                    return parameters.Length == 1 && genericType.IsAssignableFrom(parameters[0].ParameterType);
                })
                .Select(m => (Method: m, ArgType: m.GetParameters()[0].ParameterType));

            var visitors = new Dictionary<Type, Action<T>>();
            foreach (var visitor in visitorMethods)
            {
                if (visitors.ContainsKey(visitor.ArgType))
                    throw new Exception($"Class \"{thisType}\" contains several visitor methods that accept same type of \"{genericType}\" ({visitor.ArgType}).");

                var input = Expression.Parameter(genericType);
                Expression body = Expression.Convert(input, visitor.ArgType);
                body = Expression.Call(Expression.Constant(this), visitor.Method, body);

                var lambda = Expression.Lambda<Action<T>>(body, input);
                visitors[visitor.ArgType] = lambda.Compile();
            }

            _visitors = visitors;
            _isThrowOnMissingVisitor = throwOnMissingVisitor;
        }

        [IgnoreVisitor]
        public virtual void Visit(T target)
        {
            var actualType = target!.GetType();
            if (_visitors.TryGetValue(actualType, out var visitor))
            {
                visitor(target);
            }
            else if (_isThrowOnMissingVisitor)
            {
                throw new ArgumentException($"Can't find visitor method for argument of type \"{actualType}\". " +
                                            $"Make sure this class has method that starts with \"{nameof(Visit)}\" word and accepts " +
                                            $"\"{actualType}\" as it's only argument.");
            }
        }
    }
}
