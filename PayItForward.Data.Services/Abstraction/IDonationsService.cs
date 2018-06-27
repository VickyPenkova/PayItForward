namespace PayItForward.Services.Abstraction
{
    using System;
    using PayItForward.Models;

    public interface IDonationsService
    {
        bool Add(DonationDTO donation, Guid storyId);
    }
}
