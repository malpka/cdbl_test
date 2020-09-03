// AutoMapping.cs
using AutoMapper;
using webapi.Domain;
using webapi.DTOs;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        CreateMap<Email, EmailDTO>();
        CreateMap<EmailDTO, Email>();
    }
}