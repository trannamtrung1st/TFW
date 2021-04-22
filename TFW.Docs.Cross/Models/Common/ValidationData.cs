using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TFW.Docs.Cross.Exceptions;

namespace TFW.Docs.Cross.Models.Common
{
    public class ValidationData
    {
        private bool _autoMessage;
        private string _autoMessageSeperator;

        [JsonProperty("isValid")]
        public bool IsValid { get; set; }

        [JsonProperty("details")]
        public List<AppResult> Details { get; }

        private string _message;
        [JsonIgnore]
        public string Message
        {
            get
            {
                if (_autoMessage)
                {
                    var allMessages = Details.Select(o => o.Message).ToArray();
                    return string.Join(_autoMessageSeperator, allMessages);
                }

                return _message;
            }
            set
            {
                if (_autoMessage)
                    throw new InvalidOperationException($"Invalid request because of '{nameof(_autoMessage)}' = true");

                _message = value;
            }
        }

        public ValidationData(bool autoMessage = true, string autoMessageSeperator = "\n")
        {
            _autoMessage = autoMessage;
            _autoMessageSeperator = autoMessageSeperator;
            Details = new List<AppResult>();
            IsValid = true;
        }

        public ValidationData Fail(IStringLocalizer localizer, string mess = null, ResultCode? code = null, object data = null)
        {
            Details.Add(AppResult.OfCode(localizer, code, data, mess));

            IsValid = false;

            return this;
        }

        public ValidationData Fail(params AppResult[] results)
        {
            Details.AddRange(results);

            IsValid = false;

            return this;
        }

        public AppValidationException BuildException(IStringLocalizer localizer)
        {
            if (IsValid)
                throw new InvalidOperationException("This validation is valid");

            return AppValidationException.From(localizer, this);
        }
    }
}
