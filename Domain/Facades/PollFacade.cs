﻿using System;
using System.Linq;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.DataAccessApi;
using Svitla.MovieService.DomainApi;
using Svitla.MovieService.DomainApi.Exceptions;

namespace Svitla.MovieService.Domain.Facades
{
    public class PollFacade : IPollFacade
    {
        private readonly IPollRepository polls;

        public PollFacade(IPollRepository pollRepository)
        {
            polls = pollRepository;
        }

        public Poll GetCurrent()
        {
            return polls.One(q => q.SingleOrDefault(p => p.ViewDate > DateTime.Now));
        }

        public void Save(Poll poll)
        {
            var currentPoll = GetCurrent();
            if (currentPoll != null && currentPoll != poll)
            {
                throw new SinglePollAllowedException();
            }

            polls[poll.Id] = poll;

            polls.Commit();
        }
    }
}
