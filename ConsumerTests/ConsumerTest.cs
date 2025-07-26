using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using PaymentMicroservice;
using Xunit;
using static MessageContracts.MessageContract;

namespace ConsumerTests
{
    public class ConsumerTest : IAsyncLifetime
    {
        private readonly InMemoryTestHarness _harness;
        private ConsumerTestHarness<InvoiceCreatedConsumer> _consumerHarness;

        public ConsumerTest()
        {
            _harness = new InMemoryTestHarness();
        }

        public async Task InitializeAsync()
        {
            _consumerHarness = _harness.Consumer<InvoiceCreatedConsumer>();

            await _harness.Start();
        }

        [Fact]
        public async Task Verify_InvoiceCreatedMessage_Consumed()
        {
            // Act
            await _harness.Bus.Publish<IInvoiceCreated>(new { InvoiceNumber = InVar.Id });

            // Assert
            Assert.True(await _harness.Consumed.Any<IInvoiceCreated>());
            Assert.True(await _consumerHarness.Consumed.Any<IInvoiceCreated>());
            _harness.Published.Select<IInvoiceCreated>().Count().Should().Be(1);
        }

        public async Task DisposeAsync()
        {
            await _harness.Stop();
        }
    }
}