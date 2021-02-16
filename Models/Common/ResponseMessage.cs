using System;

namespace NRDCL.Models.Common
{
    public class ResponseMessage
    {
        private Boolean status;
        private String text;
        private Object dto;
        private string messageKey;

        public bool Status { get => status; set => status = value; }
        public string Text { get => text; set => text = value; }
        public string MessageKey { get => messageKey; set => messageKey = value; }
        public object Dto { get => dto; set => dto = value; }
    }
}
