using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using PoshAsp.Models;

namespace PoshAsp.Filters
{
    public class ApiAuth : Attribute, IAuthenticationFilter
    {
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue auth = request.Headers.Authorization;

            if (auth == null)
            {
                return;
            }
            else if (auth.Scheme != "Basic")
            {
                return;
            }
            else if(String.IsNullOrEmpty(auth.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", request);
            }

            try
            {
                byte[] TokenBytes = Convert.FromBase64String(auth.Parameter);

                Encoding encoding = Encoding.ASCII;
                encoding = (Encoding)encoding.Clone();
                encoding.DecoderFallback = DecoderFallback.ExceptionFallback;

                string TokenString = encoding.GetString(TokenBytes);

                GenericPrincipal principal = await AuthenticateAsync(TokenString, cancellationToken);

                if(principal == null)
                {
                    context.ErrorResult = new AuthenticationFailureResult("Invalid username or password", request);
                }
                else
                {
                    context.Principal = principal;
                }
            }
            catch
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid credentials", request);
            }
        }

        protected Task<GenericPrincipal> AuthenticateAsync(string TokenString, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                AuthToken Token = new AuthToken(TokenString);
                GenericPrincipal User = new GenericPrincipal(new GenericIdentity(Token.Username), null);
                return Task<GenericPrincipal>.FromResult<GenericPrincipal>(User);
            }
            catch
            {
                return null;
            }
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public virtual bool AllowMultiple
        {
            get { return false; }
        }
    }
}