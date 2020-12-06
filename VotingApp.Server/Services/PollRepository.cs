using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingApp.Shared.Data;
using VotingApp.Shared.Models;

namespace VotingApp.Server.Services
{
    public class PollRepository : IPollRepository
    {
        private readonly PollContext _context;

        public PollRepository(PollContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Poll> GetPolls()
        {
            return _context.Polls.ToList<Poll>();
        }

        public Poll GetPoll(int id)
        {
            return _context.Polls.FirstOrDefault(p => p.PollId == id);
        }

        public Poll GetPollByUrlCode(string urlCode)
        {
            if(urlCode == string.Empty)
            {
                throw new ArgumentNullException(nameof(urlCode));
            }

            return _context.Polls
                .Where(b => b.UrlCode == urlCode)
                .Include(u => u.Options)
                .FirstAsync()
                .Result;
        }

        public Option GetOption(int id)
        {
            return _context.Options.FirstOrDefault(o => o.OptionId == id);
        }

        public async Task<Poll> AddPoll(Poll poll)
        {
            if (poll == null)
            {
                throw new ArgumentNullException(nameof(poll));
            }

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var stringChars = new char[6];
            var random = new Random();
            var finalString = "";

            // Generating random Url code and checking it doens't already exist
            do
            {
                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                finalString = new String(stringChars);

            } while (await _context.Polls.Where(b => b.UrlCode == finalString).FirstOrDefaultAsync() != null);

            poll.UrlCode = finalString;

            // Removing blank options
            poll.Options = poll.Options.Where(s => !string.IsNullOrWhiteSpace(s.Body)).Distinct().ToList();

            _context.Polls.Add(poll);

            return poll;
        }

        public void AddOneVote(Option option)
        {
            option.Votes++;
            _context.Entry(option).State = EntityState.Modified;
        }

        public void DeletePoll(Poll poll)
        {
            _context.Polls.Remove(poll);
        }

        public bool PollExists(int id)
        {
            return _context.Polls.Any(e => e.PollId == id);
        }

        public bool OptionExists(int id)
        {
            return _context.Options.Any(e => e.OptionId == id);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
