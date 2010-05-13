using System.Web.Mvc;
using Coolite.Ext.Web;

namespace Beavers.Encounter.Web.Controllers
{
    public class AjaxResult : ActionResult
    {
        public AjaxResult() { }

        public AjaxResult(string script)
        {
            Script = script;
        }

        public AjaxResult(object result)
        {
            Result = result;
        }

        public string Script { get; set; }
        public object Result { get; set; }
        public string ErrorMessage { get; set; }

        private ParameterCollection extraParamsResponse;
        public ParameterCollection ExtraParamsResponse
        {
            get { return extraParamsResponse ?? (extraParamsResponse = new ParameterCollection()); }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = new AjaxResponse {Result = Result};

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                response.Success = false;
                response.ErrorMessage = ErrorMessage;
            }
            else
            {
                if (!string.IsNullOrEmpty(Script))
                {
                    response.Script = string.Concat("<string>", Script);
                }

                if (ExtraParamsResponse.Count > 0)
                {
                    response.ExtraParamsResponse = ExtraParamsResponse.ToJson();
                }
            }

            response.Return();
        }
    }
}
