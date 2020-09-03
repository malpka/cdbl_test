using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using webapi.Domain;
using webapi.DTOs;
using webapi.Models;

namespace webapi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class EmailController : ControllerBase
    {
        private readonly ILogger<EmailController> _logger;
        private readonly IMapper _mapper;
        private readonly EmailDbContext _dbContext;

        public EmailController(ILogger<EmailController> logger, IMapper mapper, EmailDbContext dbContext)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }


        [HttpPost(Name = nameof(Create))]
        public IActionResult Create(EmailDTO emailData)
        {
            var email =_mapper.Map<Email>(emailData);
            _dbContext.Emails.Add(email);
            return Ok(email.Id);
        }

        [HttpPost(Name = nameof(SetReceipients))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetReceipients(int emailId, string recipients)
        {
            var email = await _dbContext.Emails.FindAsync(emailId);
            if(email == null)
                return NotFound();
            email.Recipients = recipients;
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost(Name = nameof(SetSender))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetSender(int emailId, string sender)
        {
            var email = await _dbContext.Emails.FindAsync(emailId);
            if(email == null)
                return NotFound();
            email.Sender = sender;
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{id}", Name = "CheckStatus")]
        public void CheckStatus(int id)
        {

        }

        [HttpGet]
        public IEnumerable<EmailDTO> Get()
        {
            return null;
        }

        /// <summary>
        /// GET Email by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(typeof(EmailDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var email = await _dbContext.Emails.FindAsync(id);
            if (email == null)
                return NotFound();
            return Ok(_mapper.Map<EmailDTO>(email));
        }

        // [HttpGet("", Name = "SendPending")]
        // public void SendPending()
        // {

        // }

        [HttpPost(Name = nameof(AddAtachment))]
        public async Task<IActionResult> AddAtachment(int emailId, string attachmentName, byte[] attachmentData)
        {
            var email = await _dbContext.Emails.FindAsync(emailId);
            if(email == null)
                return NotFound();
            var attachment = new Attachment() 
            {
                Content = attachmentData,
                Name = attachmentName,
                Email = email
            };
            email.Attachments.Add(attachment);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost(Name = nameof(SetPrioroty))]
        public async Task<IActionResult> SetPrioroty(int emailId, int priority)
        {
            var email = await _dbContext.Emails.FindAsync(emailId);
            if(email == null)
                return NotFound();
            email.Priority = priority;
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
