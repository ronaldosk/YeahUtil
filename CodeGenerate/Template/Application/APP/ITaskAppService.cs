﻿using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using [AppName].[BusinessName]s.Dtos;

namespace [AppName].[BusinessName]s
{
    public interface I[BusinessName]AppService : IApplicationService
    {
        Task<ListResultDto<[BusinessName]ListDto>> GetAll(GetAll[BusinessName]sInput input);

        System.Threading.Tasks.Task Create(Create[BusinessName] Input input);
    }
}