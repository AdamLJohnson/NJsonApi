using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SocialCee.Framework.NJsonApi
{
    public interface IResourceMapping
    {
        Func<object, object> IdGetter { get; set; }
        Action<object, string> IdSetter { get; set; }
        Type ResourceRepresentationType { get; set; }
        string ResourceType { get; set; }
        string UrlTemplate { get; set; }
        List<ILinkMapping> Links { get; set; }
        Dictionary<string, Func<object, object>> PropertyGetters { get; set; }
        Dictionary<string, Action<object, object>> PropertySetters { get; }
        Dictionary<string, Expression<Action<object, object>>> PropertySettersExpressions { get; }

    }
}