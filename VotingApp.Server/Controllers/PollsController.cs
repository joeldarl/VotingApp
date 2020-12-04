using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VotingApp.Server.Services;
using VotingApp.Shared.Data;
using VotingApp.Shared.Models;

namespace VotingApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollsController : ControllerBase
    {
        private readonly IPollRepository _pollRepository;

        public PollsController(IPollRepository pollRepository)
        {
            _pollRepository = pollRepository;
        }

        // GET: api/Polls/GHJ7F2
        [HttpGet("{urlCode}")]
        public async Task<ActionResult<Poll>> GetPoll(string urlCode)
        {
            Poll poll = _pollRepository.GetPollByUrlCode(urlCode);

            if (poll == null)
            {
                return NotFound();
            }

            return poll;
        }

        // POST: api/Polls
        [HttpPost]
        public async Task<ActionResult<Poll>> PostPoll(Poll poll)
        {
            await _pollRepository.AddPoll(poll);
            _pollRepository.Save();

            return poll;
        }

        // PUT: api/Polls/G8JFH2/2
        [HttpPut("{urlCode}/{optionId}")]
        public async Task<IActionResult> AddVote(string urlCode, int optionId)
        {
            Poll poll = _pollRepository.GetPollByUrlCode(urlCode);
            Option option = _pollRepository.GetOption(optionId);

            if (poll == null || option == null)
            {
                return NotFound();
            }

            if (option.PollId != poll.PollId)
            {
                return BadRequest();
            }

            _pollRepository.AddOneVote(option);

            try
            {
                _pollRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_pollRepository.OptionExists(optionId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE
        public async Task<IActionResult> DeletePoll(int id)
        {
            Poll poll = _pollRepository.GetPoll(id);

            if (poll == null)
            {
                return NotFound();
            }

            _pollRepository.DeletePoll(poll);

            return NoContent();
        }
    }
}
