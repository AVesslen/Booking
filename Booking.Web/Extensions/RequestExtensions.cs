﻿namespace Booking.Web.Extensions
{
    public static class RequestExtensions
    {
        public static bool IsAjax(this HttpRequest request) 
        { 
            if (request is null)
            {
                throw new ArgumentException(nameof(request));
            }

            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        
        
        }
    }
}
