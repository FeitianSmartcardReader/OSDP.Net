using System.Collections.Generic;

namespace OSDP.Net.Messages
{
    public class IdReportCommand : Command
    {
        public IdReportCommand(byte address)
        {
            Address = address;
        }

        protected override byte CommandCode => 0x61;

        protected override IEnumerable<byte> GetData()
        {
            return new byte[] { 0x00 };
        }
    }
}