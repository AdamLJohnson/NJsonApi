﻿using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NJsonApi.Common.Utils
{
    public static class ExpressionUtils
    {
        public static PropertyInfo GetPropertyInfo(this LambdaExpression propertyExpression)
        {
            var expression = propertyExpression.Body;
            if (expression is UnaryExpression)
                expression = ((UnaryExpression)expression).Operand;

            var me = expression as MemberExpression;

            if (me == null || !(me.Member is PropertyInfo))
                throw new NotSupportedException("Only simple property accessors are supported");

            return (PropertyInfo)me.Member;
        }

        public static Func<object, object> CompileToObjectTypedFunction<T>(Expression<Func<T, object>> expression)
        {
            ParameterExpression p = Expression.Parameter(typeof(object));
            Expression<Func<object, object>> convertedExpression = Expression.Lambda<Func<object, object>>
            (
                Expression.Invoke(expression, Expression.Convert(p, typeof(T))),
                p
            );

            return convertedExpression.Compile();
        }

        public static Expression<Action<object, object>> ConvertToObjectTypeExpression<T>(Expression<Action<T, object>> expression)
        {
            ParameterExpression p = Expression.Parameter(typeof(object));
            Expression<Action<object, object>> convertedExpression = Expression.Lambda<Action<object, object>>
            (
                Expression.Invoke(expression, Expression.Convert(p, typeof(T))),
                p
            );

            return convertedExpression;
        }

        public static Expression<Func<TResource, object>> CompileToObjectTypedExpression<TResource, TNested>(Expression<Func<TResource, TNested>> expression)
        {
            ParameterExpression p = Expression.Parameter(typeof(object));
            Expression<Func<TResource, object>> convertedExpression = Expression.Lambda<Func<TResource, object>>
            (
                Expression.Invoke(expression, Expression.Convert(p, typeof(TResource))),
                p
            );

            return convertedExpression;
        }

        public static Func<TInstance, TResult> ToCompiledGetterFunc<TInstance, TResult>(this PropertyInfo pi)
        {
            var mi = pi.GetGetMethod();
            var parameter = Expression.Parameter(typeof(TInstance));
            return Expression.Lambda<Func<TInstance, TResult>>(Expression.Call(parameter, mi), parameter).Compile();
        }

        public static Action<TInstance, TValue> ToCompiledSetterAction<TInstance, TValue>(this PropertyInfo pi)
        {
            var mi = pi.GetSetMethod();
            var instanceParameter = Expression.Parameter(typeof(TInstance));
            var valueParameter = Expression.Parameter(typeof(TValue));
            return Expression.Lambda<Action<TInstance, TValue>>(Expression.Call(instanceParameter, mi, valueParameter), instanceParameter, valueParameter).Compile();
        }
    }
}
