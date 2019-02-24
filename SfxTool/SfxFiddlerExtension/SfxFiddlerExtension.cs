using Fiddler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SfxFiddlerExtension
{
  
  

	public class SfxFiddlerExtension : IAutoTamper    // Ensure class is public, or Fiddler won't see it!
    {
        string sUserAgent = "";

        public SfxFiddlerExtension()
        {
            /* NOTE: It's possible that Fiddler UI isn't fully loaded yet, so don't add any UI in the constructor.

               But it's also possible that AutoTamper* methods are called before OnLoad (below), so be
               sure any needed data structures are initialized to safe values here in this constructor */

            sUserAgent = "SfxFiddlerExtension";
        }

        public void OnLoad() { /* Load your UI here */ }
        public void OnBeforeUnload() { }

        public void AutoTamperRequestBefore(Session oSession)
        {
            oSession.oRequest["User-Agent"] = sUserAgent;
        }
        public void AutoTamperRequestAfter(Session oSession) { }
        public void AutoTamperResponseBefore(Session oSession) { }
        public void AutoTamperResponseAfter(Session oSession) { }
        public void OnBeforeReturningError(Session oSession) { }
    }
}
