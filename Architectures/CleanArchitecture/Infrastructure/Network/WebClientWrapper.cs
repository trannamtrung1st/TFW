﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Infrastructure.Network
{
    public class WebClientWrapper : IWebClientWrapper
    {
        public void Post(string address, string json)
        {
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";

                // Note: This next line is commented out to prevent an
                // Note: actual HTTP call, since this is just a demo app.

                // client.UploadString(address, "POST", json);
            }
        }
    }
}
