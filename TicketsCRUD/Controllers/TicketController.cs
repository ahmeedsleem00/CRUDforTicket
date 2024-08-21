using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketsCRUD.Core.Context;
using TicketsCRUD.Core.DTO;
using TicketsCRUD.Core.Entites;

namespace TicketsCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TicketController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //CRUD

        //CREATE
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDto createTicketDto)
        {
            var newTicket = new Ticket();
             
            _mapper.Map(createTicketDto , newTicket);
             await _context.Tickets.AddAsync(newTicket);
            await   _context.SaveChangesAsync();
            return Ok("Ticket Saved Success ");

        }

        //Get All
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetTicketDto>>> GetTicket(string? Search)
        {
            //Get Tickets From Context
            IQueryable<Ticket> query = _context.Tickets; 

            //Implement Search
            if(Search != null)
            {
                query = query.Where(t => t.PassengerName.Contains(Search));
            }
            var tickets = await query.ToArrayAsync();

            var convertTicket = _mapper.Map<IEnumerable<GetTicketDto>>(tickets);

            return Ok(convertTicket);

        }

        //Get one by Id
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<GetTicketDto>> GetTicketById([FromRoute] long id)
        {
            //Get Tickets From Context and Check if it exist
            var tickets = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
            if(tickets == null)
            {
                return NotFound("Ticket Not Found!");
            }
            else
            {
                var convertTicket = _mapper.Map<GetTicketDto>(tickets);
                return Ok(convertTicket);
            }
        }

        //Update
        [HttpPut]
        [Route("edit/{id}")]
        public async Task<IActionResult> EditTicket([FromRoute]long id ,[FromBody]UpdateTicketDto updateTicketDto)
        {
            //
            var tickets = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
            if (tickets == null)
            {
                return NotFound("Ticket Not Found!");
            }
            else
            {
                _mapper.Map(updateTicketDto, tickets);
                tickets.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                return Ok("Ticket Updated Success");
            }
        }

        //Delete
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteTicket([FromRoute]long id)
        {
            var tickets = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);

            if (tickets == null)
            {
                return NotFound("Ticket Not Found!");
            }
            else
            {
                _context.Tickets.Remove(tickets);
                await _context.SaveChangesAsync();
                return Ok("The Ticket Was Deleted Successfuly");
            }
        }
    }
}
