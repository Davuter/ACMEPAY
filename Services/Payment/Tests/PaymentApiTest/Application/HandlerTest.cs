using AutoMapper;
using MediatR;
using Moq;
using Payment.Core.Application.CQRS.Command.Authorize;
using Payment.Core.Application.CQRS.Command.Void;
using Payment.Core.Application.Exceptions;
using Payment.Core.Domain.Entities;
using Payment.Core.Domain.Enums;
using Payment.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PaymentApiTest.Application
{
    public class HandlerTest
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        public HandlerTest()
        {
            _mediatorMock = new Mock<IMediator>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _mapperMock = new Mock<IMapper>();
        }
        private AuthorizeCommand FakeAuthorizeCommand()
        {
            return new AuthorizeCommand
            {
                Amount = 100,
                CardCvv = 123,
                CardExpirationMonth = 1,
                CardExpirationYear = 2025,
                CardHolderName = "Davut Er",
                CardPan = "123123123123",
                Currency = "EUR",
                OrderReferenceNumber = "1234567890"
            };
        }
        private Transaction FakeTransaction(TransactionStatus status)
        {
            return new Transaction()
            {
                Amount = 100,
                CardCvv = 123,
                CardExpirationMonth = 1,
                CardExpirationYear = 2025,
                CardHolderName = "Davut Er",
                CardPan = "123123123123",
                Currency = "EUR",
                OrderReferenceNumber = "1234567890",
                PaymentId = Guid.NewGuid(),
                Status = status
            };
        }

        #region Authorize Handler Test Methods
        [Fact]
        public async Task Handle_throws_NotValidData_if_cardExpirationDate_is_not_valid()
        {
            //Arrange
            var fakeAuthorizeCommand = FakeAuthorizeCommand();
            fakeAuthorizeCommand.CardExpirationMonth = 7;
            fakeAuthorizeCommand.CardExpirationYear = 2021;

            //Act
            var handler = new AuthorizeCommandHandler(_transactionRepositoryMock.Object, _mapperMock.Object);
            var cltToken = new System.Threading.CancellationToken();
            var result = await Assert.ThrowsAsync<NotValidDataException>(() => handler.Handle(fakeAuthorizeCommand, cltToken));

            //Assert
            Assert.True(result is NotValidDataException);
        }

        [Fact]
        public async Task Handle_Authorize_success()
        {
            //Arrange
            var fakeAuthorizeCommand = FakeAuthorizeCommand();

            var fakeTransaction = FakeTransaction(TransactionStatus.Authorized);

            _mapperMock.Setup(x => x.Map<Transaction>(It.IsAny<AuthorizeCommand>())).Returns(Task.FromResult(fakeTransaction).Result);

            _transactionRepositoryMock.Setup(x => x.Add(It.IsAny<Transaction>())).Returns(Task.FromResult(fakeTransaction.PaymentId));

            _transactionRepositoryMock.Setup(x => x.UnitofWork.SaveEntitiesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));

            //Act
            var handler = new AuthorizeCommandHandler(_transactionRepositoryMock.Object, _mapperMock.Object);
            var cltToken = new System.Threading.CancellationToken();
            var result = await handler.Handle(fakeAuthorizeCommand, cltToken);

            //Assert
            Assert.Equal(result.Id, fakeTransaction.PaymentId);
        }
        #endregion

        #region Void Handler Test Methods
        [Fact]
        public async Task VoidHandle_throws_ValidationException()
        {
            //Arrange
            var fakeVoidCommand = new VoidCommand() { 
             Id = Guid.Empty,
             OrderReference = string.Empty
            };

            //Act
            var handler = new VoidCommandHandler(_transactionRepositoryMock.Object);
            var cltToken = new System.Threading.CancellationToken();
            var result = await Assert.ThrowsAsync<ValidationExceptions>(() => handler.Handle(fakeVoidCommand, cltToken));

            //Assert
            Assert.True(result is ValidationExceptions);
        }

        [Fact]
        public async Task VoidHandle_throws_NotFoundException()
        {
            //Arrange
            var fakeVoidCommand = new VoidCommand()
            {
                Id = Guid.Empty,
                OrderReference = string.Empty
            };

            _transactionRepositoryMock.Setup(x => x.GetAsync(fakeVoidCommand.Id, fakeVoidCommand.OrderReference)).Returns(Task.FromResult((Transaction)null));

            //Act
            var handler = new VoidCommandHandler(_transactionRepositoryMock.Object);
            var cltToken = new System.Threading.CancellationToken();
            var result = await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(fakeVoidCommand, cltToken));

            //Assert
            Assert.True(result is NotFoundException);
        }

        [Fact]
        public async Task Handle_Void_success()
        {
            //Arrange
            var fakeVoidCommand = new VoidCommand()
            {
                Id = Guid.NewGuid(),
                OrderReference = "test"
            };

            var fakeTransaction = FakeTransaction(TransactionStatus.Voided);
            
            _transactionRepositoryMock.Setup(x => x.GetAsync(fakeVoidCommand.Id,fakeVoidCommand.OrderReference)).Returns(Task.FromResult(fakeTransaction));

            _transactionRepositoryMock.Setup(x => x.Update(fakeTransaction));

            _transactionRepositoryMock.Setup(x => x.UnitofWork.SaveEntitiesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));

            //Act
            var handler = new VoidCommandHandler(_transactionRepositoryMock.Object);
            var cltToken = new System.Threading.CancellationToken();
            var result = await handler.Handle(fakeVoidCommand, cltToken);

            //Assert
            Assert.Equal(result.Id, fakeTransaction.PaymentId);
            Assert.Equal(result.Id, fakeTransaction.PaymentId);
        }
        #endregion

    }
}
