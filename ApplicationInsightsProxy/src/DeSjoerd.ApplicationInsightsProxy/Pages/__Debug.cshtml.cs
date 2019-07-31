using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeSjoerd.ApplicationInsightsProxy.DebugModes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DeSjoerd.ApplicationInsightsProxy.Pages
{
    public class DebugModel : PageModel
    {
        public DebugModel(ExceptionModeSetting exceptionModeSetting)
        {
            ExceptionModeSetting = exceptionModeSetting;
        }

        public ExceptionModeSetting ExceptionModeSetting { get; }

        public async Task<IActionResult> OnPostAsync()
        {
            ExceptionModeSetting.Enabled = !ExceptionModeSetting.Enabled;
            return Page();
        }
    }
}