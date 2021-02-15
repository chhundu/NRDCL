using System;

namespace NRDCL.Models.Common
{
    public class ResponseMessage
    {
        private Boolean status;
        private String text;
        private Object dto;

        public bool Status { get => status; set => status = value; }
        public string Text { get => text; set => text = value; }
        public object Dto { get => dto; set => dto = value; }
    }
}
