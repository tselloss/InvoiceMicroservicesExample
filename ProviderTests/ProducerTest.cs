using FluentAssertions;
using InvoiceMicroservices;
using MassTransit.Testing;
using MessageContracts;
using Xunit;
using static MessageContracts.MessageContract;

namespace ProviderTests
{
    public class ProducerTest : IAsyncLifetime
    {
        private readonly InMemoryTestHarness _harness;
        private ConsumerTestHarness<EventConsumer> _consumerHarness;

        public ProducerTest()
        {
            _harness = new InMemoryTestHarness();
        }

        public async Task InitializeAsync()
        {
            _consumerHarness = _harness.Consumer<EventConsumer>();

            await _harness.Start();
        }

        [Fact]
        public async Task Verify_InvoiceToCreateCommand_Consumed()
        {
            await _harness.InputQueueSendEndpoint.Send<IInvoiceToCreate>(
                new
                {
                    CustomerNumber = 19282,
                    InvoiceItems = new List<InvoiceItems>
                    {
                    new InvoiceItems
                    {
                        Description = "Tables",
                        Price = Math.Round(1020.99),
                        ActualMileage = 40,
                        BaseRate = 12.50,
                        IsHazardousMaterial = false,
                        IsOversized = true,
                        IsRefrigerated = false
                    }
                    }
                });

            // Assert that the message was consumed by the harness
            Assert.True(await _harness.Consumed.Any<IInvoiceToCreate>(), "Message was not consumed by the harness");

            // Assert that the message was handled by the actual consumer
            Assert.True(await _consumerHarness.Consumed.Any<IInvoiceToCreate>(), "Message was not consumed by the consumer");

            // Assert that the IInvoiceCreated message was published
            (await _harness.Published.Any<IInvoiceCreated>()).Should().BeTrue("No IInvoiceCreated message was published");
        }

        [Fact]
        public async Task Should_Publish_InvoiceCreationFailed_When_Exception_Occurs()
        {
            var customerNumber = 12345;

            // Send a message that triggers an exception (InvoiceItems = null)
            await _harness.InputQueueSendEndpoint.Send<IInvoiceToCreate>(
                new
                {
                    CustomerNumber = customerNumber,
                    InvoiceItems = new List<InvoiceItems>()
                });

            var publishedInvoiceCreated = await _harness.Published.Any<IInvoiceCreated>();
            publishedInvoiceCreated.Should().BeFalse();

            var failedEvent = (await _harness.Published.SelectAsync<IInvoiceCreationFailed>().FirstOrDefault());
            failedEvent.Should().NotBeNull();
            failedEvent.Context.Message.Error
                    .Should().Contain($"Error creating invoice for customer {customerNumber}");
        }

        public async Task DisposeAsync()
        {
            await _harness.Stop();
        }
    }
}


