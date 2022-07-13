﻿namespace Api.Controllers.AccountControllers;

[ApiController]
[Route("api/accounts")]
public partial class AccountController : ControllerBase
{
    private readonly int MAX_ACCOUNTS_IN_SEARCHES = 20;

    private readonly ModelsContext context;
    private readonly Mapper mapper;
    private readonly DataValidator dataValidator;
    private readonly IConfiguration configuration;
    private readonly LevenshteinDistance stringComparer;
    private readonly ExceptionHandler exceptionHandler;

    public AccountController(
        ModelsContext context,
        Mapper mapper,
        DataValidator dataValidator,
        IConfiguration configuration,
        LevenshteinDistance stringComparer,
        ExceptionHandler exceptionHandler)
    {
        this.context = context;
        this.mapper = mapper;
        this.dataValidator = dataValidator;
        this.configuration = configuration;        
        this.stringComparer = stringComparer;
        this.exceptionHandler = exceptionHandler;
    }
}
