using FinalProject.Application.Repositories;
using FinalProject.Domain.Entities;
using FinalProject.Domain.Entities.ViewModel;
using FinalProject.Persistence.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Company")]

    public class CompanyController : ControllerBase
    {
        private readonly ICompanyReadRepository _companyReadRepository;
        private readonly ICompanyWriteRepository _companyWriteRepository;
        private readonly IEventReadRepository _eventReadRepository;
        private readonly IEventWriteRepository _eventWriteRepository;
        private readonly ITicketReadRepository _ticketReadRepository;
        private readonly ITicketWriteRepository _ticketWriteRepository;
        private readonly OnlineEventsDbContext _context;

        public CompanyController(ICompanyReadRepository companyReadRepository, ICompanyWriteRepository companyWriteRepository, OnlineEventsDbContext context, IEventReadRepository eventReadRepository, IEventWriteRepository eventWriteRepository, ITicketReadRepository ticketReadRepository, ITicketWriteRepository ticketWriteRepository)
        {
            _ticketReadRepository = ticketReadRepository;
            _ticketWriteRepository = ticketWriteRepository;
            _companyReadRepository = companyReadRepository;
            _companyWriteRepository = companyWriteRepository;
            _eventReadRepository = eventReadRepository;
            _eventWriteRepository = eventWriteRepository;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetEvents()
        {
            List<Event> @event = _context.Events.Where(a => a.TicketInfo == true).ToList();


            return Ok(@event);
            //IQueryable<Event> biseyler =_eventReadRepository.GetAll().Where(a => a.TicketInfo == true);
            //return Ok(biseyler);

        }
        //biletolusturma
        // ticket.ticketprice=event.ticketprice*company.comission
        [HttpPost]
        public async Task<IActionResult> AddTicket(AddTicketModel addTicketModel)
        {
            Event @event = _context.Events.FirstOrDefault(a => a.EventId == addTicketModel.EventId);
            string? mail = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;
            Company company = _context.Companies.FirstOrDefault(a => a.MailAddress == mail);
            Ticket _ticket = _context.Tickets.FirstOrDefault(a=>a.EventId==addTicketModel.EventId && a.CompanyId == company.CompanyId );
            if (_ticket == null && @event != null)
            {
                
            Ticket ticket = new Ticket()
            {
                EventId = addTicketModel.EventId,
                CompanyId = company.CompanyId,
                TicketPrice = (@event.TicketPrice * company.Commission) + @event.TicketPrice,
                Event = @event,
                Company = company
            };
                await _ticketWriteRepository.AddAsync(ticket);
                //await _context.Tickets.AddAsync(ticket);
                _ticketWriteRepository.Save();
                return Ok("  👍 ");

            }
            else 
            {
                if (@event != null)
                    return NotFound("event not found");
                return BadRequest("this ticket already created.");
            }

            


        }
    }
}
