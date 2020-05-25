using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace chapter03
{
    public sealed class TranslateRouteValueTransformer : DynamicRouteValueTransformer
    {
        private const string _languageKey = "language";
        private const string _actionKey = "action";
        private const string _controllerKey = "controller";

        private readonly ITranslator _translator;

        public TranslateRouteValueTransformer(ITranslator translator)
        {
            this._translator = translator;
        }

        public override async ValueTask<RouteValueDictionary> TransformAsync(
            HttpContext httpContext, RouteValueDictionary values)
        {
            var language = values[_languageKey] as string;
            var controller = values[_controllerKey] as string;
            var action = values[_actionKey] as string;

            controller = await this._translator.Translate(language, controller) ?? controller;
            action = await this._translator.Translate(language, action) ?? action;

            values[_controllerKey] = controller;
            values[_actionKey] = action;

            return values;
        }
    }
}
