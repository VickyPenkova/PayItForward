namespace PayItForward.Services.Abstraction
{
    using System;
    using System.Collections.Generic;
    using PayItForward.Models;

    public interface IDonationsService
    {
        int IsDonationSuccessfull(DonationDTO donation, Guid storyId);
    }
}
