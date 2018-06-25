namespace PayItForward.Services
{
    using System;
    using AutoMapper;
    using PayItForward.Data;
    using PayItForward.Models;
    using PayItForward.Services.Abstraction;
    using PayItForwardDbmodels = PayItForward.Data.Models;

    public class DonationsService : IDonationsService
    {
        private readonly IRepository<PayItForwardDbmodels.Donation, Guid> donationsRepo;
        private readonly IMapper mapper;

        public DonationsService(IRepository<PayItForwardDbmodels.Donation, Guid> donationsRepo, IMapper mapper)
        {
            this.donationsRepo = donationsRepo;
            this.mapper = mapper;
        }

        // TODO: TEST
        public Guid Make(DonationDTO donation)
        {
            var donationEntity = this.mapper.Map<PayItForwardDbmodels.Donation>(donation);
            this.donationsRepo.Add(donationEntity);
            return donationEntity.Id;
        }
    }
}
