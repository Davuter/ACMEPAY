using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Payment.Core.Application.CQRS.Command.Authorize;
using Payment.Core.Application.CQRS.Command.Capture;
using Payment.Core.Application.CQRS.Command.Void;
using Payment.Core.Application.Exceptions;
using Payment.Core.Domain.Entities;
using Payment.Core.Domain.Enums;
using Payment.Core.Domain.Interfaces;
using PaymentAPI.Controllers;
using PaymentAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PaymentApiTest.Application
{
    public class PaymentWebApiTest
    {
        private readonly Mock<IMediator> _mediatorMock;

        public PaymentWebApiTest()
        {
            _mediatorMock = new Mock<IMediator>();
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

        #region Void Controller Test Methods
        [Fact]
        public async Task Void_payment_with_requestId_success()
        {
            var fakeTransaction = FakeTransaction(TransactionStatus.Voided);

            var fakeVoidResponse = new VoidResponse
            {
                Id = fakeTransaction.PaymentId,
                Status = fakeTransaction.Status
            };
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<VoidCommand>(), default(CancellationToken)))
                .Returns(Task.FromResult(fakeVoidResponse));

            //Act
            var authorizeController = new AuthorizeController(_mediatorMock.Object);

            var actionResult =await authorizeController.Void(fakeTransaction.PaymentId, fakeTransaction.OrderReferenceNumber);

            //Assert
            Assert.Equal(fakeTransaction.PaymentId.ToString(), actionResult.Id.ToString());
            Assert.Equal(TransactionStatus.Voided, fakeTransaction.Status);
        }

        [Fact]
        public async Task Void_payment_NotFound_request()
        {
            var id = Guid.NewGuid();
            var fakeOrderNumber = "123665";
            var fakeExceptionResponse = new NotFoundException("Transactions",id.ToString());
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<VoidCommand>(), default(CancellationToken)))
                .Throws(fakeExceptionResponse);

            //Act
            var authorizeController = new AuthorizeController(_mediatorMock.Object);
            
            var actionResult = await Assert.ThrowsAsync<NotFoundException>(()=> authorizeController.Void(id, fakeOrderNumber));

            //Assert
            Assert.Equal(string.Format("Entity \"Transactions\" ({0}) was not found.", id), actionResult.Message);
        }

        [Fact]
        public async Task Void_payment_Bad_request()
        {
            var id = Guid.Empty;
            var fakeExceptionResponse = new ValidationExceptions(new List<FluentValidation.Results.ValidationFailure> { 
             new FluentValidation.Results.ValidationFailure("id","id is invalid"),
             new FluentValidation.Results.ValidationFailure("orderReference","order reference is invalid")
            });
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<VoidCommand>(), default(CancellationToken)))
                .Throws(fakeExceptionResponse);

            //Act
            var authorizeController = new AuthorizeController(_mediatorMock.Object);

            var actionResult = await Assert.ThrowsAsync<ValidationExceptions>(() => authorizeController.Void(id, string.Empty));

            //Assert
            Assert.Equal(2, actionResult.Failures.Count);
        }

        #endregion

        #region Capture Controller Test Methods
        [Fact]
        public async Task Capture_payment_with_requestId_success()
        {
            var fakeTransaction = FakeTransaction(TransactionStatus.Captured);

            var fakeCaptureResponse = new CaptureResponse
            {
                Id = fakeTransaction.PaymentId,
                Status = fakeTransaction.Status
            };
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<CaptureCommand>(), default(CancellationToken)))
                .Returns(Task.FromResult(fakeCaptureResponse));

            //Act
            var authorizeController = new AuthorizeController(_mediatorMock.Object);

            var actionResult = await authorizeController.Capture(fakeTransaction.PaymentId, fakeTransaction.OrderReferenceNumber);

            //Assert
            Assert.Equal(fakeTransaction.PaymentId.ToString(), actionResult.Id.ToString());
            Assert.Equal(TransactionStatus.Captured, fakeTransaction.Status);
        }

        [Fact]
        public async Task Capture_payment_NotFound_request()
        {
            var id = Guid.NewGuid();
            var fakeOrderNumber = "123665";
            var fakeExceptionResponse = new NotFoundException("Transactions", id.ToString());
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<CaptureCommand>(), default(CancellationToken)))
                .Throws(fakeExceptionResponse);

            //Act
            var authorizeController = new AuthorizeController(_mediatorMock.Object);

            var actionResult = await Assert.ThrowsAsync<NotFoundException>(() => authorizeController.Capture(id, fakeOrderNumber));

            //Assert
            Assert.Equal(string.Format("Entity \"Transactions\" ({0}) was not found.", id), actionResult.Message);
        }

        [Fact]
        public async Task Capture_payment_Bad_request()
        {
            var id = Guid.Empty;
            var fakeExceptionResponse = new ValidationExceptions(new List<FluentValidation.Results.ValidationFailure> {
             new FluentValidation.Results.ValidationFailure("id","id is invalid"),
             new FluentValidation.Results.ValidationFailure("orderReference","order reference is invalid")
            });
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<CaptureCommand>(), default(CancellationToken)))
                .Throws(fakeExceptionResponse);

            //Act
            var authorizeController = new AuthorizeController(_mediatorMock.Object);

            var actionResult = await Assert.ThrowsAsync<ValidationExceptions>(() => authorizeController.Capture(id, string.Empty));

            //Assert
            Assert.Equal(2, actionResult.Failures.Count);
        }

        #endregion

        #region Authorize Controller Test Methods
        [Fact]
        public async Task Authorize_payment_with_requestId_success()
        {
            var fakeTransaction = FakeTransaction(TransactionStatus.Authorized);

            var fakeAuthorizeResponse = new AuthorizeResponse
            {
                Id = fakeTransaction.PaymentId,
                Status = fakeTransaction.Status
            };
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<AuthorizeCommand>(), default(CancellationToken)))
                .Returns(Task.FromResult(fakeAuthorizeResponse));

            //Act
            var authorizeController = new AuthorizeController(_mediatorMock.Object);

            var actionResult = await authorizeController.Authorize(FakeAuthorizeCommand());

            //Assert
            Assert.Equal(fakeTransaction.PaymentId.ToString(), actionResult.Id.ToString());
            Assert.Equal(TransactionStatus.Authorized, fakeTransaction.Status);
        }

        [Fact]
        public async Task Authorize_payment_NotValidData_request()
        {
            var fakecommand = FakeAuthorizeCommand();
            fakecommand.CardExpirationMonth = 7;
            fakecommand.CardExpirationYear = 2020;
            var fakeExceptionResponse = new NotValidDataException("Card Expiration Date Not Valid", new string[] {});
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<AuthorizeCommand>(), default(CancellationToken)))
                .Throws(fakeExceptionResponse);

            //Act
            var authorizeController = new AuthorizeController(_mediatorMock.Object);

            var actionResult = await Assert.ThrowsAsync<NotValidDataException>(() => authorizeController.Authorize(fakecommand));

            //Assert
            Assert.Equal(fakeExceptionResponse.Message, actionResult.Message);
        }

        [Fact]
        public async Task Authorize_payment_Bad_request()
        {
            var fakecommand = FakeAuthorizeCommand();
            fakecommand.Currency = string.Empty;
            var fakeExceptionResponse = new ValidationExceptions(new List<FluentValidation.Results.ValidationFailure> {
             new FluentValidation.Results.ValidationFailure("currency","currency is invalid")
            });
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<AuthorizeCommand>(), default(CancellationToken)))
                .Throws(fakeExceptionResponse);

            //Act
            var authorizeController = new AuthorizeController(_mediatorMock.Object);

            var actionResult = await Assert.ThrowsAsync<ValidationExceptions>(() => authorizeController.Authorize(fakecommand));

            //Assert
            Assert.Equal(1, actionResult.Failures.Count);
        }

        #endregion
    }
}
