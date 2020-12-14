using System;
public interface IAuthenticatedCommand : ICommand
{
    Guid UserId {get; set;}
}