namespace PayItForward.Services.Abstraction
{
    using System;
    using PayItForward.Models;

    public interface IDonationsService
    {
        Guid Make(DonationDTO donation);
    }
}
