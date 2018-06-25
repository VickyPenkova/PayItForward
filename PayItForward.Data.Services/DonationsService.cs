namespace PayItForward.Services
{
    using System;
    using System.Linq;
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using PayItForward.Data;
    using PayItForward.Data.Models;
    using PayItForward.Models;
    using PayItForward.Services.Abstraction;
    using PayItForwardDbmodels = PayItForward.Data.Models;

    public class DonationsService : IDonationsService
    {
        private readonly IRepository<PayItForwardDbmodels.Donation, Guid> donationsRepo;
        private readonly IRepository<PayItForwardDbmodels.Story, Guid> storiesRepo;
        private readonly IRepository<PayItForwardDbmodels.User, string> usersRepo;
        private readonly IMapper mapper;

        public DonationsService(
            IRepository<PayItForwardDbmodels.Donation, Guid> donationsRepo,
            IRepository<PayItForwardDbmodels.Story, Guid> storiesRepo,
            IRepository<PayItForwardDbmodels.User, string> usersRepo,
            IMapper mapper)
        {
            this.donationsRepo = donationsRepo;
            this.storiesRepo = storiesRepo;
            this.usersRepo = usersRepo;
            this.mapper = mapper;
        }

        // TODO: TEST
        public Guid Add(DonationDTO donation, Guid storyId)
        {
            var donationEntity = new Donation
            {
                Amount = donation.Amount,
                CreatedOn = DateTime.UtcNow,
                Id = donation.Id,
                StoryId = storyId,
                UserId = donation.Donator.Id
            };

            this.donationsRepo.Add(donationEntity);
            this.donationsRepo.Save();

            var storyToDonate = this.storiesRepo.GetById(storyId);
            storyToDonate.CollectedAmount += donation.Amount;
            this.storiesRepo.Save();

            var userToDonate = this.usersRepo.GetById(donation.Donator.Id);
            userToDonate.AvilableMoneyAmount -= donation.Amount;
            this.usersRepo.Save();

            return donationEntity.Id;
        }
    }
}
