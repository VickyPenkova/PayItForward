namespace PayItForward.Services.Abstraction
{
    using System;
    using System.Collections.Generic;
    using PayItForward.Models;

    public interface IDonationsService
    {
        bool Add(DonationDTO donation, Guid storyId);

        IEnumerable<DonationDTO> GetDonationsByUserId(string userId);
    }
}
