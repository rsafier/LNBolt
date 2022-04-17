using LNBolt.BOLT11;
using NUnit.Framework;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LNBolt.Tests
{
    public class Bech32Tests
    {
        [Test]
        public void ValidChecksums()
        {
            string[] valid_checksum =
            {
                "A12UEL5L",
                "an83characterlonghumanreadablepartthatcontainsthenumber1andtheexcludedcharactersbio1tt5tgs",
                "abcdef1qpzry9x8gf2tvdw0s3jn54khce6mua7lmqqqxw",
                "11qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqc8247j",
                "split1checkupstagehandshakeupstreamerranterredcaperred2y9e3w",
            };

            foreach (var encoded in valid_checksum)
            {
                string hrp;
                byte[] data;
                Bech32Engine.Decode(encoded, out hrp, out data);
                Assert.IsNotNull(data, "bech32_decode fails: {0}", encoded);

                var rebuild = Bech32Engine.Encode(hrp, data);
                Assert.IsNotNull(rebuild, "bech32_encode fails: {0}", encoded);
                Assert.AreEqual(encoded.ToLower(), rebuild, "bech32_encode produces incorrect result : {0}", encoded);
            }
        }

        [Test]
        public void InvalidChecksums()
        {
            string[] invalid_checksum =
            {
                "tc1qw508d6qejxtdg4y5r3zarvary0c5xw7kg3g4ty",
                "bc1qw508d6qejxtdg4y5r3zarvary0c5xw7kv8f3t5",
                "BC13W508D6QEJXTDG4Y5R3ZARVARY0C5XW7KN40WF2",
                "bc1rw5uspcuh",
                "bc10w508d6qejxtdg4y5r3zarvary0c5xw7kw508d6qejxtdg4y5r3zarvary0c5xw7kw5rljs90",
                "BC1QR508D6QEJXTDG4Y5R3ZARVARYV98GJ9P",
                "tb1qrp33g0q5c5txsp9arysrx4k6zdkfs4nce4xj0gdcccefvpysxf3q0sL5k7",
            };

            foreach (var encoded in invalid_checksum)
            {
                string hrp;
                byte[] data;
                Bech32Engine.Decode(encoded, out hrp, out data);
                Assert.IsNull(data, "bech32_decode should fail: {0}", encoded);
            }
        }

        [Test]
        public void LNURLDecode()
        {
            string hrp;
            byte[] data;
            var lnurl = "LNURL1DP68GURN8GHJ7UM9WFMXJCM99E3K7MF0V9CXJ0M385EKVCENXC6R2C35XVUKXEFCV5MKVV34X5EKZD3EV56NYD3HXQURZEPEXEJXXEPNXSCRVWFNV9NXZCN9XQ6XYEFHVGCXXCMYXYMNSERXFQ5FNS";
            Bech32Engine.Decode(lnurl, out hrp, out data);
            Assert.AreEqual(hrp, "lnurl");
            var url = UTF8Encoding.UTF8.GetString(data);
            Assert.AreEqual("https://service.com/api?q=3fc3645b439ce8e7f2553a69e5267081d96dcd340693afabe04be7b0ccd178df", url);
        }
    }
}
