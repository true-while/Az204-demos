using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using System.Threading.Tasks;

namespace Src
{
    public class BetaController : Controller
    {
        private readonly IFeatureManager _featureManager;

        public BetaController(IFeatureManagerSnapshot featureManager)
        {
            _featureManager = featureManager;
        }

        [FeatureGate(MyFeatureFlags.BlackFridayDeals)] //check
        public async Task<IActionResult> Index()
        {
            //alternative check
            if (await _featureManager.IsEnabledAsync(MyFeatureFlags.BlackFridayDeals.ToString()))
            {
                return View();
            }

            return null;

        }
    }
}