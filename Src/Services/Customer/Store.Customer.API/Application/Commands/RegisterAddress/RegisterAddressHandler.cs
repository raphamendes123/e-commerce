﻿using Core.Message;
using Store.Customer.API.Application.Events;
using Store.Customer.API.Domain.Data.Entitys;
using Store.Customer.API.Domain.Data.Repository.Interfaces;
using FluentValidation.Results;
using MediatR;
using Core.SpecificationsUseCase;
using Store.Customer.API.Domain.Data.Contexts;
using Store.Customer.API.Application.Extensions;

namespace Store.Customer.API.Application.Commands
{
    public class RegisterAddressHandler : CommandHandler, IRequestHandler<RegisterAddressCommand, ValidationResult>
    {
        private readonly CustomerDbContext _customerDbContext;
        private readonly ICustomerRepository _customerRepository;

        public RegisterAddressHandler(
            CustomerDbContext customerDbContex,
            ICustomerRepository customerRepository)
        {
            _customerDbContext = customerDbContex;
            _customerRepository = customerRepository;
        }

        public async Task<ValidationResult> Handle(RegisterAddressCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;

            var address = request.ToAddressEntity();
             
            _customerRepository.AddAddress(address); 

            return await PersistData(_customerRepository.UnitOfWork);
        }  
    }
}
