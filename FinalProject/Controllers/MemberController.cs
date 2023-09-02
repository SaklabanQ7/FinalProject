using FinalProject.Application.Repositories;
using FinalProject.Domain.Entities;
using FinalProject.Domain.Entities.ViewModel;
using FinalProject.Persistence.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System.Collections.Immutable;
using System.Data;
using System.Diagnostics.Metrics;
using System.Net.Mail;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Member")]

    public class MemberController : ControllerBase
    {
        private readonly IMemberReadRepository _memberReadRepository;
        private readonly IMemberWriteRepository _memberWriteRepository;
        private readonly IEventReadRepository _eventReadRepository;
        private readonly IEventWriteRepository _eventWriteRepository;
        private readonly IAttendeeReadRepository _attendeeReadRepository;
        private readonly IAttendeeWriteRepository _attendeeWriteRepository;
        private readonly OnlineEventsDbContext _context;
        
        public MemberController(IMemberReadRepository memberReadRepository, IMemberWriteRepository memberWriteRepository,OnlineEventsDbContext context,IEventReadRepository eventReadRepository,IEventWriteRepository eventWriteRepository, IAttendeeReadRepository attendeeReadRepository, IAttendeeWriteRepository attendeeWriteRepository)
        {
            _eventReadRepository  = eventReadRepository;
            _eventWriteRepository = eventWriteRepository;
            _context = context;
            _memberReadRepository = memberReadRepository;
            _memberWriteRepository = memberWriteRepository;
            _attendeeReadRepository = attendeeReadRepository;
            _attendeeWriteRepository = attendeeWriteRepository;
        }
        [Authorize(Roles ="Admin")]
        [HttpGet]
        public IActionResult Get()
        {
            IQueryable<Member> members = _memberReadRepository.GetAll();
             
            return Ok(members);

        }
        [Authorize(Roles = "Admin")]


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Member members = await _memberReadRepository.GetByIdAsync(id);

            return Ok(members);
        }
        [HttpPost]
        public async Task<IActionResult> AddMember(Member member)
        {
            Member member2 = _context.Members.FirstOrDefault(a=>a.MailAddress == member.MailAddress);

            if (!ModelState.IsValid || member.Title!=null || member2!=null)
            {
                if (member.Title != null)
                    return BadRequest("Title cannot be entered ");
                else if (member2 != null)
                    return BadRequest("This email already taken");
                else
                    return BadRequest();
            }
            else 
            {
                member.Title = "Member";
            await _memberWriteRepository.AddAsync(member);
            _memberWriteRepository.Save();

            return Ok("  👍 ");
            }
        }
        [Authorize(Roles = "Admin")]


        [HttpDelete("{id}")]
        public IActionResult DeleteMember(int id)
        {
            _memberWriteRepository.Remove(id);
            _memberWriteRepository.Save();


            return Ok("  👍 ");
        }

        

        [HttpPatch]
        public IActionResult Update(UpdateMemberModel updateMemberModel)
        {
            string? mail = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;
            Member updatedMember=_context.Members.FirstOrDefault( a =>a.MailAddress==mail);
            if (updatedMember != null && ModelState.IsValid)
            {
                updatedMember.Password = updateMemberModel.Password;


                _memberWriteRepository.Update(updatedMember);
                _memberWriteRepository.Save();
                return Ok("  👍 ");
            }
            else
            {
                return BadRequest("Incorrect password");

            }

        }
        [HttpGet("GetEvent")]
        public IActionResult GetEvent()
        {
            IQueryable<Event> events = (IQueryable<Event>)_eventReadRepository.GetAll();

            return Ok(events);
            
        }
        [HttpPost("AddEvent")]
        public async Task<IActionResult> AddEvent(Event _event)
        {
            string mail = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

            Member member =   _context.Members.FirstOrDefault(a=>a.MailAddress==mail);
            if (member == null)
            {
                return NotFound();
            }
            else
            {
            _event.Status = "On Hold";
            _event.Member = member;
            await _eventWriteRepository.AddAsync(_event);
            _eventWriteRepository.Save();
            return Ok("  👍 ");

            }

        }
        [HttpPatch("UpdateEvent")]
        public IActionResult UpdateEvent(UpdateEventModel updateEventModel)
        {
            Event @event = _context.Events.FirstOrDefault(a => a.EventId == updateEventModel.EventId);
            string mail = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;
            Member member = _context.Members.FirstOrDefault(a => a.MailAddress == mail);
            if(member.MemberId==@event.MemberId && (@event.EventDate.Day-DateTime.Now.Day)>5)
            {
            @event.Quota = updateEventModel.Quota;
            @event.Address = updateEventModel.Address;
            _eventWriteRepository.Update(@event);
            _eventWriteRepository.Save();
            return Ok("  👍 ");

            }
            else

            {
                if((@event.EventDate.Day - DateTime.Now.Day) > 5)
                return BadRequest(" you cannot update infos until 5 days");
                return BadRequest();
            }
            //Event event31 = member.Event.FirstOrDefault(a => a.EventId == @event.EventId);
            //if ()

            
            
        }
        [HttpDelete("DeleteEvent, {id}")]
        public IActionResult DeleteEvent(int id)
        {
            //Event @event =_context.Events.Find(id);
            if(((_context.Events.Find(id)).EventDate.Day - DateTime.Now.Day) > 5)
            {
                _eventWriteRepository.Remove(id);
                _eventWriteRepository.Save();


                return Ok("  👍 ");
            }
            else
            {
                return BadRequest(" you cannot delete events until 5 days");
            }
        }
        [Authorize(Roles = "Admin")]

        [HttpPatch("SetStatus")]
        public IActionResult SetStatus(StatusModel statusModel)
        {
            Event @event = _context.Events.FirstOrDefault(a => a.EventId == statusModel.EventId);
            @event.Status = statusModel.Status;
            if (@event.Status == "Approved")
            {
                _eventWriteRepository.Update(@event);
                _eventWriteRepository.Save();
                return Ok("Event is approved");

            }
            else if (@event.Status == "Denied")
                return DeleteEvent(statusModel.EventId);
            else
                return BadRequest();
            
        }
        [HttpPost("AddAttendee")]
        public  async Task<IActionResult> AddAttendee([FromBody] int id)
        {
            Event @event = _context.Events.Find(id);
            if (@event == null)
                return NotFound();
            string mail = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;
            Member member = _context.Members.FirstOrDefault(a => a.MailAddress == mail);
            Attendee attendee = new Attendee()
            {
                Member = member,
                EventId = id,
                MemberId = member.MemberId,
                Name=member.Name,
                SurName=member.SurName,
            };
            await _attendeeWriteRepository.AddAsync(attendee);
            _attendeeWriteRepository.Save();
            return Ok("You have successfully participated in the event");
        }
    }
}
