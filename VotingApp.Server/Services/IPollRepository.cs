using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingApp.Shared.Models;

namespace VotingApp.Server.Services
{
    public interface IPollRepository
    {
        IEnumerable<Poll> GetPolls();
        Poll GetPoll(int pollId);
        Poll GetPollByUrlCode(string urlCode);
        Option GetOption(int optionId);
        Task<Poll> AddPoll(Poll poll);
        void AddOneVote(Option option);
        void DeletePoll(Poll poll);
        bool OptionExists(int optionId);
        bool PollExists(int pollId);
        bool Save();
    }
}
