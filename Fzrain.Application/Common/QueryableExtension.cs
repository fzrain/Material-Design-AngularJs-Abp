using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Fzrain.Common
{
    public static  class QueryableExtension
    {
        public static IQueryable<T> FilterBy<T,S>(this IQueryable<T> query, BaseInputDto<S> input,out int totalCount)
        {
            var param = Expression.Parameter(typeof(T), "t");

            var whereExpression = Expression.Equal(Expression.Constant(1), Expression.Constant(1)); //1==1
            var props = input.Filter.GetType().GetProperties();
            foreach (var prop in props)
            {
                if (prop.GetValue(input.Filter)!=null&&!string.IsNullOrWhiteSpace(prop.GetValue(input.Filter).ToString()))
                {
                    var value = prop.GetValue(input.Filter).ToString();
                    if (prop.PropertyType.IsPrimitive || prop.PropertyType == typeof(string) || prop.PropertyType == typeof(DateTime) || prop.PropertyType.IsEnum)  //判断是否为基本类型或String或DateTime
                    {
                            var domainProp= typeof(T).GetProperties().FirstOrDefault(p => p.Name == prop.Name);
                        if (domainProp!=null)
                        {
                            if (domainProp.PropertyType == typeof(string))//string类型模糊检索
                            {
                                var expression = Expression.Call
                                    (
                                        Expression.Property(param, domainProp), //u.UserName
                                        typeof(string).GetMethod("Contains", new[] { typeof(string) }),// 反射使用.Contains()方法                         
                                        Expression.Constant(value) // .Contains(value)
                                    );
                                whereExpression = Expression.AndAlso(whereExpression, expression);
                                break;
                            }
                            if (domainProp.PropertyType == typeof(int))//Int类型精确匹配
                            {
                                var expression = Expression.Equal(Expression.Property(param, domainProp),
                                    Expression.Constant(Convert.ToInt32(value)));
                                whereExpression = Expression.AndAlso(whereExpression, expression);
                                break;
                            }
                            if (domainProp.PropertyType.IsEnum) //Enum类型精确匹配
                            {
                                var expression = Expression.Equal(Expression.Property(param, domainProp),
                                  Expression.Constant(Enum.Parse(domainProp.PropertyType, value)));
                                whereExpression = Expression.AndAlso(whereExpression, expression);
                                break;
                            }
                        }
                          
                        

                    }
                }
            }      
             query = query.Where(Expression.Lambda<Func<T, bool>>(whereExpression, param));
            totalCount = query.Count();
            return query;
        }
    }
}
