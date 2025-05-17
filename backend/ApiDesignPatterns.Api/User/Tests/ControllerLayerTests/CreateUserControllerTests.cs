// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.User.ApplicationLayer.Commands.CreateUser;
using backend.User.Controllers;
using backend.User.DomainModels.ValueObjects;
using backend.User.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace backend.User.Tests.ControllerLayerTests;

public class CreateUserControllerTests : CreateUserControllerTestBase
{
    [Fact]
    public async Task CreateUser_ReturnsOkResponse_WhenUserCreatedSuccessfully()
    {
        var user = new UserTestDataBuilder().Build();
        var request = Mapper.Map<CreateUserRequest>(user);
        CreateUserController sut = GetCreateUserController();

        var result = await sut.CreateUser(request);

        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<CreateUserResponse>();
    }

    [Fact]
    public async Task CreateUser_HandlesCommandFailure_WhenCreateUserFails()
    {
        var user = new UserTestDataBuilder().Build();
        var request = Mapper.Map<CreateUserRequest>(user);
        Mock
            .Get(CreateUser)
            .Setup(x => x.Handle(It.IsAny<CreateUserCommand>()))
            .ThrowsAsync(new Exception("Failed to create user"));
        var sut = GetCreateUserController();

        Func<Task> act = async () => await sut.CreateUser(request);

        await act.Should().ThrowAsync<Exception>();
    }

    [Theory]
    [InlineData("John", "Doe", "john.doe@example.com")]
    [InlineData("Jane", "Smith", "jane.smith@example.com")]
    public async Task CreateUser_ValidatesMapping_WithDifferentUserData(
        string firstName, string lastName, string email)
    {
        var user = new UserTestDataBuilder()
            .WithFirstName(new FirstName(firstName))
            .WithLastName(new LastName(lastName))
            .WithEmail(new Email(email))
            .Build();
        var request = Mapper.Map<CreateUserRequest>(user);
        CreateUserController sut = GetCreateUserController();

        var result = await sut.CreateUser(request);

        var response = result.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<CreateUserResponse>().Subject;
        response.FirstName.Should().Be(firstName);
        response.LastName.Should().Be(lastName);
        response.Email.Should().Be(email);
    }

    [Fact]
    public async Task CreateUser_ValidatesMapping_WithDifferentReferenceUserData()
    {
        var user = new UserTestDataBuilder()
            .WithAddressIds([1, 2, 3])
            .WithPhoneNumberIds([4, 5, 6])
            .Build();
        var request = Mapper.Map<CreateUserRequest>(user);
        CreateUserController sut = GetCreateUserController();

        var result = await sut.CreateUser(request);

        var response = result.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<CreateUserResponse>().Subject;
        response.AddressIds.Should().BeEmpty();
        response.PhoneNumberIds.Should().BeEmpty();
    }
}
