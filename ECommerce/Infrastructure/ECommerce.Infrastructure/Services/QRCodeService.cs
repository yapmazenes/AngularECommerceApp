using ECommerce.Application.Abstractions.Services;
using QRCoder;

namespace ECommerce.Infrastructure.Services
{
    public class QRCodeService : IQRCodeService
    {

        public byte[] GenerateQRCode(string text)
        {
            var generator = new QRCodeGenerator();
            var data = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);

            var qrCode = new PngByteQRCode(data);
            return qrCode.GetGraphic(10, new byte[] { 84, 99, 71 }, new byte[] { 240, 240, 240 });
        }
    }
}
