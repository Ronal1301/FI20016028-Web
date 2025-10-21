using Microsoft.AspNetCore.Mvc;
using BinaryCalc.Models;

namespace BinaryCalc.Controllers
{
    public class HomeController : Controller
    {
        // GET /
        [HttpGet("/")]
        public IActionResult Index()
        {
            return View(new BinaryViewModel());
        }

        // POST /
        [HttpPost("/")]
        public IActionResult Index(BinaryViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var a = vm.A;
            var b = vm.B;

            var (aBin, aOct, aDec, aHex) = BinaryCalculator.AllBasesFromBin(a);
            var (bBin, bOct, bDec, bHex) = BinaryCalculator.AllBasesFromBin(b);

            vm.Results.Add(new ResultRow { Label = "a", Bin = vm.A8, Oct = aOct, Dec = aDec, Hex = aHex });
            vm.Results.Add(new ResultRow { Label = "b", Bin = vm.B8, Oct = bOct, Dec = bDec, Hex = bHex });

            var andBin = BinaryCalculator.And(a, b);
            var orBin  = BinaryCalculator.Or(a, b);
            var xorBin = BinaryCalculator.Xor(a, b);

            var andInt = BinaryCalculator.BinToInt(andBin);
            var orInt  = BinaryCalculator.BinToInt(orBin);
            var xorInt = BinaryCalculator.BinToInt(xorBin);

            var andBases = BinaryCalculator.AllBases(andInt);
            var orBases  = BinaryCalculator.AllBases(orInt);
            var xorBases = BinaryCalculator.AllBases(xorInt);

            vm.Results.Add(new ResultRow { Label = "a AND b", Bin = andBases.bin, Oct = andBases.oct, Dec = andBases.dec, Hex = andBases.hex });
            vm.Results.Add(new ResultRow { Label = "a OR b",  Bin = orBases.bin,  Oct = orBases.oct,  Dec = orBases.dec,  Hex = orBases.hex });
            vm.Results.Add(new ResultRow { Label = "a XOR b", Bin = xorBases.bin, Oct = xorBases.oct, Dec = xorBases.dec, Hex = xorBases.hex });

            var sum = BinaryCalculator.Add(a, b);
            var mul = BinaryCalculator.Mul(a, b);

            var sumBases = BinaryCalculator.AllBases(sum);
            var mulBases = BinaryCalculator.AllBases(mul);

            vm.Results.Add(new ResultRow { Label = "a + b", Bin = sumBases.bin, Oct = sumBases.oct, Dec = sumBases.dec, Hex = sumBases.hex });
            vm.Results.Add(new ResultRow { Label = "a • b", Bin = mulBases.bin, Oct = mulBases.oct, Dec = mulBases.dec, Hex = mulBases.hex });

            return View(vm);
        }
    }
}
