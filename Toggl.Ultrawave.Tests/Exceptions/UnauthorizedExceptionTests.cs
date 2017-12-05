﻿using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NSubstitute;
using Toggl.Multivac;
using Toggl.Ultrawave.Exceptions;
using Toggl.Ultrawave.Network;
using Xunit;

namespace Toggl.Ultrawave.Tests.Exceptions
{
    public sealed class UnauthorizedExceptionTests
    {
        [Fact, LogIfTooSlow]
        public void ReturnsTheApiTokenWhichWasUsedForAuthentication()
        {
            var originalToken = Guid.NewGuid().ToString();
            var request = Substitute.For<IRequest>();
            request.Headers.Returns(new[] { Credentials.WithApiToken(originalToken).Header });
            var exception = new UnauthorizedException(request, Substitute.For<IResponse>());

            var token = exception.ApiToken;

            token.Should().Be(originalToken);
        }

        [Fact, LogIfTooSlow]
        public void ReturnsNullWhenThereAreNoAuthHeaders()
        {
            var request = Substitute.For<IRequest>();
            request.Headers.Returns(new[] { new HttpHeader("name", "value", HttpHeader.HeaderType.None) });
            var exceptionWithoutApiToken = new UnauthorizedException(request, Substitute.For<IResponse>());

            var token = exceptionWithoutApiToken.ApiToken;

            token.Should().BeNull();
        }

        [Theory, LogIfTooSlow]
        [MemberData(nameof(AuthHeadersWithoutApiToken))]
        internal void ReturnsNullWhenTheAuthorizationHeaderNodesNotContainTheBase64EncodedApiToken(HttpHeader authHeader)
        {
            var request = Substitute.For<IRequest>();
            request.Headers.Returns(new[] { authHeader });
            var exceptionWithoutApiToken = new UnauthorizedException(request, Substitute.For<IResponse>());

            var token = exceptionWithoutApiToken.ApiToken;

            token.Should().BeNull();
        }

        public static IEnumerable<object[]> AuthHeadersWithoutApiToken
            => new[]
            {
                    new object[] { Credentials.WithPassword(Email.FromString("some@email.com"), "123456").Header },
                    new object[] { Credentials.None.Header },
                    new object[] { new HttpHeader("Authorization", Credentials.WithApiToken("some_token").Header.Value, HttpHeader.HeaderType.None) },
                    new object[] { new HttpHeader("Authorization", null, HttpHeader.HeaderType.Auth) },
                    new object[] { new HttpHeader("Authorization", "", HttpHeader.HeaderType.Auth) },
                    new object[]
                    {
                        new HttpHeader("NotRealAuthorization", Guid.NewGuid().ToString(), HttpHeader.HeaderType.Auth)
                    },
                    new object[]
                    {
                        new HttpHeader("Authorization", Guid.NewGuid().ToString(), HttpHeader.HeaderType.Auth)
                    },
                    new object[]
                    {
                        new HttpHeader("Authorization", Convert.ToBase64String(Encoding.UTF8.GetBytes("string_without_a_colon")), HttpHeader.HeaderType.Auth)
                    }
            };
    }
}