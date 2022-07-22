using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace Src
{
    public class BetaController : Controller
    {
        private readonly IFeatureManager _featureManager;

        public BetaController(IFeatureManagerSnapshot featureManager)
        {
            _featureManager = featureManager;
        }

        [FeatureGate(MyFeatureFlags.BlackFridayDeals)]
        public IActionResult Index()
        {
            if (await _featureManager.IsEnabledAsync(MyFeatureFlags.BlackFridayDeals))
            {
                return View();
            }

            return null;

        }
    }
}