using System.Linq.Expressions;
using System.Reflection;

namespace ms_usuario.Helpers
{
    public class MessageHelper
    {
        public static string NotFoundFor<T>(T o, Expression<Func<T, object>> property) => $"{typeof(T).Name}.{GetName(property)}: '{GetValue(o, property)}' não encontrado.";
        public static string NotFoundFor<T>() => $"{typeof(T).Name} não encontrado.";

        public static string InvalidFor<T>(T o, Expression<Func<T, object>> property) => $"{typeof(T).Name}.{GetName(property)}: '{GetValue(o, property)}' inválido.";

        public static string InvalidFor<T>(T o, Expression<Func<T, object>> property, string message) => $"{typeof(T).Name}.{GetName(property)}: '{GetValue(o, property)}' inválido. {message}";

        public static string DuplicateKeyFor<T>(T o, Expression<Func<T, object>> property) => $"{typeof(T).Name}.{GetName(property)}: '{GetValue(o, property)}' já cadastrado.";

        public static string EmptyFor<T>(Expression<Func<T, object>> property) => $"Campo '{typeof(T).Name}.{GetName(property)}' está vazio.";

        public static string NullFor<T>(Expression<Func<T, object>> property) => $"Campo '{typeof(T).Name}.{GetName(property)}' é nulo.";

        public static string NullFor<T>() => $"'{nameof(T)}' é nulo.";

        public static string InactiveFor<T>(T o, Expression<Func<T, object>> property) => $"{typeof(T).Name}.{GetName(property)}: '{GetValue(o, property)}' está inátivo.";

        private static string GetName<T>(Expression<Func<T, object>> property) => GetMemberExpression(property).Member.Name;

        private static string? GetValue<T>(T o, Expression<Func<T, object>> property)
        {
            var expr = GetMemberExpression(property);
            PropertyInfo? prop = expr?.Member as PropertyInfo;
            object? value = prop?.GetValue(o);
            return value == null ? "Null" : value.ToString();
        }

        private static MemberExpression? GetMemberExpression<T>(Expression<Func<T, object>> property)
        {
            MemberExpression? member = property.Body as MemberExpression;
            return member ?? (property.Body is UnaryExpression unary ? unary.Operand as MemberExpression : null);
        }
    }
}
