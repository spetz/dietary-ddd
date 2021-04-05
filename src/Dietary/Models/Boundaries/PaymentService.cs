using System.Linq;
using System.Threading.Tasks;

namespace Dietary.Models.Boundaries
{
    public class PaymentService
    {
        private const decimal MinAmountOfOneOrderToBeVip = 100;
        private const int MinAmountOfOrdersToBeVip = 10;

        private readonly IPayerRepository _payerRepository;
        private readonly IClientAddressRemoteService _ordersRemoteService;
        private readonly IOrderRemoteService _orderRemoteService;
        private readonly IClaimsRemoteService _claimsRemoteService;

        public PaymentService(IPayerRepository payerRepository, IClientAddressRemoteService payerAddressRemoteService,
            IOrderRemoteService orderRemoteService, IClaimsRemoteService claimsRemoteService)
        {
            _payerRepository = payerRepository;
            _ordersRemoteService = payerAddressRemoteService;
            _orderRemoteService = orderRemoteService;
            _claimsRemoteService = claimsRemoteService;
        }

        public async Task<bool> PayAsync(PayerId payerId, decimal? amountToPay)
        {
            var payer = await _payerRepository.FindByIdAsync(payerId);
            if (CanAfford(amountToPay, payer))
            {
                await PayAsync(amountToPay, payer);
                return true;
            }
            else if (await PayerIsVipAsync(payer))
            {
                await PayUsingExtraLimit(amountToPay, payer);
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task PayAsync(decimal? amountToPay, Payer payer)
        {
            payer.Pay(amountToPay);
            await _orderRemoteService.InformAboutNewOrderWithPaymentAsync(amountToPay);
        }

        private async Task PayUsingExtraLimit(decimal? amountToPay, Payer payer)
        {
            payer.PayUsingExtraLimit(amountToPay);
            await _orderRemoteService.InformAboutNewOrderWithPaymentAsync(amountToPay);
        }

        private static bool CanAfford(decimal? amountToPay, Payer payer) => payer.Has(amountToPay);

        private async Task<bool> PayerIsVipAsync(Payer payer)
            => await HasEnoughOrdersAsync(payer) && await AddressIsInEuropeAsync(payer) &&
               IsOldEnough(payer) && await NoClaimsByAsync(payer);

        private async Task<bool> NoClaimsByAsync(Payer payer)
            => await _claimsRemoteService.ClientHasNoClaimsAsync(payer.PayerId);

        private static bool IsOldEnough(Payer payer) => payer.IsAtLeast20Yo();

        private async Task<bool> AddressIsInEuropeAsync(Payer payer)
            => (await _ordersRemoteService.GetByPayerIdAsync(payer.PayerId)).IsWithinEurope();

        private async Task<bool> HasEnoughOrdersAsync(Payer payer) =>
            (await _orderRemoteService.GetByPayerIdAsync(payer.PayerId))
            .Count(x => x.IsMoreThan(MinAmountOfOneOrderToBeVip)) > MinAmountOfOrdersToBeVip;
    }
}