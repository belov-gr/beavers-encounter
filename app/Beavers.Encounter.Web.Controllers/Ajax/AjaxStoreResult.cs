using System;
using System.Web.Mvc;
using Coolite.Ext.Web;

namespace Beavers.Encounter.Web.Controllers
{
    public class AjaxStoreResult : ActionResult
    {
        public AjaxStoreResult() { }

        public AjaxStoreResult(object data)
        {
            this.Data = data;
        }

        public AjaxStoreResult(object data, int totalCount)
            : this(data)
        {
            this.TotalCount = totalCount;
        }

        public AjaxStoreResult(StoreResponseFormat responseFormat)
        {
            this.ResponseFormat = responseFormat;
        }

        private object data;
        public object Data
        {
            get { return this.data; }
            set { this.data = value; }
        }

        private int totalCount;
        public int TotalCount
        {
            get { return this.totalCount; }
            set { this.totalCount = value; }
        }

        private StoreResponseFormat responseFormat = StoreResponseFormat.Load;
        public StoreResponseFormat ResponseFormat
        {
            get { return this.responseFormat; }
            set { this.responseFormat = value; }
        }

        private SaveStoreResponse saveResponse;
        public SaveStoreResponse SaveResponse
        {
            get
            {
                if (this.saveResponse == null)
                {
                    this.saveResponse = new SaveStoreResponse();
                }
                return this.saveResponse;
            }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            switch (this.ResponseFormat)
            {
                case StoreResponseFormat.Load:
                    StoreResponseData storeResponse = new StoreResponseData();
                    storeResponse.Data = JSON.Serialize(this.Data);
                    storeResponse.TotalCount = this.TotalCount;
                    storeResponse.Return();
                    break;
                case StoreResponseFormat.Save:
                    Response response = new Response(true);
                    response.Success = this.SaveResponse.Success;
                    response.Msg = this.SaveResponse.ErrorMessage;
                    StoreResponseData saveResponse = new StoreResponseData();
                    saveResponse.Confirmation = this.SaveResponse.ConfirmationList;
                    response.Data = saveResponse.ToString();

                    response.Return();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum StoreResponseFormat
    {
        Load,
        Save
    }

    public class SaveStoreResponse
    {
        private bool success = true;
        private string errorMessage;

        public bool Success
        {
            get { return this.success; }
            set { this.success = value; }
        }

        public string ErrorMessage
        {
            get { return this.errorMessage; }
            set { this.errorMessage = value; }
        }

        public ConfirmationList ConfirmationList { get; set; }
    }
}