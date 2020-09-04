using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private readonly IOptions<AppSettings> _appSettings;

        public EmailController(ILogger<EmailController> logger, IMapper mapper, EmailDbContext dbContext, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
            _appSettings = appSettings;
        }


        [HttpPost(Name = nameof(Create))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(EmailDTO emailData)
        {
            var email =_mapper.Map<Email>(emailData);
            _dbContext.Emails.Add(email);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
            return Ok(email.Id);
        }

        /// <summary>
        /// Set email recipients
        /// </summary>
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CheckStatus(int id)
        {
            var email = await _dbContext.Emails.FindAsync(id);
            if(email == null)
                return NotFound();
            return Ok(email.Status);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            var emails =  _dbContext.Emails.Select(e => _mapper.Map<EmailDTO>(e)).ToList();
            return Ok(emails);
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

        [HttpGet(Name = "SendPending")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SendPending()
        {
            var emailsToSend =  _dbContext.Emails.Where(e => e.Status == EmailStatus.Pending).ToList();
            foreach(var e in emailsToSend)
            {
                MailSender ms = new MailSender(_appSettings);
                ms.Send(e);
                e.Status = EmailStatus.Sent;
            }
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost(Name = nameof(AddAtachment))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAtachment(int emailId, string attachmentName, byte[] attachmentData)
        {
            if(attachmentData.Length > (_appSettings.Value.AttachmentMaxLength ?? 10000))
                return BadRequest();
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
