using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Web.Mvc;
using Coolite.Ext.Web;
using Coolite.Utilities;

namespace Beavers.Encounter.Web.Controllers
{
    public class AjaxFormResult : ActionResult
    {
        public AjaxFormResult() { }

        [ClientConfig]
        public string Script { get; set; }

        private bool success = true;

        [ClientConfig]
        [DefaultValue("")]
        public bool Success
        {
            get { return this.success; }
            set { this.success = value; }
        }

        private List<FieldError> errors;

        [ClientConfig(JsonMode.AlwaysArray)]
        public List<FieldError> Errors
        {
            get
            {
                if (this.errors == null)
                {
                    this.errors = new List<FieldError>();
                }

                return this.errors;
            }
        }

        private ParameterCollection extraParams;
        public ParameterCollection ExtraParams
        {
            get
            {
                if (this.extraParams == null)
                {
                    this.extraParams = new ParameterCollection();
                }

                return this.extraParams;
            }
        }

        [ClientConfig("extraParams", JsonMode.Raw)]
        [DefaultValue("")]
        protected string ExtraParamsProxy
        {
            get
            {
                if (this.ExtraParams.Count > 0)
                {
                    return ExtraParams.ToJson();
                }

                return "";
            }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            CompressionUtils.GZipAndSend(new ClientConfig().Serialize(this));
        }
    }

    public class FieldError
    {
        public FieldError(string fieldID, string errorMessage)
        {
            if (string.IsNullOrEmpty(fieldID))
            {
                throw new ArgumentNullException("fieldID", "Field ID can not be empty");
            }

            if (string.IsNullOrEmpty(errorMessage))
            {
                throw new ArgumentNullException("errorMessage", "Error message can not be empty");
            }

            this.FieldID = fieldID;
            this.ErrorMessage = errorMessage;
        }

        [ClientConfig("id")]
        [DefaultValue("")]
        public string FieldID { get; set; }

        [ClientConfig("msg")]
        [DefaultValue("")]
        public string ErrorMessage { get; set; }
    }

    public class FieldErrors : Collection<FieldError> { }
}
