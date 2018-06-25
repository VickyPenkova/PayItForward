namespace PayItForward.Services.Abstraction
{
    using System;
    using PayItForward.Models;

    public interface IDonationsService
    {
        Guid Add(DonationDTO donation, Guid storyId);
    }
}
