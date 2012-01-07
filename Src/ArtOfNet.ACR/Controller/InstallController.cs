using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace ArtOfNet.ACR
{
    public class InstallController : RtcControllerBase<InstallArgumentModel>
    {
        public InstallController()
        {
        
        }

        public override ControllerResult Execute()
        {
            if (!string.IsNullOrEmpty(ArgumentModel.RegisterPath))
            {
                //RegistrationHelper.ConsoleMode = true;
                //ActionAfterInstall register = new ActionAfterInstall();
                //register.CustomRegister(ArgumentModel.RegisterPath);
            }
            return new EmptyResult();
        }
    }
}
